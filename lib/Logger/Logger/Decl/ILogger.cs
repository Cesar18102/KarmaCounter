using System;

namespace Logger
{
    public interface ILogger
    {
        void Debug(string log);
        void Error(Exception ex);
    }
}