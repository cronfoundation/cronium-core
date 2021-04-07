using Cron.SmartContract.Enumerators;
using Cron.VM;

namespace Cron.SmartContract.Iterators
{
    internal interface IIterator : IEnumerator
    {
        StackItem Key();
    }
}
