using Core.Data.Derived.EntityFramework;
using Core.Data.Derived.EntityFramework.ConfigurationType;
using Core.Data.Model.Infrastructure;
using SaveCustomerService.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Reflection;
using SaveCustomerService.Core.Infrastructure;
using SaveCustomerService.Model.ServiceModel.User;

namespace SaveCustomerService.Data.Repository.Derived.EFSQL
{
    public class SaveCustomerContext : BaseEntityContext
    {
        public SaveCustomerContext() : base(GetOptions()) { }
        public DbSet<CustomersInformation> CustomersInformation { get; set; }
        public static DbContextOptions<SaveCustomerContext> GetOptions()
        {
            if (!IocManager.IsInstalled)
                new SaveCustomerServiceStartup();
            DbContextOptionsBuilder<SaveCustomerContext> optionsBuilder = new DbContextOptionsBuilder<SaveCustomerContext>();
            string connectionString = GetConnectionString();
            if (!string.IsNullOrEmpty(connectionString))
            {
                optionsBuilder.UseSqlServer(connectionString);
                optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                return optionsBuilder.Options;
            }
            throw new Exception("ConnectionString is empty");
        }
        private static string GetConnectionString()
        {
            IConfigurationRoot configuration = IocManager.Resolve<IConfigurationRoot>();
            Console.WriteLine("Providers ==>" + configuration.Providers.First());
            string connectionString = configuration.GetConnectionString("DbSQL");
            return connectionString;
        }
        
    }
}
