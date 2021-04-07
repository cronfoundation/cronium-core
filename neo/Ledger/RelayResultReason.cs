namespace Cron.Ledger
{
    public enum RelayResultReason : byte
    {
        Succeed,
        AlreadyExists,
        OutOfMemory,
        UnableToVerify,
        Invalid,
        PolicyFail,
        Unknown,
        Error
    }
}
