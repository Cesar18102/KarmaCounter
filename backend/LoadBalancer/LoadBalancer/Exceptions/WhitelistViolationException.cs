using System;
using System.Net;

namespace LoadBalancer.Exceptions
{
    public class WhitelistViolationException : ResponseException
    {
        public WhitelistViolationException() : base($"Access is not permitted", HttpStatusCode.Forbidden) { }
    }
}