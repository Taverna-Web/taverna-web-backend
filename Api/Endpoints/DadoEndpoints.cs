using System;
using System.Security.Cryptography;

namespace Api.Endpoints;

public static class DadoEndpoints
{
    public static void MapearDadoEndpoints(this IEndpointRouteBuilder app)
    {
        var grupo = app.MapGroup("/api/taverna");

        List<int> dados = new List<int> {2, 4, 8, 10, 12, 20, 100};

        grupo.MapGet("/rolar-dado", (int lados = 6) => {

            bool valido = dados.Contains(lados);
            if (!valido)
            {
                return Results.BadRequest($"Insira um dado válido para rolar. {string.Join(", ", dados)}");
            }

            int numeroSorteado = RandomNumberGenerator.GetInt32(1, lados + 1);

            return Results.Ok(new
            {
                Dado = $"D{lados}",
                Resultado = numeroSorteado,
                Metodo = "Cryptographically Secure (RandomNumberGenerator)",
                Mensagem = $"Você rolou um dado D{lados} e tirou {numeroSorteado}!"
            });
        });
    }
}
