﻿using Cron.Cryptography;
using Cron.IO;
using System.Collections;
using System.IO;
using System.Linq;

namespace Cron.Network.P2P.Payloads
{
    public class MerkleBlockPayload : BlockBase
    {
        public int TxCount;
        public UInt256[] Hashes;
        public byte[] Flags;

        public override int Size => base.Size + sizeof(int) + Hashes.GetVarSize() + Flags.GetVarSize();

        public static MerkleBlockPayload Create(Block block, BitArray flags)
        {
            MerkleTree tree = new MerkleTree(block.Transactions.Select(p => p.Hash).ToArray());
            tree.Trim(flags);
            byte[] buffer = new byte[(flags.Length + 7) / 8];
            flags.CopyTo(buffer, 0);
            return new MerkleBlockPayload
            {
                Version = block.Version,
                PrevHash = block.PrevHash,
                MerkleRoot = block.MerkleRoot,
                Timestamp = block.Timestamp,
                Index = block.Index,
                ConsensusData = block.ConsensusData,
                NextConsensus = block.NextConsensus,
                Witness = block.Witness,
                TxCount = block.Transactions.Length,
                Hashes = tree.ToHashArray(),
                Flags = buffer
            };
        }

        public override void Deserialize(BinaryReader reader)
        {
            base.Deserialize(reader);
            TxCount = (int)reader.ReadVarInt(Block.MaxTransactionsPerBlock);
            Hashes = reader.ReadSerializableArray<UInt256>(TxCount);
            Flags = reader.ReadVarBytes((TxCount + 7) / 8);
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVarInt(TxCount);
            writer.Write(Hashes);
            writer.WriteVarBytes(Flags);
        }
    }
}
