namespace TwoNil.Logic.Exceptions
{
   public class NotFoundException : BusinessLogicException
   {
      public NotFoundException(string message)
         : base(message)
      {
      }
   }
}
