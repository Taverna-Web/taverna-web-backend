using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Api.Extensions;

public static class AutenticacaoSeguranca
{
    public static IServiceCollection AddAutenticacaoSeguranca(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string? connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION")
            ?? configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        return services;
    }
}
