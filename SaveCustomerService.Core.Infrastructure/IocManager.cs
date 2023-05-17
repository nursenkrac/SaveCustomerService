using Castle.MicroKernel.Lifestyle;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using SaveCustomerService.Core.Dependency;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;

namespace SaveCustomerService.Core.Infrastructure
{
    public class IocManager
    {
        private static WindsorContainer container;
        public static void Install(IHttpContextAccessor httpContextAccessor, IConfigurationRoot configuration)
        {
            if (container != null && IsInstalled)
            {
                return;
            }
            container = new WindsorContainer();

            container.Register(Component.For(typeof(IConfigurationRoot)).Instance(configuration).LifestyleSingleton());
            if (httpContextAccessor != null)
                container.Register(Component.For(typeof(IHttpContextAccessor)).Instance(httpContextAccessor).LifestyleSingleton());
            container.Register(Classes.FromAssemblyInDirectory(new AssemblyFilter(DependecyInstaller._assemblyDirectoryName, mask: DependecyInstaller._mask))
                    .BasedOn<IWindsorInstaller>()
                    .WithServiceBase()
                    .LifestyleTransient());
            foreach (var item in container.ResolveAll<IWindsorInstaller>())
            {
                container.Install(item);

            }

            IsInstalled = true;
        }
        #region Static Methods
        public static bool IsInstalled { get; set; }
        public static T Resolve<T>()
        {
            using (BeginScope())
            {
                return container.Resolve<T>();
            }
        }
        public static void Register<T>(T component)
        {
            container.Register(Component.For(typeof(T)).Instance(component).LifestyleTransient());
        }
        public static IDisposable BeginScope()
        {
            return container.BeginScope();
        }
        public static object Resolve(Type service)
        {
            return container.Resolve(service);
        }
        public static void Dispose()
        {
            container.Dispose();
        }

        public static T[] ResolveAll<T>()
        {
            return container.ResolveAll<T>();
        }

        public static void Release(object instance)
        {
            container.Release(instance);
        }

        #endregion
    }
}
