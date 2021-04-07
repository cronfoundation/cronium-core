using System.Collections.Generic;
using Cron.Network.P2P.Payloads;

namespace Cron.Plugins
{
    public interface IMemoryPoolTxObserverPlugin
    {
        void TransactionAdded(Transaction tx);
        void TransactionsRemoved(MemoryPoolTxRemovalReason reason, IEnumerable<Transaction> transactions);
    }
}
