using Application.Dtos.Cadastro.Request;
using Application.Dtos.Cadastro.Response;
using Application.Interfaces;
using Application.Mapper.Cadastro;
using Application.Utils;
using Domain.Exceptions.Common;
using Domain.Interfaces.Repository;

namespace Application.Service;

public class CadastroService(IUsuarioRepository repository) : ICadastroService
{
    private readonly IUsuarioRepository _repository = repository;

    public async Task<CadastroResponseDto> CadastrarUsuarioAsync(CadastroRequestDto request)
    {

        if (await _repository.EmailJaCadastradoRepositoryAsync(request.Email))
        {
            throw new ConflictException("O email fornecido já está cadastrado.");
        }
        var command = CadastroMapper.ToCommand(request);

        var senhaHash = Hash.GerarHashSenha(command.Senha);
        await _repository.AdicionarUsuarioRepositoryAsync(command.Nome, senhaHash, command.Email);


        return new CadastroResponseDto
        {
            Token = Token.GerarToken(
                await _repository.ObterIdUsuarioComEmailRepositoryAsync(command.Email),
                command.Email)
        };
    }
}
