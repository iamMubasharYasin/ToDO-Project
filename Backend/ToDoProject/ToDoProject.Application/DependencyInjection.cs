using Microsoft.Extensions.DependencyInjection;
using ToDoProject.Application.BLL;
using ToDoProject.Application.Interfaces;

namespace ToDoProject.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddScoped<IProductBLL, ProductBLL>();

            return services;
        }
    }
}