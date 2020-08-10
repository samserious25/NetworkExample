using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Telepathy;
using System.Diagnostics;

namespace Gallows.Server
{
    public class RoomPort
    {
        private int port = 1000;
        private int usedCounter = 0;

        public int Port
        {
            get
            {
                if (usedCounter < 2)
                {
                    usedCounter++;
                    return port;
                }
                else
                {
                    port++;
                    usedCounter = 1;
                    return port;
                }
            }
        }

    }

    public class GameServer
    {
        public static Telepathy.Server Listener { get; private set; }
        private static List<PlayerInfo> playersQueue;
        private static List<PlayerInfo> authedPlayers;
        private static MessageParser messageParser;
        private static RoomPort roomPort;
        private static int lastPort = 0;

        public GameServer(int port)
        {
            roomPort = new RoomPort();
            playersQueue = new List<PlayerInfo>();
            authedPlayers = new List<PlayerInfo>();
            messageParser = new MessageParser();
            Listener = new Telepathy.Server();

            Listener.Start(port);
        }

        public void Update()
        {
            while (Listener.GetNextMessage(out Message message))
            {
                switch (message.eventType)
                {
                    case EventType.Connected:
                        AddPlayerToQueue(message);
                        break;
                    case EventType.Data:
                        messageParser.Parse(message.data, message.connectionId);
                        break;
                    case EventType.Disconnected:
                        Console.WriteLine(GetDisconnectedAuthedPlayer(message.connectionId) + " connected to Room");
                        break;
                }
            }

            SendPlayerToRoom();
        }

        private string GetDisconnectedAuthedPlayer(int connectionID)
        {
            PlayerInfo playerInfo = authedPlayers.Where(x => x.ConnectionID == connectionID).FirstOrDefault();
            authedPlayers.Remove(playerInfo);
            return playerInfo.Identity;
        }

        private void AddPlayerToQueue(Message message)
        {
            playersQueue.Add(new PlayerInfo() { ConnectionID = message.connectionId });
        }

        private PlayerInfo PickUpPlayer()
        {
            PlayerInfo playerInfo = playersQueue.Where(x => x.Connected == true).FirstOrDefault();
            playersQueue.Remove(playerInfo);
            return playerInfo;
        }

        public static void SetPlayerReady(int connectionID, string identity)
        {
            PlayerInfo playerInfo = playersQueue.Where(x => x.ConnectionID == connectionID).FirstOrDefault();

            if (playerInfo == null)
                return;

            playerInfo.Connected = true;
            playerInfo.Identity = identity;
            authedPlayers.Add(playerInfo);

            Console.WriteLine(playerInfo.Identity + " Connected");
        }

        private void SendPlayerToRoom()
        {
            PlayerInfo playerInfo = PickUpPlayer();

            if (playerInfo == null)
                return;

            int port = roomPort.Port;

            if (port != lastPort)
            {
                string filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Room.exe");
                Process.Start(filename, port.ToString());

                SendRoomPort(playerInfo.ConnectionID, port);
                lastPort = port;
            }
            else
            {
                SendRoomPort(playerInfo.ConnectionID, port);
            }
        }

        private void SendRoomPort(int connectionID, int port)
        {
            NetworkMessage networkMessage = new NetworkMessage(MessageType.Port, port);
            Listener.Send(connectionID, Parser.Serialize(networkMessage));
        }

        public void Stop() => Listener.Stop();
    }
}
