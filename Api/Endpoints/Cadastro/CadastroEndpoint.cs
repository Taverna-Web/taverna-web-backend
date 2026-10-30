using Application.Dtos.Cadastro.Request;
using Application.Interfaces;

namespace Api.Endpoints.Cadastro;

public static class CadastroEndpoint
{
    public static void MapCadastroEndpoint(this WebApplication app)
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
        });

    }
}
