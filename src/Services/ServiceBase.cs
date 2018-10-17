using TwoNil.Data;

namespace TwoNil.Services
{
    public abstract class ServiceBase
    {
        protected IUnitOfWorkFactory UowFactory;

        protected ServiceBase(IUnitOfWorkFactory uowFactory)
        {
            UowFactory = uowFactory;
        }
    }
}
