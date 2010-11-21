namespace Infrastructure.Data
{
    public abstract class UnitOfWorkController : ModelBindingController
    {
        protected readonly IUnitOfWorkFactory _unitOfWorkFactory;

        private IUnitOfWork _unitOfWork;

        protected UnitOfWorkController(IUnitOfWorkFactory unitOfWorkFactory, 
            params ILocatableModelBinder[] binders) : base(binders)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork ?? (_unitOfWork = _unitOfWorkFactory.Create()); }
        }
    }
}
