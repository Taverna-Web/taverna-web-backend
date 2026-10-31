using Application.Dtos.Cadastro.Request;
using Application.Dtos.Cadastro.Response;
using Application.Interfaces;

namespace Api.Endpoints.Cadastro;

public static class CadastroEndpoint
{
    public static void MapCadastroEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/cadastro")
            .WithTags("Cadastro");

        group
            .MapPost("",
            async (CadastroRequestDto request,
            ICadastroService service) =>
        {
            var response = await service.CadastrarUsuarioAsync(request);
            return Results.Ok(response);
        })
        .WithName("CadastrarUsuario")
        .WithSummary("Cadastra um novo usuário no sistema e retorna o token de acesso")
        .Produces<CadastroResponseDto>(200)
        .ProducesProblem(400)
        .ProducesProblem(409)
        .ProducesProblem(500);
    }
}
