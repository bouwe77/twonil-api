using System;

namespace TwoNil.Logic.Exceptions
{
   public abstract class BusinessLogicException : Exception
   {
      protected BusinessLogicException(string message)
         : base(message)
      {         
      }
   }
}
