﻿using Cron.VM.Types;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Array = Cron.VM.Types.Array;
using Boolean = Cron.VM.Types.Boolean;

namespace Cron.VM
{
    public abstract class StackItem : IEquatable<StackItem>
    {
        public abstract bool Equals(StackItem other);

        public sealed override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj == this) return true;
            if (obj is StackItem other)
                return Equals(other);
            return false;
        }

        public static StackItem FromInterface<T>(T value)
            where T : class
        {
            return new InteropInterface<T>(value);
        }

        public virtual BigInteger GetBigInteger()
        {
            return new BigInteger(GetByteArray());
        }

        public abstract bool GetBoolean();

        public abstract byte[] GetByteArray();

        public virtual int GetByteLength()
        {
            return GetByteArray().Length;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                foreach (byte element in GetByteArray())
                    hash = hash * 31 + element;
                return hash;
            }
        }

        public virtual string GetString()
        {
            return Encoding.UTF8.GetString(GetByteArray());
        }

        public static implicit operator StackItem(int value)
        {
            return (BigInteger)value;
        }

        public static implicit operator StackItem(uint value)
        {
            return (BigInteger)value;
        }

        public static implicit operator StackItem(long value)
        {
            return (BigInteger)value;
        }

        public static implicit operator StackItem(ulong value)
        {
            return (BigInteger)value;
        }

        public static implicit operator StackItem(BigInteger value)
        {
            return new Integer(value);
        }

        public static implicit operator StackItem(bool value)
        {
            return new Boolean(value);
        }

        public static implicit operator StackItem(byte[] value)
        {
            return new ByteArray(value);
        }

        public static implicit operator StackItem(string value)
        {
            return new ByteArray(Encoding.UTF8.GetBytes(value));
        }

        public static implicit operator StackItem(StackItem[] value)
        {
            return new Array(value);
        }

        public static implicit operator StackItem(List<StackItem> value)
        {
            return new Array(value);
        }
    }
}
