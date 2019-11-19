using System;
using System.Threading.Tasks;

namespace Logger
{
    public interface ILogger
    {
        Task Trace(string log);
        Task Error(Exception ex);
        Task Trace(string caption, string log);
    }
}