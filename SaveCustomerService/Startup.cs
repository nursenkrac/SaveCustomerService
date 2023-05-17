using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SaveCustomerService.Core.Api;
using SaveCustomerService.Core.Infrastructure;
using SaveCustomerService.Logger;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.AspNetCore.Routing;

namespace SaveCustomerService
{
    public class Startup : ApiStartup
    {
        private const string CorsName = "SaveCustomer";
        public override void ConfigureServices(IServiceCollection services)
        {
            Console.WriteLine("burda==>ConfigureServices");
            base.ConfigureServices(services);
            services.AddSwaggerGen(c =>
            {
                //var filePath = Path.Combine(System.AppContext.BaseDirectory, "SaveCustomerService.xml");
                //c.IncludeXmlComments(filePath);
            });
            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                     new CultureInfo("en-US"),
                     new CultureInfo("tr-TR")
                 };

                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

                options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(context =>
                {
                    var currentLanguage = context.Request.Headers["Language"].ToString();
                    var defaultLanguage = string.IsNullOrEmpty(currentLanguage) ? "tr-TR" : currentLanguage == "tr" ? "tr-TR" : "en-US";
                    return Task.FromResult(new ProviderCultureResult(defaultLanguage, defaultLanguage));
                }));
            });
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //.AddJwtBearer();
            services.AddCors(options =>
            {
                options.AddPolicy(CorsName,
                builder => builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader()
                );
            });
            services.AddMvc();

            /*Options*/
            services.AddOptions<RequestResponseLoggerOption>().Bind(Configuration.GetSection("RequestResponseLogger")).ValidateDataAnnotations();
            /*IOC*/
            services.AddSingleton<IRequestResponseLogger, RequestResponseLogger>();
            services.AddScoped<IRequestResponseLogModelCreator, RequestResponseLogModelCreator>();
            /*Filter*/
            services.AddMvc(options =>
            {
                options.Filters.Add(new RequestResponseLoggerActionFilter());
                options.Filters.Add(new RequestResponseLoggerErrorFilter());
            });
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();

            app.UseRequestLocalization(locOptions.Value);
            app.UseCors(CorsName);

            var httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
            IocManager.Install(httpContextAccessor, Configuration);

            string nameSpace = SetNameSpace();
            string swaggerVersion = SetSwaggerVersion();

            UseHandler(app, env);
            //app.UseHttpsRedirection();
            app.UseCookiePolicy();
            app.UseStaticFiles();

            app.UseMiddleware<RequestResponseLoggerMiddleware>();

            /*error manage*/
            app.UseExceptionHandler(c => c.Run(async context =>
            {
                var exception = context.Features
                    .Get<IExceptionHandlerPathFeature>()
                    .Error;
                var response = new { details = "An error occurred" };
                var json = JsonConvert.SerializeObject(response);
                await context.Response.WriteAsync(json);
            }));

            UseSwagger(app, nameSpace, swaggerVersion);
            UseEndpoint(app);
        }
        public override void UseSwagger(IApplicationBuilder app, string nameSpace, string swaggerVersion)
        {
            app.UseSwagger();
            app.UseSwaggerUI(delegate (SwaggerUIOptions c)
            {

                 c.SwaggerEndpoint("/swagger/" + swaggerVersion + "/swagger.json", nameSpace + " " + swaggerVersion);
                //c.SwaggerEndpoint("/kolingazservice/swagger/" + swaggerVersion + "/swagger.json", nameSpace + " " + swaggerVersion);
                c.RoutePrefix = string.Empty;
            });
        }
        public override string SetNameSpace()
        {
            return "SaveCustomerService";
        }
        public override string SetSwaggerVersion()
        {
            return "v1";
        }
        public override void UseEndpoint(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(delegate (IEndpointRouteBuilder endpoints)
            {
                endpoints.MapHealthChecks("/health");
                endpoints.MapControllers().RequireAuthorization();
                //endpoints.MapControllers();
            });
        }
        public override IConfigurationBuilder SetBuilder()
        {
            //$env:ASPNETCORE_ENVIRONMENT='Local' > Migration Package Console Set Command
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true);
            //.AddJsonFile($"appsettings.{environmentName}.json", true, true);
            return builder;
        }
    }
}
