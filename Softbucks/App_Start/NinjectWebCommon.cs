using DataModul;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Softbucks.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Softbucks.App_Start.NinjectWebCommon), "Stop")]

namespace Softbucks.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
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
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            //kernel.Bind<IRepository>().ToMethod(p=>new Repository(
            //    new DataClassesSoftbucksDataContext(
            //        @"Data Source=(LocalDb)\v11.0;AttachDbFilename=|DataDirectory|\aspnet-Softbucks-20140302121355.mdf;Initial Catalog=aspnet-Softbucks-20140302121355;Integrated Security=True")));

            //Data Source=ilya.database.windows.net;Initial Catalog=test;Integrated Security=False;User ID=ilya;Password=********;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False
            kernel.Bind<IRepository>().ToMethod(p => new Repository(
                new DataClassesSoftbucksDataContext(
                    @"Data Source=ilya.database.windows.net;Initial Catalog=test2;Persist Security Info=True;User ID=ilya;Password=Twmo3urx")));
        }        
    }
}
