using Application.Dtos.Login.Request;
using Application.Interfaces;

namespace Api.Endpoints.Login;

public static class LoginEndpoint
{
    public static void MapLoginEndpoint(this WebApplication app)
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
        });

    }
}
