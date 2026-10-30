namespace Application.Dtos.Cadastro.Command;

public record CadastroCommandDto
{
    public string Nome { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Senha { get; init; } = string.Empty;
}
