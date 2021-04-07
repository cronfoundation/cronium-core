namespace Cron.VM
{
    public interface IScriptTable
    {
        byte[] GetScript(byte[] script_hash);
    }
}
