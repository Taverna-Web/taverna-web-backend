using Application.Dtos.Login.Request;
using Application.Mapper.Login;
using Application.Dtos.Login.Response;
using Application.Interfaces;
using Application.Utils;
using Domain.Exceptions.Common;
using Domain.Interfaces.Repository;

namespace Application.Service;

public class LoginService(IUsuarioRepository repository) : ILoginService
{
    private readonly IUsuarioRepository _repository = repository;

    public async Task<LoginResponseDto> AutenticarAsync(LoginRequestDto request)
    {
        int idUsuario = await _repository.ObterIdUsuarioComEmailRepositoryAsync(request.Email);
        var command = LoginMapper.ToCommand(request);

        string senhaArmazenada = await _repository.ObterSenhaUsuarioRepositoryAsync(idUsuario);

        if (!Hash.VerificarSenha(command.Senha, senhaArmazenada))
        {
            throw new UnauthorizedException("Credenciais inválidas.");
        }
        var token = new LoginResponseDto
        {
            Token = Token.GerarToken(idUsuario, request.Email)
        };
        return token;
    }
}
