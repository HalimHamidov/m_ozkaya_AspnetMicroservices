using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Models;
using Ordering.Infrastructure.Mail;
using Ordering.Infrastructure.Persistence;
using Ordering.Infrastructure.Repositories;

namespace Ordering.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OrderContext>(option =>
                option.UseSqlServer(configuration.GetConnectionString("OrderingConnectionString")));

            // Mediatr required types, thats why we use typeof
            services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));

            // We don't use Mediatr for below classes, thats why we don't need typeofs
            services.AddScoped<IOrderRepository, OrderRepository>();

            // Will get from the application.json
            services.Configure<EmailSettings>(c => configuration.GetSection("EmailSettings"));
            services.AddTransient<IEmailService, EmailService>();

            return services;
        }
    }
}
