using System;
using Api.Endpoints.Dtos;
using Api.Services;

namespace Api.Endpoints;

public static class DadoEndpoints
{
    
    public static void MapearDadoEndpoints(this IEndpointRouteBuilder app)
    {
        var grupo = app.MapGroup("/api/taverna")
                       .WithTags("Dados");


        grupo.MapGet("/dado/{lados:int}", (IDadoService dadoService, int lados) =>
        {
            var (sucesso, resultado, erro) = dadoService.RolarDado(lados);

            if (!sucesso)
            {
                return Results.BadRequest(new RespostaErroDto(
                    Erro: "Dado inválido.", 
                    Mensagem: erro ?? "Erro desconhecido."
                ));
            }

            return Results.Ok(new RespostaDadoDto(
            
                Dado: $"D{lados}",
                Resultado: resultado!.Value,
                Metodo: "Cryptographically Secure (RandomNumberGenerator)",
                Mensagem: $"Você rolou um dado D{lados} e tirou {resultado}!"
            ));
        })
        .WithName("RolarDadoPorTipo")
        .Produces<RespostaDadoDto>(StatusCodes.Status200OK)
        .Produces<RespostaErroDto>(StatusCodes.Status400BadRequest);
    }
}
