using System;
using Application.Interfaces;
using Application.Dtos.Dados.Response;
using Application.Dtos.Dados.Request;

namespace Api.Endpoints.Dados;

public static class DadoEndpoints
{
    
    public static void MapRolarDadoEndpoint(this IEndpointRouteBuilder app)
    {
        var grupo = app.MapGroup("/api/taverna")
                       .WithTags("Dados")
                       .RequireAuthorization();


        grupo.MapGet("/dado/{lados:int}", ([AsParameters] RolarDadoRequestDto request, IDadoService service) =>
        {
            var response = service.RolarDado(request);

            return Results.Ok(response);
        })
        .WithName("RolarDadoPorTipo")
        .WithSummary("Rola um dado com o número de lados especificado")
        .Produces<ResponseDadoDto>(200)
        .ProducesProblem(400)
        .ProducesProblem(401)
        .ProducesProblem(500);
    }
    // adicionar endpoint de vezes consecutivas e retorna a lista de valores e a soma deles
}
