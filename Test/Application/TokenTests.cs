using Domain.Exceptions.Common;
using Application.Utils;

namespace Test.Application;

public class TokenTests
{
    private const string CHAVE_TESTE = "a1b2c3d4e5f60718293a4b5c6d7e8f90a1b2c3d4e5f60718293a4b5c6d7e8f90";

    [Fact]
    public void GerarToken_EValidarComMesmaChave_DeveRetornarTrueEExtrairIdUsuario()
    {
        string jwt = Token.GerarToken(42, "mestre@taverna.com", CHAVE_TESTE);

        bool valido = Token.ValidarToken(jwt, CHAVE_TESTE);
        int idExtraido = Token.ObterIdUsuarioDoToken(jwt, CHAVE_TESTE);

        Assert.True(valido);
        Assert.Equal(42, idExtraido);
    }

    [Fact]
    public void ValidarToken_ComChaveIncorreta_DeveRetornarFalse()
    {
        string jwt = Token.GerarToken(10, "jogador@taverna.com", CHAVE_TESTE);
        const string chaveErrada = "0000000000000000000000000000000000000000000000000000000000000000";

        bool valido = Token.ValidarToken(jwt, chaveErrada);

        Assert.False(valido);
    }

    [Fact]
    public void ValidarToken_Expirado_DeveRetornarFalse()
    {
        string jwtExpirado = Token.GerarToken(10, "jogador@taverna.com", CHAVE_TESTE, expiracaoHoras: -1);

        bool valido = Token.ValidarToken(jwtExpirado, CHAVE_TESTE);

        Assert.False(valido);
        Assert.Throws<UnauthorizedException>(() => Token.ObterIdUsuarioDoToken(jwtExpirado, CHAVE_TESTE));
    }

    [Theory]
    [InlineData("")]
    [InlineData("token.invalido")]
    [InlineData("parte1.parte2.assinaturaAdulterada")]
    public void ValidarToken_ComFormatoOuAssinaturaInvalida_DeveRetornarFalse(string tokenInvalido)
    {
        bool valido = Token.ValidarToken(tokenInvalido, CHAVE_TESTE);

        Assert.False(valido);
    }
}
