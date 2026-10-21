using System.Text.RegularExpressions;
using Domain.Exceptions;

namespace Domain.ValueObject;

public sealed class Email
{
    private const string PADRAO_EMAIL = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
    public string EnderecoEmail { get; private set; }

    public Email(string enderecoEmail)
    {
        this.EnderecoEmail = !string.IsNullOrWhiteSpace(enderecoEmail) && Regex.IsMatch(enderecoEmail, PADRAO_EMAIL)
            ? enderecoEmail
            : throw new InputException("Email invalido");
    }
}
