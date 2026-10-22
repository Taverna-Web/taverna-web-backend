using Domain.Interfaces.Repository;
using Persistence.Repository;

namespace Api.Extensions;
public static class InjecaoDependenciasExtensions
{
    public static IServiceCollection InjecaoDependencias(this IServiceCollection services)
    {
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();

        return services;
    }
}
