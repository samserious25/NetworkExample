namespace Gallows.Client
{
    public enum MessageType
    {
        PlayerInfo,
        Port,
        Error
    }

    public struct NetworkMessage
    {
        public MessageType MessageType { get; private set; }
        public object Body { get; private set; }

        public NetworkMessage(MessageType messageType, object body)
        {
            MessageType = messageType;
            Body = body;
        }
    }
}
