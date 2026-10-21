using Domain.Exceptions;
using Domain.ValueObject;

namespace Test.Domain;

public class EmailTests
{
    [Theory]
    [InlineData("usuario@exemplo.com")]
    [InlineData("teste.dev@taverna.com.br")]
    [InlineData("mestre+rpg@dominio.org")]
    public void CriarEmail_ComEnderecoValido_DeveInstanciarComSucesso(string enderecoValido)
    {
        var email = new Email(enderecoValido);

        Assert.NotNull(email);
        Assert.Equal(enderecoValido, email.EnderecoEmail);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("invalido")]
    [InlineData("sem-arroba.com")]
    [InlineData("@sem-usuario.com")]
    [InlineData("usuario@sem-tld")]
    public void CriarEmail_ComEnderecoInvalidoOuNulo_DeveLancarInputException(string? enderecoInvalido)
    {
        var exception = Assert.Throws<InputException>(() => new Email(enderecoInvalido!));

        Assert.Equal("Email invalido", exception.Message);
    }

    [Fact]
    public void InputException_DeveHerdarDeDomainException()
    {
        var exception = new InputException("Email invalido");

        Assert.IsAssignableFrom<DomainException>(exception);
    }
}
