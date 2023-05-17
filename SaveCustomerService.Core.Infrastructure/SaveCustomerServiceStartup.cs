using Core.Presentation.Infrastructure;

namespace SaveCustomerService.Core.Infrastructure
{
    public class SaveCustomerServiceStartup : BaseStartup
    {
        public SaveCustomerServiceStartup()
        {
            IocManager.Install(null, Configuration);
        }
    }
}
