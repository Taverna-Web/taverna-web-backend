using Application.Dtos.Login.Commands;
using Application.Dtos.Login.Request;
using Riok.Mapperly.Abstractions;

namespace Application.Mapper.Login;

[Mapper]
public static partial class LoginMapper
{
    public static partial LoginCommandDto ToCommand(this LoginRequestDto request);
}

