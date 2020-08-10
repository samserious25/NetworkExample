using System;
using System.Collections.Generic;
using System.Linq;
using Telepathy;

namespace Gallows.Room
{
    public static class GameRoom
    {
        public static Server Listener;
        public static ServerTimer serverTimer;
        private static readonly double liveTime = 600000;
        private static List<PlayerInfo> players;

        public static void Start(int port)
        {
            players = new List<PlayerInfo>();
            Listener = new Server();
            Listener.Start(port);
            serverTimer = new ServerTimer(liveTime);
            serverTimer.OnTick += Stop;
        }

        public static void Update()
        {
            while (Listener.GetNextMessage(out Message msg))
            {
                switch (msg.eventType)
                {
                    case EventType.Connected:
                        Console.WriteLine("Connected");
                        break;
                    case EventType.Data:
                        MessageParser.Parse(msg.data, msg.connectionId);
                        break;
                    case EventType.Disconnected:
                        OnPlayerDisconnected(msg.connectionId);
                        break;
                }
            }
        }

        private static void OnPlayerDisconnected(int connectionID)
        {
            PlayerInfo player = players.Where(x => x.ConnectionID == connectionID).FirstOrDefault();
            if (player != null)
            {
                player.Connected = false;
                Console.WriteLine(player.Identity + " is offline");
            }
        }

        public static void UpdatePlayerStatus(string identity, int connectionID)
        {
            PlayerInfo player = players.Where(x => x.Identity == identity).FirstOrDefault();

            if (player == null)
            {
                player = new PlayerInfo() { ConnectionID = connectionID, Connected = true, Identity = identity };
                players.Add(player);
                Console.WriteLine(player.Identity + " is online");
            }
            else
            {
                player.ConnectionID = connectionID;
                player.Connected = true;
                Console.WriteLine(player.Identity + " is online");
            }
        }

        public static void Stop()
        {
            //Отослать сообщение о том, что партия закончена
            Listener.Stop();
        }
    }
}
