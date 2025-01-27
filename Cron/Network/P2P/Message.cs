﻿using Cron.Cryptography;
using Cron.IO;
using Cron.Network.P2P.Payloads;
using System;
using System.IO;

namespace Cron.Network.P2P
{
    public class Message : ISerializable
    {
        public const int HeaderSize = sizeof(uint) + 12 + sizeof(int) + sizeof(uint);
        public const int PayloadMaxSize = 0x02000000;

        public static readonly uint Magic = ProtocolSettings.Default.Magic;
        public string Command;
        public uint Checksum;
        public byte[] Payload;

        public int Size => HeaderSize + Payload.Length;

        public static Message Create(string command, ISerializable payload = null)
        {
            return Create(command, payload == null ? new byte[0] : payload.ToArray());
        }

        public static Message Create(string command, byte[] payload)
        {
            return new Message
            {
                Command = command,
                Checksum = GetChecksum(payload),
                Payload = payload
            };
        }

        void ISerializable.Deserialize(BinaryReader reader)
        {
            if (reader.ReadUInt32() != Magic)
                throw new FormatException();
            this.Command = reader.ReadFixedString(12);
            uint length = reader.ReadUInt32();
            if (length > PayloadMaxSize)
                throw new FormatException();
            this.Checksum = reader.ReadUInt32();
            this.Payload = reader.ReadBytes((int)length);
            if (GetChecksum(Payload) != Checksum)
                throw new FormatException();
        }

        private static uint GetChecksum(byte[] value)
        {
            return Crypto.Default.Hash256(value).ToUInt32(0);
        }

        private ISerializable _payload_deserialized = null;
        public T GetPayload<T>() where T : ISerializable, new()
        {
            if (_payload_deserialized is null)
                _payload_deserialized = Payload.AsSerializable<T>();
            return (T)_payload_deserialized;
        }

        public Transaction GetTransaction()
        {
            if (_payload_deserialized is null)
                _payload_deserialized = Transaction.DeserializeFrom(Payload);
            return (Transaction)_payload_deserialized;
        }

        void ISerializable.Serialize(BinaryWriter writer)
        {
            writer.Write(Magic);
            writer.WriteFixedString(Command, 12);
            writer.Write(Payload.Length);
            writer.Write(Checksum);
            writer.Write(Payload);
        }
    }
}
