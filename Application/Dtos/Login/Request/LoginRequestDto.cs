using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Login.Request;

public record LoginRequestDto
{
    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "O formato do e-mail é inválido.")]
    [StringLength(254, ErrorMessage = "O e-mail deve ter no máximo 254 caracteres.")]
    public string Email { get; init; } = string.Empty;

    [Required(ErrorMessage = "A senha é obrigatória.")]
    [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
    [StringLength(70, ErrorMessage = "A senha deve ter no máximo 70 caracteres.")]
    public string Senha { get; init; } = string.Empty;
}
