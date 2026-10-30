using System;

namespace Api.Endpoints.Dtos;

public record RespostaDadoDto(
    string Dado, 
    int Resultado, 
    string Metodo, 
    string Mensagem
);
