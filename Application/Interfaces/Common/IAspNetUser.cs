using Domain.ValueObject;

namespace Application.Interfaces.Common;

public interface IAspNetUser
{
    int IdUsuario { get; }
    Email? EnderecoEmailUsuario { get; }
}
