namespace TwoNil.Data
{
   internal class GameDatabaseLocator
   {
      public static string GetLocation(string gameId)
      {
         return $@"D:\Mijn Databases\TwoNil\{gameId}.db";
      }
   }
}
