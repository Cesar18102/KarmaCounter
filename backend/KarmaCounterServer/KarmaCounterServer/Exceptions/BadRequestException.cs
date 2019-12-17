using System.Net;
using System.Linq;
using System.Web.Http.ModelBinding;

using Newtonsoft.Json;

namespace KarmaCounterServer.Exceptions
{
    public class BadRequestException : ResponseException
    {
        public BadRequestException(string message) :
            base(message, "", HttpStatusCode.BadRequest) { }

        public BadRequestException(ModelStateDictionary modelState) : 
            base(string.Join(", ", modelState.Where(MS => !modelState.IsValidField(MS.Key)).Select(NV => NV.Key + " is not valid")), "", HttpStatusCode.BadRequest) { }
    }
}