using System;

namespace LoadBalancer.Log
{
    public interface ILogger
    {
        void Debug(string log);
        void Error(Exception ex);
    }
}