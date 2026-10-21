using Domain.ValueObject;

namespace Domain.Model;

public class Usuario
{
    public int IdUsuario { get; set; }
    public string NomeUsuario { get; set; } = string.Empty;
    public string SenhaUsuario { get; set; } = string.Empty;
    public Email EnderecoEmailUsuario { get; set; } = null!;
    public bool ContaDeletada { get; set; } = false;
}
