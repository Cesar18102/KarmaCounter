using System;

namespace KarmaCounterServer.DataAccess.Exceptions
{
    public static class DbExceptionParser
    {
        public static Exception ParseDbException(Exception ex)
        {
            if (ex.Message.StartsWith("Violation of UNIQUE KEY"))
                return new DuplicateKeyException();

            return new UnknownDbException();
        }
    }
}