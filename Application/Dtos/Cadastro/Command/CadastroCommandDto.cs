namespace Application.Dtos.Cadastro.Command;

public record CadastroCommandDto
{
    public required string Nome { get; init; }
    public required string Email { get; init; }
    public required string Senha { get; init; }
}
