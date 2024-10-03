using Core.Service;
using System;
using System.Linq;
using System.Reflection;
using AttendanceSystem.DapperServices;
using AttendanceSystem.DatabaseConnectionFactory;
using AttendanceSystem.GenericRepository;
using AttendanceSystem.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AttendanceSystem.Database.Configuration
{
   public class ConfigureDependency
    {
        //AK Do not modify
        public void RegisterDependencies(IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IDapperRepository, DapperRepository>();
            services.AddScoped<IDatabaseFactory, DatabaseFactory>();
            services.AddScoped<IUnitOfWorkManager, UnitOfWorkManager>();
            Assembly ass = typeof(IAccountService).GetTypeInfo().Assembly;

            // get all concrete types which implements IService in BPRS.Service Project
         var allServices = ass.GetTypes().Where(t =>
                t.GetTypeInfo().IsClass &&
                !t.GetTypeInfo().IsAbstract &&
                typeof(IService).IsAssignableFrom(t));

            foreach (var type in allServices)
            {
                var allInterfaces = type.GetInterfaces();
                var mainInterfaces = allInterfaces.Except
                        (allInterfaces.SelectMany(t => t.GetInterfaces()));
                foreach (var itype in mainInterfaces)
                {
                    if (allServices.Any(x => !x.Equals(type) && itype.IsAssignableFrom(x)))
                    {
                        throw new Exception("The " + itype.Name + " type has more than one implementations, please change your filter");
                    }
                    services.AddScoped(itype, type);
                }
            }
        }
    }
}
