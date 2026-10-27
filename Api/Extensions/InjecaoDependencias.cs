using Domain.Interfaces.Repository;
using Persistence.Repository;

namespace Api.Extensions;
public static class InjecaoDependencias
{
    public static IServiceCollection AddInjecaoDependencias(this IServiceCollection services)
    {
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();

        return services;
    }
}
