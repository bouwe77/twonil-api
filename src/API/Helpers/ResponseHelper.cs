using System.Net.Http;
using Dolores.Exceptions;
using Dolores.Http;
using Newtonsoft.Json;

namespace TwoNil.API.Helpers
{
   public class ResponseHelper
   {
      public static HttpException Get404NotFound(string reason)
      {
         return new HttpNotFoundException(reason);
      }

      public static HttpException Get409Conflict(string reason)
      {
         return GetHttpResponseException(HttpStatusCode.Conflict, reason);
      }

      public static HttpException Get400BadRequest(string reason)
      {
         return new HttpBadRequestException(reason);
      }

      public static HttpException Get501NotImplemented(string reason)
      {
         return new HttpNotImplementedException(reason);
      }

      public static HttpException Get500InternalServerError(string reason)
      {
         return new HttpInternalServerErrorException(reason);
      }

      public static HttpException GetHttpResponseException(HttpStatusCode statusCode, string reason)
      {
         return new HttpException(reason, statusCode);
      }
   }
}