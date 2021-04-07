using Cron.VM;
using Newtonsoft.Json;

namespace Cron.Test.Types
{
    public class VMUTExecutionEngineState
    {
        [JsonProperty]
        public VMState State { get; set; }

        [JsonProperty]
        public VMUTStackItem[] ResultStack { get; set; }

        [JsonProperty]
        public VMUTExecutionContextState[] InvocationStack { get; set; }
    }
}