using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Cron.Test.Types
{
    public class VMUTStackItem
    {
        [JsonProperty]
        public VMUTStackItemType Type { get; set; }

        [JsonProperty]
        public JToken Value { get; set; }
    }
}