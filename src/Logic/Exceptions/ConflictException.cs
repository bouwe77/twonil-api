namespace TwoNil.Logic.Exceptions
{
   public class ConflictException : BusinessLogicException
   {
      public ConflictException(string message)
         : base(message)
      {
      }
   }
}
