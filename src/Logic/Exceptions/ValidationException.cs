namespace TwoNil.Logic.Exceptions
{
   public class ValidationException : BusinessLogicException
   {
      public ValidationException(string message)
         : base(message)
      {
      }
   }
}
