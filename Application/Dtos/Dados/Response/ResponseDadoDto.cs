using System;

namespace Application.Dtos.Dados.Response;

public record ResponseDadoDto
{
    public string Dado { get; init; } = string.Empty;
    public int Resultado { get; init; }
}