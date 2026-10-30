using System;
using System.Collections.Frozen;
using System.Security.Cryptography;

namespace Api.Services;

public interface IDadoService
{
    (bool Sucesso, int? Resultado, string? Erro) RolarDado(int lados);
    IEnumerable<int> ObterDadosValidos();
}

public class DadoService : IDadoService
{
    private static readonly FrozenSet<int> DadosValidos = new[] {2, 4, 6, 8, 10, 12, 20, 100}.ToFrozenSet();

    public IEnumerable<int> ObterDadosValidos() => DadosValidos;

    public (bool Sucesso, int? Resultado, string? Erro) RolarDado(int lados)
    {
        if(!DadosValidos.Contains(lados))
        {
            return (false, null, $"Insira um dado válido para rolar. Opções: {string.Join(", ", DadosValidos)}");
        }

        int numeroSorteado = RandomNumberGenerator.GetInt32(1, lados + 1);
        return (true, numeroSorteado, null);
    }
}