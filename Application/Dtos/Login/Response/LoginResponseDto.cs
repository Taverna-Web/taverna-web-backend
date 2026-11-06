namespace Application.Dtos.Login.Response;

public record LoginResponseDto
{
    public string Token { get; init; } = string.Empty;
}
