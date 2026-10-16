using ContractDigitalSignature.Modules.Roles.Data;
using ContractDigitalSignature.Modules.Roles.Repositories;
using ContractDigitalSignature.Modules.Roles.Services;
using Microsoft.EntityFrameworkCore;

namespace ContractDigitalSignature.Modules.Roles.Common.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Thiếu connection string 'Default'.");
        }

        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddControllers();
        services.AddExceptionHandler<ApiExceptionHandler>();
        services.AddProblemDetails();
        return services;
    }
}
