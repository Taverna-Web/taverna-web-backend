using Application.Dtos.Cadastro.Request;
using Application.Dtos.Cadastro.Response;
using Application.Interfaces;
using Application.Mapper.Cadastro;
using Application.Utils;
using Domain.Exceptions.Common;
using Domain.Interfaces.Repository;
using Domain.ValueObject;

namespace Application.Service;

public class CadastroService(IUsuarioRepository repository) : ICadastroService
{
    private readonly IUsuarioRepository _repository = repository;

    public async Task<CadastroResponseDto> CadastrarUsuarioAsync(CadastroRequestDto request)
    {
        var command = CadastroMapper.ToCommand(request);
        var email = new Email(command.Email);

        if (await _repository.EmailJaCadastradoRepositoryAsync(email))
        {
            throw new ConflictException("O email fornecido já está cadastrado.");
        }
        var senhaHash = Hash.GerarHashSenha(command.Senha);
        await _repository.AdicionarUsuarioRepositoryAsync(command.Nome, senhaHash, email);


        return new CadastroResponseDto
        {
            Token = Token.GerarToken(await _repository.ObterIdUsuarioComEmailRepositoryAsync(email), email.EnderecoEmail)
        };
    }
}
