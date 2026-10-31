using Application.Dtos.Login.Request;
using Application.Dtos.Login.Response;
using Application.Interfaces;

namespace Api.Endpoints.Login;

public static class LoginEndpoint
{
    public static void MapLoginEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/login")
            .WithTags("Login");

        group
            .MapPost("",
            async (LoginRequestDto request,
            ILoginService service) =>
        {
            var response = await service.AutenticarAsync(request);
            return Results.Ok(response);
        })
        .WithName("RealizarLogin")
        .WithSummary("Autentica o usuário e retorna o token de acesso")
        .Produces<LoginResponseDto>(200)
        .ProducesProblem(400)
        .ProducesProblem(401)
        .ProducesProblem(500);
    }
}
