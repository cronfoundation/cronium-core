using Moq;
using Cron.Cryptography.ECC;
using Cron.IO.Wrappers;
using Cron.Ledger;
using Cron.Persistence;
using System;

namespace Cron.UnitTests
{
    public static class TestBlockchain
    {
        private static CronSystem _theCronSystem;

        public static CronSystem InitializeMockCronSystem()
        {
            if (_theCronSystem == null)
            {
                var mockSnapshot = new Mock<Snapshot>();
                mockSnapshot.SetupGet(p => p.Blocks).Returns(new TestDataCache<UInt256, BlockState>());
                mockSnapshot.SetupGet(p => p.Transactions).Returns(new TestDataCache<UInt256, TransactionState>());
                mockSnapshot.SetupGet(p => p.Accounts).Returns(new TestDataCache<UInt160, AccountState>());
                mockSnapshot.SetupGet(p => p.UnspentCoins).Returns(new TestDataCache<UInt256, UnspentCoinState>());
                mockSnapshot.SetupGet(p => p.SpentCoins).Returns(new TestDataCache<UInt256, SpentCoinState>());
                mockSnapshot.SetupGet(p => p.Validators).Returns(new TestDataCache<ECPoint, ValidatorState>());
                mockSnapshot.SetupGet(p => p.Assets).Returns(new TestDataCache<UInt256, AssetState>());
                mockSnapshot.SetupGet(p => p.Contracts).Returns(new TestDataCache<UInt160, ContractState>());
                mockSnapshot.SetupGet(p => p.Storages).Returns(new TestDataCache<StorageKey, StorageItem>());
                mockSnapshot.SetupGet(p => p.StateRoots).Returns(new TestDataCache<UInt32Wrapper, StateRootState>());
                mockSnapshot.SetupGet(p => p.HeaderHashList)
                    .Returns(new TestDataCache<UInt32Wrapper, HeaderHashList>());
                mockSnapshot.SetupGet(p => p.ValidatorsCount).Returns(new TestMetaDataCache<ValidatorsCountState>());
                mockSnapshot.SetupGet(p => p.BlockHashIndex).Returns(new TestMetaDataCache<HashIndexState>());
                mockSnapshot.SetupGet(p => p.HeaderHashIndex).Returns(new TestMetaDataCache<HashIndexState>());
                mockSnapshot.SetupGet(p => p.StateRootHashIndex).Returns(new TestMetaDataCache<RootHashIndex>());

                var mockStore = new Mock<Store>();

                var defaultTx = TestUtils.CreateRandomHashInvocationMockTransaction().Object;
                mockStore.Setup(p => p.GetBlocks()).Returns(new TestDataCache<UInt256, BlockState>());
                mockStore.Setup(p => p.GetTransactions()).Returns(new TestDataCache<UInt256, TransactionState>(
                    new TransactionState
                    {
                        BlockIndex = 1,
                        Transaction = defaultTx
                    }));

                mockStore.Setup(p => p.GetAccounts()).Returns(new TestDataCache<UInt160, AccountState>());
                mockStore.Setup(p => p.GetUnspentCoins()).Returns(new TestDataCache<UInt256, UnspentCoinState>());
                mockStore.Setup(p => p.GetSpentCoins()).Returns(new TestDataCache<UInt256, SpentCoinState>());
                mockStore.Setup(p => p.GetValidators()).Returns(new TestDataCache<ECPoint, ValidatorState>());
                mockStore.Setup(p => p.GetAssets()).Returns(new TestDataCache<UInt256, AssetState>());
                mockStore.Setup(p => p.GetContracts()).Returns(new TestDataCache<UInt160, ContractState>());
                mockStore.Setup(p => p.GetStorages()).Returns(new TestDataCache<StorageKey, StorageItem>());
                mockStore.Setup(p => p.GetHeaderHashList()).Returns(new TestDataCache<UInt32Wrapper, HeaderHashList>());
                mockStore.Setup(p => p.GetStateRoots()).Returns(new TestDataCache<UInt32Wrapper, StateRootState>());
                mockStore.Setup(p => p.GetValidatorsCount()).Returns(new TestMetaDataCache<ValidatorsCountState>());
                mockStore.Setup(p => p.GetBlockHashIndex()).Returns(new TestMetaDataCache<HashIndexState>());
                mockStore.Setup(p => p.GetHeaderHashIndex()).Returns(new TestMetaDataCache<HashIndexState>());
                mockStore.Setup(p => p.GetStateRootHashIndex()).Returns(new TestMetaDataCache<RootHashIndex>());
                mockStore.Setup(p => p.GetSnapshot()).Returns(mockSnapshot.Object);
                mockStore.Setup(p => p.Get(It.IsAny<byte>(), It.IsAny<byte[]>())).Returns(UInt256.Zero.ToArray());
                Console.WriteLine("initialize CronSystem");
                _theCronSystem = new CronSystem(mockStore.Object, null); // new Mock<CronSystem>(mockStore.Object);
            }

            return _theCronSystem;
        }
    }
}