﻿using Cron.Cryptography.ECC;
using Cron.IO.Caching;
using Cron.IO.Data.LevelDB;
using Cron.IO.Wrappers;
using Cron.Ledger;
using LSnapshot = Cron.IO.Data.LevelDB.Snapshot;

namespace Cron.Persistence.LevelDB
{
    internal class DbSnapshot : Snapshot
    {
        private readonly DB db;
        private readonly LSnapshot snapshot;
        private readonly WriteBatch batch;
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

        public DbSnapshot(DB db)
        {
            this.db = db;
            this.snapshot = db.GetSnapshot();
            this.batch = new WriteBatch();
            ReadOptions options = new ReadOptions { FillCache = false, Snapshot = snapshot };
            Blocks = new DbCache<UInt256, BlockState>(db, options, batch, Prefixes.DATA_Block);
            Transactions = new DbCache<UInt256, TransactionState>(db, options, batch, Prefixes.DATA_Transaction);
            Accounts = new DbCache<UInt160, AccountState>(db, options, batch, Prefixes.ST_Account);
            UnspentCoins = new DbCache<UInt256, UnspentCoinState>(db, options, batch, Prefixes.ST_Coin);
            SpentCoins = new DbCache<UInt256, SpentCoinState>(db, options, batch, Prefixes.ST_SpentCoin);
            Validators = new DbCache<ECPoint, ValidatorState>(db, options, batch, Prefixes.ST_Validator);
            Assets = new DbCache<UInt256, AssetState>(db, options, batch, Prefixes.ST_Asset);
            Contracts = new DbCache<UInt160, ContractState>(db, options, batch, Prefixes.ST_Contract);
            Storages = new DbCacheWithTrie<StorageKey, StorageItem>(db, options, batch, Prefixes.ST_Storage);
            StateRoots = new DbCache<UInt32Wrapper, StateRootState>(db, options, batch, Prefixes.ST_StateRoot);
            HeaderHashList = new DbCache<UInt32Wrapper, HeaderHashList>(db, options, batch, Prefixes.IX_HeaderHashList);
            ValidatorsCount = new DbMetaDataCache<ValidatorsCountState>(db, options, batch, Prefixes.IX_ValidatorsCount);
            BlockHashIndex = new DbMetaDataCache<HashIndexState>(db, options, batch, Prefixes.IX_CurrentBlock);
            HeaderHashIndex = new DbMetaDataCache<HashIndexState>(db, options, batch, Prefixes.IX_CurrentHeader);
            StateRootHashIndex = new DbMetaDataCache<RootHashIndex>(db, options, batch, Prefixes.IX_CurrentStateRoot);
        }

        public override void Commit()
        {
            base.Commit();
            db.Write(WriteOptions.Default, batch);
        }

        public override void Dispose()
        {
            snapshot.Dispose();
        }
    }
}
