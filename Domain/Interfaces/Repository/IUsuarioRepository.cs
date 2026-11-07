namespace Domain.Interfaces.Repository;

public interface IUsuarioRepository
{
    Task<int> ObterIdUsuarioComEmailRepositoryAsync(string enderecoEmail);
    Task<bool> EmailJaCadastradoRepositoryAsync(string enderecoEmail);
    Task<string> ObterSenhaUsuarioRepositoryAsync(int IdUsuario);
    Task<string> ObterEmailUsuarioRepositoryAsync(int IdUsuario);
    Task<string> ObterNomeUsuarioRepositoryAsync(int IdUsuario);

    Task AdicionarUsuarioRepositoryAsync(string NomeUsuario, string Senha, string enderecoEmail);
    Task RedefinirNomeUsuarioRepositoryAsync(int IdUsuario, string NomeUsuario);
    Task RedefinirSenhaUsuarioRepositoryAsync(int IdUsuario, string Senha);
    Task RedefinirEmailUsuarioRepositoryAsync(int IdUsuario, string enderecoEmail);
    Task DeletarUsuarioRepositoryAsync(int IdUsuario);
}
