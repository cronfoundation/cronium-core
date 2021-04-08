using Cron.Network.P2P;
using Cron.Network.P2P.Payloads;

namespace Cron.Plugins
{
    public interface IP2PPlugin
    {
        bool OnP2PMessage(Message message);
        bool OnConsensusMessage(ConsensusPayload payload);
    }
}
