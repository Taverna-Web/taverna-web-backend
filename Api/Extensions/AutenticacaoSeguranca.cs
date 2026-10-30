using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Api.Extensions;

public static class AutenticacaoSeguranca
{
    public static IServiceCollection AddAutenticacaoSeguranca(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string? jwtSecretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY")
            ?? configuration["JWT:SecretKey"];
        string? jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER")
            ?? Environment.GetEnvironmentVariable("ISS")
            ?? configuration["JWT:Issuer"];
        string? jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")
            ?? Environment.GetEnvironmentVariable("AUD")
            ?? configuration["JWT:Audience"];

        if (string.IsNullOrWhiteSpace(jwtSecretKey) ||
            string.IsNullOrWhiteSpace(jwtIssuer) ||
            string.IsNullOrWhiteSpace(jwtAudience))
        {
            throw new InvalidOperationException("As configurações do JWT não foram encontradas.");
        }
        if (jwtSecretKey.Length < 32)
        {
            throw new InvalidOperationException("A chave secreta do JWT deve ter pelo menos 32 caracteres.");
        }

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtIssuer,
                    ValidateAudience = true,
                    ValidAudience = jwtAudience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey)),
                    ClockSkew = TimeSpan.Zero
                };
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();

                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";

                        string mensagemErro = context.AuthenticateFailure switch
                        {
                            SecurityTokenExpiredException => "O token expirou.",
                            SecurityTokenInvalidSignatureException => "Assinatura do token inválida.",
                            SecurityTokenInvalidIssuerException => "Emissor do token inválido.",
                            SecurityTokenInvalidAudienceException => "Audiência do token inválida.",
                            not null => "Falha na autenticação do token.",
                            null => "Token de autenticação não fornecido."
                        };
                        var payload = new
                        {
                            status = StatusCodes.Status401Unauthorized,
                            erro = "NaoAutorizado",
                            mensagem = mensagemErro
                        };
                        await context.Response.WriteAsJsonAsync(payload);
                    }
                };
            });

        services.AddAuthorization();

        return services;
    }
}
