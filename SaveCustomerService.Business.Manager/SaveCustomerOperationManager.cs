using Core.Data.Model.Infrastructure;
using Core.Mapper.Infrastructure;
using Core.Model.Infrastructure;
using Core.Model.Infrastructure.Pager;
using Core.Operation.Derived.ValidationOperation;
using Core.Operation.Manager;
using SaveCustomerService.Core.Infrastructure;
using SaveCustomerService.Mapper.Infrastructure;

namespace SaveCustomerService.Business.Manager
{
    public abstract class SaveCustomerOperationManager<TEntity, TModel, TPagerInputModel> : BaseOperationManager<TEntity, TModel, TPagerInputModel>
        where TEntity : class, IEntity, new()
        where TModel : class, IModel, new()
        where TPagerInputModel : class, IPagerInputModel, new()
    {
        public override IMapping SetMapper()
        {
            return IocManager.Resolve<IMapper>();
        }
        public override BaseValidationOperation<TModel> SetDeleteValidationOperation(TModel dto)
        {
            return null;
        }
    }
}
