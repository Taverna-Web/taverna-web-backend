using Microsoft.OpenApi;

namespace Api.Extensions;

public static class ConfiguracaoSwagger
{
    public static IServiceCollection AddConfiguracaoSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Taverna Web API",
                Version = "v1",
                Description = "API de suporte para o ecossistema Taverna Web VTT."
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Insira o token JWT no formato: Bearer {seu_token}",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT"
            });

            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecuritySchemeReference("Bearer", document),
                    new List<string>()
                }
            });
        });

        return services;
    }
}
