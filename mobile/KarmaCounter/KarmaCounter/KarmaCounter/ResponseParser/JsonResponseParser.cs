using System;
using System.Collections.Generic;

using KarmaCounter.Models;
using KarmaCounter.Server.Output;

using Newtonsoft.Json;

namespace KarmaCounter.ResponseParser
{
    public class JsonResponseParser : IResponseParser
    {
        public T Parse<T, E>(IServerResponse modelElementJSON) where T : IModelElement
                                                               where E : Exception
        {
            E ex = null;

            try { ex = JsonConvert.DeserializeObject<E>(modelElementJSON.Data); }
            catch { return JsonConvert.DeserializeObject<T>(modelElementJSON.Data); }

            throw ex;
        }

        public IEnumerable<T> ParseCollection<T, E>(IServerResponse modelElementJSON) where T : IModelElement
                                                                                   where E : Exception
        {
            E ex = null;

            try { ex = JsonConvert.DeserializeObject<E>(modelElementJSON.Data); }
            catch { return JsonConvert.DeserializeObject<List<T>>(modelElementJSON.Data); }

            throw ex;
        }
    }
}
