using System;
using System.Collections.Generic;

using KarmaCounter.Models;
using KarmaCounter.Server.Output;

namespace KarmaCounter.ResponseParser
{
    public interface IResponseParser
    {
        T Parse<T, E>(IServerResponse modelElementJSON) where T : IModelElement
                                                        where E : Exception;

        IEnumerable<T> ParseCollection<T, E>(IServerResponse modelElementJSON) where T : IModelElement
                                                                               where E : Exception;
    }
}
