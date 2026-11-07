namespace Domain.Model;

public class Usuario
{
    public int IdUsuario { get; set; }
    public required string NomeUsuario { get; set; }
    public required string SenhaUsuario { get; set; }
    public required string EnderecoEmailUsuario { get; set; }
    public bool ContaDeletada { get; set; } = false;
}
