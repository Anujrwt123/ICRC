using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using ICRC.Data.Infrastructure;
using Autofac;
using Autofac.Integration.Mvc;
using ICRC.Data;
using ICRC.Data.Repositories;
using ICRCService;
using System.Web.Mvc;

namespace IC_RC.App_Start
{
    public class Bootstrapper
    {

        public static void Run()
        {
            SetAutoFacContainer();
        }

        public static void SetAutoFacContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<DbFactory>().As<IDbFactory>().InstancePerRequest();

            // Repositories
            builder.RegisterAssemblyTypes(typeof(CertificatesRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerRequest();
            // Services
            builder.RegisterAssemblyTypes(typeof(CertificateService).Assembly)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces().InstancePerRequest();

            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }
    }
}