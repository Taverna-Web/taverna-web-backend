using Domain.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Repository;

namespace Api.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AdicionarDependenciasUsuario(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        CarregarVariaveisAmbienteDotEnv();

        string? connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION")
            ?? configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IUsuarioRepository, UsuarioRepository>();

        return services;
    }

    private static void CarregarVariaveisAmbienteDotEnv()
    {
        string diretorioAtual = Directory.GetCurrentDirectory();
        string? diretorioBusca = diretorioAtual;

        while (!string.IsNullOrEmpty(diretorioBusca))
        {
            string caminhoEnv = Path.Combine(diretorioBusca, ".env");
            if (File.Exists(caminhoEnv))
            {
                foreach (string linha in File.ReadAllLines(caminhoEnv))
                {
                    string linhaLimpa = linha.Trim();
                    if (string.IsNullOrEmpty(linhaLimpa) || linhaLimpa.StartsWith('#'))
                        continue;

                    int indiceIgual = linhaLimpa.IndexOf('=');
                    if (indiceIgual <= 0)
                        continue;

                    string chave = linhaLimpa[..indiceIgual].Trim();
                    string valor = linhaLimpa[(indiceIgual + 1)..].Trim().Trim('"');

                    if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(chave)))
                    {
                        Environment.SetEnvironmentVariable(chave, valor);
                    }
                }
                break;
            }

            diretorioBusca = Directory.GetParent(diretorioBusca)?.FullName;
        }
    }
}
