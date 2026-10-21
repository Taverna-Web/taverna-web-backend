using Application.Utils;

namespace Test.Application;

public class HashTests
{
    [Fact]
    public void GerarHashSenha_ParaMesmaSenha_DeveGerarHashesDiferentesDevidoAoSalt()
    {
        const string senha = "SenhaSegura@123";

        string hash1 = Hash.GerarHashSenha(senha);
        string hash2 = Hash.GerarHashSenha(senha);

        Assert.NotEmpty(hash1);
        Assert.NotEmpty(hash2);
        Assert.NotEqual(hash1, hash2); // O Salt garante que os hashes são únicos
    }

    [Fact]
    public void VerificarSenha_ComSenhaCorreta_DeveRetornarTrue()
    {
        const string senha = "SenhaSegura@123";
        string hash = Hash.GerarHashSenha(senha);

        bool resultado = Hash.VerificarSenha(senha, hash);

        Assert.True(resultado);
    }

    [Fact]
    public void VerificarSenha_ComSenhaIncorreta_DeveRetornarFalse()
    {
        const string senhaCorreta = "SenhaSegura@123";
        const string senhaErrada = "SenhaIncorreta@123";
        string hash = Hash.GerarHashSenha(senhaCorreta);

        bool resultado = Hash.VerificarSenha(senhaErrada, hash);

        Assert.False(resultado);
    }

    [Theory]
    [InlineData("", "qualquerHash")]
    [InlineData("senha", "")]
    [InlineData("senha", "hashInvalidoSemDoisPontos")]
    [InlineData("senha", "saltNaoHex:hashNaoHex")]
    public void VerificarSenha_ComEntradasInvalidas_DeveRetornarFalse(string senha, string hashInvalido)
    {
        bool resultado = Hash.VerificarSenha(senha, hashInvalido);

        Assert.False(resultado);
    }

    [Fact]
    public void CalculaHash_ParaMesmaEntrada_DeveRetornarMesmoHashSHA256()
    {
        const string entrada = "texto-teste";

        string hash1 = Hash.CalculaHash(entrada);
        string hash2 = Hash.CalculaHash(entrada);

        Assert.Equal(hash1, hash2);
        Assert.Equal(64, hash1.Length); // 256 bits = 64 caracteres hex
    }
}
