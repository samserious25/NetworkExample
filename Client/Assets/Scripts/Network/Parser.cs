using GameDevWare.Serialization;
using System.IO;

namespace Gallows.Client
{
    public static class Parser
    {
        public static byte[] Serialize(NetworkMessage obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                MsgPack.Serialize(obj, stream);
                return stream.ToArray();
            }
        }

        public static NetworkMessage Deserialize(byte[] data)
        {
            using (MemoryStream stream = new MemoryStream(data))
            {
                return MsgPack.Deserialize<NetworkMessage>(stream);
            }
        }
    }
}
