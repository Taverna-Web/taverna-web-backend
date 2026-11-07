using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Domain.Exceptions.Common;

namespace Api.Extensions;

public static class ConfiguracaoBanco
{
    public static IServiceCollection AddConfiguracaoBanco(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
        ?? throw new NotFoundException("Not found connection string do banco de dados");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        return services;
    }
}
