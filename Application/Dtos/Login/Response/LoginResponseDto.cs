namespace Application.Dtos.Login.Response;

public record LoginResponseDto
{
    public required string Token { get; init; }
}
