using System;

namespace Cron.Interface
{
    public interface ICronLogger
    {
        void Debug(string message);

        void Info(string message);

        void Warning(string message);

        void Trace(string message);

        void Error(string message);
        
        void Error(Exception e, string error);
        
        void Error(Exception e);
    }
}