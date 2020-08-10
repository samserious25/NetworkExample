using Telepathy;
using UnityEngine;

namespace Gallows.Client
{
    public static class GameClient
    {
        public static Telepathy.Client Listener;
        public static string PlayerIdentity;
        private static string hostAdress;
        private static int standartPort;
        private static ServerTimer serverTimer;
        private static int roomPort;
        private static readonly double connectionTimeout = 20000;
        private static int connectionTryCount = 2;

        public static void Initialize(string adress, int port, string identity)
        {
            hostAdress = adress;
            standartPort = port;
            PlayerIdentity = identity;
            Listener = new Telepathy.Client();
            ConnectToPort(port);
        }

        public static void ConnectToPort(int port)
        {
            Stop();

            roomPort = port;           
            serverTimer = new ServerTimer(1000);
            serverTimer.OnTick += CheckConnection;
        }

        private static void CheckConnection()
        {
            if (!Listener.Connected)
            {
                float time = 0;
                while (!Listener.Connected)
                {
                    Listener.Connect(hostAdress, roomPort);
                    time++;

                    if (time == connectionTimeout)
                    {
                        if (connectionTryCount > 0)
                        {
                            Listener.Connect(hostAdress, standartPort);
                            Debug.Log("No connection to room, connecting to master");
                            connectionTryCount--;
                        }
                        else
                        {
                            connectionTryCount = 2;
                            //Отправить сообщение об ошибке подключения или добавить бота
                        }

                        break;
                    }
                }

                connectionTryCount = 2;
                serverTimer.Stop();
                serverTimer = null;
            }
        }

        public static void Update()
        {
            if (Listener.Connected)
            {
                while (Listener.GetNextMessage(out Message msg))
                {
                    switch (msg.eventType)
                    {
                        case Telepathy.EventType.Connected:
                            Debug.Log("Connected");
                            Listener.Send(Parser.Serialize(new NetworkMessage(MessageType.PlayerInfo, PlayerIdentity)));
                            break;
                        case Telepathy.EventType.Data:
                            MessageParser.Parse(msg.data, msg.connectionId);
                            break;
                        case Telepathy.EventType.Disconnected:
                            Debug.Log("Disconnected");
                            break;
                    }
                }
            }
        }

        public static void Stop()
        {
            if (Listener != null)
                Listener.Disconnect();
        }
    }
}
