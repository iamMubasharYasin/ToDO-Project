using Microsoft.Extensions.DependencyInjection;
using ToDoProject.Application.Interfaces;
using ToDoProject.Infrastructure.Services;

namespace ToDoProject.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services)
        {
            services.AddScoped<IEmailService, EmailService>();
            return services;
        }
    }
}
