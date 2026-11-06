using System;
using System.Collections.Frozen;
using System.Security.Cryptography;
using Application.Dtos.Dados.Command;
using Application.Dtos.Dados.Request;
using Application.Dtos.Dados.Response;
using Application.Interfaces;
using Application.Mapper.Dados;
using Domain.Exceptions.Common;

namespace Application.Service;

public class DadoService : IDadoService
{
    private static readonly FrozenSet<int> DadosValidos = new[] {2, 4, 6, 8, 10, 12, 20, 100}.ToFrozenSet();

    //public IEnumerable<int> ObterDadosValidos() => DadosValidos;

    public ResponseDadoDto RolarDado(RolarDadoRequestDto request)
    {
        var command = RolarDadoMapper.ToCommand(request);

        if(!DadosValidos.Contains(command.Lados))
        {
            throw new BadRequestException($"Insira um dado válido para rolar. Opções: {string.Join(", ", DadosValidos)}");
        }

        int numeroSorteado = RandomNumberGenerator.GetInt32(1, command.Lados + 1);
        return new ResponseDadoDto
        {
            Dado = $"D{command.Lados}",
            Resultado = numeroSorteado
        };
    }
}