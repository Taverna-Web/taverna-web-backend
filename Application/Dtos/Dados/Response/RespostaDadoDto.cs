using System;

namespace Application.Dtos.Dados.Response;

public record RespostaDadoDto(
    string Dado, 
    int Resultado, 
    string Metodo, 
    string Mensagem
);
