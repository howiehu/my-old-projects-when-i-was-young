[assembly: WebActivator.PreApplicationStartMethod(typeof(EEDDMS.WebSite.App_Start.NinjectMVC3), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(EEDDMS.WebSite.App_Start.NinjectMVC3), "Stop")]

namespace EEDDMS.WebSite.App_Start
{
    using System.Reflection;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Mvc;
    using EEDDMS.Domain.Abstract;
    using EEDDMS.Domain.Concrete;

    public static class NinjectMVC3 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestModule));
            DynamicModuleUtility.RegisterModule(typeof(HttpApplicationInitializationModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<ICollectorRepository>().To<EFCollectorRepository>();
            kernel.Bind<ICollectorRecordRepository>().To<EFCollectorRecordRepository>();
            kernel.Bind<IManufacturerRepository>().To<EFManufacturerRepository>();
            kernel.Bind<IEquipmentRepository>().To<EFEquipmentRepository>();
            kernel.Bind<IEquipmentClassRepository>().To<EFEquipmentClassRepository>();
            kernel.Bind<IEquipmentDetailRepository>().To<EFEquipmentDetailRepository>();
            kernel.Bind<IEquipmentRecordRepository>().To<EFEquipmentRecordRepository>();
            kernel.Bind<ILocationRepository>().To<EFLocationRepository>();
            kernel.Bind<IUnitRepository>().To<EFUnitRepository>();
        }        
    }
}
