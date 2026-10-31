using System.Security.Claims;
using Application.Interfaces.Common;
using Domain.ValueObject;

namespace Api.Security;

public class AspNetUser(IHttpContextAccessor accessor) : IAspNetUser
{
    public bool EstaAutenticado =>
        accessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    public int IdUsuario
    {
        get
        {
            var claim = accessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                        ?? accessor.HttpContext?.User?.FindFirst("sub")?.Value;

            return int.TryParse(claim, out int id) ? id : 0;
        }
    }

    public Email? EnderecoEmailUsuario
    {
        get
        {
            var emailClaim = accessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value
                            ?? accessor.HttpContext?.User?.FindFirst("email")?.Value;

            return !string.IsNullOrWhiteSpace(emailClaim) ? new Email(emailClaim) : null;
        }
    }
}
