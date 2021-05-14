﻿using Cron.IO;
using Cron.IO.Json;
using Cron.Persistence;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cron.IO.Caching;
using Cron.Ledger;

namespace Cron.Network.P2P.Payloads
{
    public class InvocationTransaction : Transaction
    {
        public byte[] Script;
        public Fixed8 Gas;

        public override int Size => base.Size  // Transaction base size
           + Script.GetVarSize()               // Script variable size
           + (Version >= 1 ? Gas.Size : 0);    // Gas Fixed8 size (for version >= 1)

        public override Fixed8 SystemFee => Gas;

        public AssetState Asset { get; private set; }
        
        public InvocationTransaction()
            : base(TransactionType.InvocationTransaction)
        {
        }

        protected override void DeserializeExclusiveData(BinaryReader reader)
        {
            if (Version > 1) throw new FormatException();
            Script = reader.ReadVarBytes(65536);
            if (Script.Length == 0) throw new FormatException();
            if (Version >= 1)
            {
                Gas = reader.ReadSerializable<Fixed8>();
                if (Gas < Fixed8.Zero) throw new FormatException();
            }
            else
            {
                Gas = Fixed8.Zero;
            }
        }

        public static Fixed8 GetGas(Fixed8 consumed, Fixed8 freeAmount)
        {
            Fixed8 gas = consumed - freeAmount;
            if (gas <= Fixed8.Zero) return Fixed8.Zero;
            return gas.Ceiling();
        }

        protected override void SerializeExclusiveData(BinaryWriter writer)
        {
            writer.WriteVarBytes(Script);
            if (Version >= 1)
                writer.Write(Gas);
        }

        public void SetAsset(DataCache<UInt256, AssetState> assetCache)
        {
            var asset = assetCache?.TryGet(Hash);
            if(asset == null)
                return;
            Asset = asset;
        }
        
        public override JObject ToJson()
        {
            var json = base.ToJson();
            json["script"] = Script.ToHexString();
            json["gas"] = Gas.ToString();
            json["invocationData"] = InvocationDataJson();
            return json;
        }
        
        // added for cron-tracker
        private JObject InvocationDataJson()
        {
            JObject invocationData = new JObject();
            var result = new JObject();

            var stack = new JArray();
            var stackObj = new JObject();
            stackObj["type"] = "Integer";
            stackObj["value"] = "1";
            stack.Add(stackObj);
            
            result["state"] = 1;
            result["gas_consumed"] = 0;
            result["gas_cost"] = 0;
            result["stack"] = stack;
            
            invocationData["result"] = result;
            invocationData["contracts"] = new JArray();
            invocationData["deletedContractHashes"] = new JArray();
            invocationData["migratedContractHashes"] = new JArray();
            invocationData["voteUpdates"] = new JArray();
            invocationData["actions"] = new JArray();
            invocationData["storageChanges"] = new JArray();

            if (Asset != null)
            {
                invocationData["asset"] = Asset.ToJson();
            }
            
            return invocationData;
        }

        public override bool Verify(Snapshot snapshot, IEnumerable<Transaction> mempool)
        {
            if (Gas.GetData() % 100000000 != 0) return false;
            if (References is null || NetworkFee < ProtocolSettings.Default.MinimumNetworkFee) return false;
            return base.Verify(snapshot, mempool);
        }
    }
}
