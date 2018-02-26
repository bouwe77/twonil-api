namespace TwoNil.API.Helpers
{
   public class RequestHelper
   {
      public static void ValidateId(string id)
      {
         // As long as the ID is not empty it's OK.
         bool isValid = !string.IsNullOrWhiteSpace(id);
         if (!isValid)
         {
            throw ResponseHelper.Get400BadRequest($"The request parameter '{id}' is not a valid ID");
         }
      }
   }
}