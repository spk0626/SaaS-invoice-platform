using Invoice.Application.Common.Interfaces;
using Invoice.Application.Common.Settings;
using Invoice.Domain.Interfaces;
using Invoice.Infrastructure.BackgroundJobs;
using Invoice.Infrastructure.Persistence;
using Invoice.Infrastructure.Persistence.Repositories;
using Invoice.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Invoice.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(opts =>
            opts.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(
                    typeof(AppDbContext).Assembly.FullName)));

        services
            .AddOptions<EmailSettings>()
            .BindConfiguration(EmailSettings.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();

        services.AddTransient<IEmailService, EmailService>();

        services.AddHostedService<OverdueInvoiceDetector>();

        return services;
    }
}