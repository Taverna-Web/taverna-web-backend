using System;
using Application.Dtos.Dados.Request;
using Application.Dtos.Dados.Response;

namespace Application.Interfaces;

public interface IDadoService
{
    ResponseDadoDto RolarDado(RolarDadoRequestDto request);
}