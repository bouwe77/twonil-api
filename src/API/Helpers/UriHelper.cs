using System.Text.RegularExpressions;

namespace TwoNil.API.Helpers
{
   public class LinkHelper
   {
      public static string GetIdFromUri(string uri, string pattern)
      {
         Match match = Regex.Match(uri, pattern, RegexOptions.IgnoreCase);

         string id = null;
         if (match.Success)
         {
            id = match.Groups[1].Value;
         }

         return id;
      }
   }
}