using System.IO;

namespace Cron.IO
{
    public interface ISerializable
    {
        int Size { get; }

        void Serialize(BinaryWriter writer);
        void Deserialize(BinaryReader reader);
    }
}
