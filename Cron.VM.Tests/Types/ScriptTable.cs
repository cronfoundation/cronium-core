using System.Collections.Generic;
using Cron.Test.Extensions;
using Cron.VM;

namespace Cron.Test.Types
{
    public class ScriptTable : IScriptTable
    {
        private Dictionary<string, byte[]> _data = new Dictionary<string, byte[]>();

        public byte[] GetScript(byte[] script_hash)
        {
            if (!_data.TryGetValue(script_hash.ToHexString(), out var ret))
            {
                return null;
            }

            return ret;
        }

        public void Add(byte[] script)
        {
            _data.Add(Crypto.Default.Hash160(script).ToHexString(), script);
        }
    }
}