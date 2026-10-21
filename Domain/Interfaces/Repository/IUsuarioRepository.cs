using Domain.ValueObject;

namespace Domain.Interfaces.Repository;

public interface IUsuarioRepository
{
    Task<int> ObterIdUsuarioComEmailRepositoryAsync(Email enderecoEmail);
    Task<string> ObterSenhaUsuarioRepositoryAsync(int IdUsuario);
    Task<string> ObterEmailUsuarioRepositoryAsync(int IdUsuario);
    Task<string> ObterNomeUsuarioRepositoryAsync(int IdUsuario);

    Task AdicionarUsuarioRepositoryAsync(string NomeUsuario, string Senha, Email enderecoEmail);
    Task RedefinirNomeUsuarioRepositoryAsync(int IdUsuario, string NomeUsuario);
    Task RedefinirSenhaUsuarioRepositoryAsync(int IdUsuario, string Senha);
    Task RedefinirEmailUsuarioRepositoryAsync(int IdUsuario, Email enderecoEmail);
    Task DeletarUsuarioRepositoryAsync(int IdUsuario);
}
