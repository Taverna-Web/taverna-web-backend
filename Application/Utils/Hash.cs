using System.Security.Cryptography;
using System.Text;

namespace Application.Utils;

public static class Hash
{
    private const int TAMANHO_SALT = 16;
    private const int TAMANHO_HASH = 32;
    private const int ITERACOES = 100_000;
    private static readonly HashAlgorithmName Algoritmo = HashAlgorithmName.SHA256;

    /// <summary>
    /// Gera um hash SHA256 rápido para validações genéricas (ex: tokens temporários, checksums).
    /// </summary>
    public static string CalculaHash(string entrada)
    {
        byte[] bytesTexto = Encoding.UTF8.GetBytes(entrada);
        byte[] bytesHash = SHA256.HashData(bytesTexto);

        return Convert.ToHexString(bytesHash).ToLowerInvariant();
    }

    /// <summary>
    /// Gera um hash criptograficamente seguro para senhas usando PBKDF2 com Salt embutido.
    /// </summary>
    public static string GerarHashSenha(string senha)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(TAMANHO_SALT);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(senha, salt, ITERACOES, Algoritmo, TAMANHO_HASH);

        // Armazena o Salt e o Hash juntos separados por delimitador
        return $"{Convert.ToHexString(salt)}:{Convert.ToHexString(hash)}";
    }

    /// <summary>
    /// Verifica se a senha em texto puro corresponde ao hash gerado com Salt.
    /// </summary>
    public static bool VerificarSenha(string senhaDigitada, string hashSalvo)
    {
        if (string.IsNullOrWhiteSpace(senhaDigitada) || string.IsNullOrWhiteSpace(hashSalvo))
            return false;

        string[] partes = hashSalvo.Split(':');
        if (partes.Length != 2)
            return false;

        try
        {
            byte[] salt = Convert.FromHexString(partes[0]);
            byte[] hashOriginal = Convert.FromHexString(partes[1]);

            byte[] hashTeste = Rfc2898DeriveBytes.Pbkdf2(senhaDigitada, salt, ITERACOES, Algoritmo, TAMANHO_HASH);

            return CryptographicOperations.FixedTimeEquals(hashOriginal, hashTeste);
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
