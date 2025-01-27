﻿using Cron.Network.P2P.Payloads;
using System;

namespace Cron.Wallets
{
    public class TransferOutput
    {
        public UIntBase AssetId;
        public BigDecimal Value;
        public UInt160 ScriptHash;

        public bool IsGlobalAsset => AssetId.Size == 32;

        public TransactionOutput ToTxOutput()
        {
            if (AssetId is UInt256 asset_id)
                return new TransactionOutput
                {
                    AssetId = asset_id,
                    Value = Value.ToFixed8(),
                    ScriptHash = ScriptHash
                };
            throw new NotSupportedException();
        }
    }
}
