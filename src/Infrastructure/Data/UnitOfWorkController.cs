using System.Web.Mvc;

namespace Infrastructure.Data
{
    public abstract class UnitOfWorkController : Controller
    {
        protected readonly IUnitOfWorkFactoryProxy _proxy;

        private IUnitOfWork _unitOfWork;

        protected UnitOfWorkController(IUnitOfWorkFactoryProxy proxy)
        {
            _proxy = proxy;
        }

        public IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork ?? (_unitOfWork = _proxy.Create()); }
        }
    }
}
