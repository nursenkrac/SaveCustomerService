using Core.Presentation.Api;
using SaveCustomerService.Core.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace SaveCustomerService.Core.Api
{
    public abstract class ApiStartup : BaseApiStartup
    {
        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
            IocManager.Install(httpContextAccessor, Configuration);
            base.Configure(app, env);
        }
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
        }
    }
}
