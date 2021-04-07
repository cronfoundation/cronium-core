using System.Collections.Generic;
using Cron.Test.Converters;
using Newtonsoft.Json;

namespace Cron.Test.Types
{
    public class VMUTEntry
    {
        [JsonProperty]
        public IList<VMUTScriptEntry> ScriptTable { get; set; }

        [JsonProperty, JsonConverter(typeof(ScriptConverter))]
        public byte[] Message { get; set; }

        [JsonProperty, JsonConverter(typeof(ScriptConverter))]
        public byte[] Script { get; set; }

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public VMUTStep[] Steps { get; set; }
    }
}