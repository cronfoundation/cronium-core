using Cron.Test.Converters;
using Newtonsoft.Json;

namespace Cron.Test.Types
{
    public class VMUTScriptEntry
    {
        [JsonProperty, JsonConverter(typeof(ScriptConverter))]
        public byte[] Script { get; set; }
    }
}