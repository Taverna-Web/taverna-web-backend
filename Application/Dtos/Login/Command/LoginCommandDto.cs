namespace Application.Dtos.Login.Command;

public record LoginCommandDto
{
    public required string Email { get; init; }
    public required string Senha { get; init; }
}
