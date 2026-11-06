namespace Application.Dtos.Login.Command;

public record LoginCommandDto
{
    public string Email { get; init; } = string.Empty;
    public string Senha { get; init; } = string.Empty;
}
