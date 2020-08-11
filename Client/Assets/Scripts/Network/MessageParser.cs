using System;

namespace Gallows.Server
{
    public static class MessageParser
    {
        public static void Parse(byte[] data, int connectionID)
        {
            NetworkMessage message = Parser.Deserialize(data);
            Process(message, connectionID);     
        }

        private static void Process(NetworkMessage message, int connectionID)
        {
            switch (message.MessageType)
            {
                case MessageType.Port:
                    GameClient.ConnectToPort(Convert.ToInt32(message.Body));
                    break;
            }
        }
    }
}
