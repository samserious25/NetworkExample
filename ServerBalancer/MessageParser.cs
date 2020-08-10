using System;

namespace Gallows.Server
{
    public class MessageParser
    {
        public Action<string, int> OnPlayerIdentity = delegate { };

        public void Parse(byte[] data, int connectionID)
        {
            Process(Parser.Deserialize(data), connectionID);
        }

        private void Process(NetworkMessage message, int connectionID)
        {
            switch (message.MessageType)
            {
                case MessageType.PlayerInfo:
                    GameServer.SetPlayerReady(connectionID, (string)message.Body);
                    break;
            }
        }
    }
}
