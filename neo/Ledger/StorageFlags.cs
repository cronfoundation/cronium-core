﻿using System;

namespace Cron.Ledger
{
    [Flags]
    public enum StorageFlags : byte
    {
        None = 0,
        Constant = 0x01
    }
}
