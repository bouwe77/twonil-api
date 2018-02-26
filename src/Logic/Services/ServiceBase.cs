using Randomization;

namespace TwoNil.Logic.Services
{
   public abstract class ServiceBase
   {
      protected INumberRandomizer NumberRandomizer;
      protected IListRandomizer ListRandomizer;
      protected ServiceFactory ServiceFactory;

      protected ServiceBase()
      {
         NumberRandomizer = new NumberRandomizer();
         ListRandomizer = new ListRandomizer();
         ServiceFactory = new ServiceFactory();
      }
   }
}
