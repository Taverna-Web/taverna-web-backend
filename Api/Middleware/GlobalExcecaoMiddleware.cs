using Domain.Exceptions.Common;

namespace Api.Middleware;

public class GlobalExcecaoMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await TratarErroAsync(context, ex);
        }
    }

    private static Task TratarErroAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, mensagem) = exception switch
        {
            NotFoundException notFound     => (StatusCodes.Status404NotFound, notFound.Message),
            BadRequestException badRequest => (StatusCodes.Status400BadRequest, badRequest.Message),
            ConflictException conflict     => (StatusCodes.Status409Conflict, conflict.Message),
            UnauthorizedException unauth   => (StatusCodes.Status401Unauthorized, unauth.Message),
            _ => (StatusCodes.Status500InternalServerError, "Ocorreu um erro interno inesperado no servidor.")
        };

        context.Response.StatusCode = statusCode;

        var resposta = new
        {
            Status = statusCode,
            Mensagem = mensagem
        };

        return context.Response.WriteAsJsonAsync(resposta);
    }
}
