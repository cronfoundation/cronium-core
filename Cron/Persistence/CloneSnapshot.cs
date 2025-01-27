﻿using Cron.Cryptography.ECC;
using Cron.IO.Caching;
using Cron.IO.Wrappers;
using Cron.Ledger;

namespace Cron.Persistence
{
    internal class CloneSnapshot : Snapshot
    {
        public override DataCache<UInt256, BlockState> Blocks { get; }
        public override DataCache<UInt256, TransactionState> Transactions { get; }
        public override DataCache<UInt160, AccountState> Accounts { get; }
        public override DataCache<UInt256, UnspentCoinState> UnspentCoins { get; }
        public override DataCache<UInt256, SpentCoinState> SpentCoins { get; }
        public override DataCache<ECPoint, ValidatorState> Validators { get; }
        public override DataCache<UInt256, AssetState> Assets { get; }
        public override DataCache<UInt160, ContractState> Contracts { get; }
        public override DataCache<StorageKey, StorageItem> Storages { get; }
        public override DataCache<UInt32Wrapper, StateRootState> StateRoots { get; }
        public override DataCache<UInt32Wrapper, HeaderHashList> HeaderHashList { get; }
        public override MetaDataCache<ValidatorsCountState> ValidatorsCount { get; }
        public override MetaDataCache<HashIndexState> BlockHashIndex { get; }
        public override MetaDataCache<HashIndexState> HeaderHashIndex { get; }
        public override MetaDataCache<RootHashIndex> StateRootHashIndex { get; }

        public CloneSnapshot(Snapshot snapshot)
        {
            this.PersistingBlock = snapshot.PersistingBlock;
            this.Blocks = snapshot.Blocks.CreateSnapshot();
            this.Transactions = snapshot.Transactions.CreateSnapshot();
            this.Accounts = snapshot.Accounts.CreateSnapshot();
            this.UnspentCoins = snapshot.UnspentCoins.CreateSnapshot();
            this.SpentCoins = snapshot.SpentCoins.CreateSnapshot();
            this.Validators = snapshot.Validators.CreateSnapshot();
            this.Assets = snapshot.Assets.CreateSnapshot();
            this.Contracts = snapshot.Contracts.CreateSnapshot();
            this.Storages = snapshot.Storages.CreateSnapshot();
            this.StateRoots = snapshot.StateRoots.CreateSnapshot();
            this.HeaderHashList = snapshot.HeaderHashList.CreateSnapshot();
            this.ValidatorsCount = snapshot.ValidatorsCount.CreateSnapshot();
            this.BlockHashIndex = snapshot.BlockHashIndex.CreateSnapshot();
            this.HeaderHashIndex = snapshot.HeaderHashIndex.CreateSnapshot();
            this.StateRootHashIndex = snapshot.StateRootHashIndex.CreateSnapshot();
        }
    }
}
