using System;

namespace KarmaCounter.Util
{
    public static class Formatter
    {
        public static string Stringify(this DateTime dt) => $"{dt.Year}-{(dt.Month < 10 ? "0" : "") + dt.Month}-{(dt.Day < 10 ? "0" : "") + dt.Day}";
    }
}
