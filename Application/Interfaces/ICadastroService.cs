using Application.Dtos.Cadastro.Request;
using Application.Dtos.Cadastro.Response;
namespace Application.Interfaces;
public interface ICadastroService
{
    Task<CadastroResponseDto> CadastrarUsuarioAsync(CadastroRequestDto request);
}
