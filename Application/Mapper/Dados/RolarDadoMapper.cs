using System;
using Application.Dtos.Dados.Command;
using Application.Dtos.Dados.Request;
using Riok.Mapperly.Abstractions;

namespace Application.Mapper.Dados;

[Mapper]
public static partial class RolarDadoMapper
{
    public static partial RolarDadoCommandDto ToCommand(this RolarDadoRequestDto request);
}
