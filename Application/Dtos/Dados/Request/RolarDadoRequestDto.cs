using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Dados.Request;

public record RolarDadoRequestDto
{
    [Required(ErrorMessage = "O número de lados é obrigatório.")]
    [AllowedValues(2, 4, 6, 8, 10, 12, 20, 100, ErrorMessage = "Lados permitidos: 2, 4, 6, 8, 10, 12, 20 ou 100.")]
    public int Lados { get; init; }
}
