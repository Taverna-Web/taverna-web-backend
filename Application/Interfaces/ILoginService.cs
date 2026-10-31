using Application.Dtos.Login.Request;
using Application.Dtos.Login.Response;

namespace Application.Interfaces;

public interface ILoginService
{
    Task<LoginResponseDto> AutenticarAsync(LoginRequestDto request);
}
