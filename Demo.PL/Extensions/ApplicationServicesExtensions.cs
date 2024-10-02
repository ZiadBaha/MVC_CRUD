using Castle.Core.Configuration;
using Demp.BLL;
using Demp.BLL.Interfaces;
using Demp.BLL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.PL.Extensions
{
    public  static class ApplicationServicesExtensions
    {

        public static IServiceCollection AddApplicationServices( this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork,UnitOfWork > ();
            return services;
        }

    }
}
