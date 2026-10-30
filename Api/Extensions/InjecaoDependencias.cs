using Domain.Interfaces.Repository;
using Persistence.Repository;
using Application.Interfaces.Common;
using Api.Security;

namespace Api.Extensions;
public static class InjecaoDependencias
{
    public static IServiceCollection AddInjecaoDependencias(this IServiceCollection services)
    {

        services.AddScoped<IUsuarioRepository, UsuarioRepository>();

        services.AddHttpContextAccessor();
        services.AddScoped<IAspNetUser, AspNetUser>();
        
        return services;
    }
}
