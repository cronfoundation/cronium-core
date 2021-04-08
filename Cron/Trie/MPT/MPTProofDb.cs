using Cron.IO;
using Cron.Cryptography;
using System.Collections.Generic;

namespace Cron.Trie.MPT
{
    public class MPTProofStore : IKVReadOnlyStore
    {
        private Dictionary<byte[], byte[]> store = new Dictionary<byte[], byte[]>(ByteArrayEqualityComparer.Default);

        public MPTProofStore(HashSet<byte[]> proof)
        {
            foreach (byte[] data in proof)
            {
                store.Add(Crypto.Default.Hash256(data), data);
            }
        }

        public byte[] Get(byte[] hash)
        {
            var result = store.TryGetValue(hash, out byte[] value);
            return result ? value : null;
        }
    }
}
