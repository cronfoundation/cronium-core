﻿using Cron.Network.P2P.Payloads;

namespace Cron.IO.Caching
{
    internal class RelayCache : FIFOCache<UInt256, IInventory>
    {
        public RelayCache(int max_capacity)
            : base(max_capacity)
        {
        }

        protected override UInt256 GetKeyForItem(IInventory item)
        {
            return item.Hash;
        }
    }
}
