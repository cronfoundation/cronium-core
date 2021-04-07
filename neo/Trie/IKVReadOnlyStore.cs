
namespace Cron.Trie
{
    public interface IKVReadOnlyStore
    {
        byte[] Get(byte[] key);
    }
}
