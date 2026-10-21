using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Domain.Exceptions.Common;

namespace Application.Utils;

public static class Token
{
    private const string HEADER_PADRAO_JSON = "{\"alg\":\"HS256\",\"typ\":\"JWT\"}";

    /// <summary>
    /// Gera um token JWT assinado com HMAC-SHA256 (HS256).
    /// </summary>
    public static string GerarToken(
        int idUsuario,
        string email,
        string? chaveSecreta = null,
        int expiracaoHoras = 24)
    {
        string segredo = ObterChaveSecreta(chaveSecreta);

        long agoraUnix = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        long expiracaoUnix = DateTimeOffset.UtcNow.AddHours(expiracaoHoras).ToUnixTimeSeconds();

        var payload = new Dictionary<string, object>
        {
            ["sub"] = idUsuario.ToString(),
            ["email"] = email,
            ["iat"] = agoraUnix,
            ["exp"] = expiracaoUnix
        };

        string headerBase64 = CodificarBase64Url(Encoding.UTF8.GetBytes(HEADER_PADRAO_JSON));
        string payloadBase64 = CodificarBase64Url(JsonSerializer.SerializeToUtf8Bytes(payload));

        string dadosAssinatura = $"{headerBase64}.{payloadBase64}";
        string assinaturaBase64 = CalcularAssinaturaHmac(dadosAssinatura, segredo);

        return $"{dadosAssinatura}.{assinaturaBase64}";
    }

    /// <summary>
    /// Verifica se a assinatura e a data de expiração do token JWT são válidas.
    /// </summary>
    public static bool ValidarToken(string token, string? chaveSecreta = null)
    {
        if (string.IsNullOrWhiteSpace(token))
            return false;

        string segredo;
        try
        {
            segredo = ObterChaveSecreta(chaveSecreta);
        }
        catch
        {
            return false;
        }

        string[] partes = token.Split('.');
        if (partes.Length != 3)
            return false;

        try
        {
            string dadosAssinatura = $"{partes[0]}.{partes[1]}";
            string assinaturaEsperada = CalcularAssinaturaHmac(dadosAssinatura, segredo);

            byte[] bytesRecebidos = Encoding.UTF8.GetBytes(partes[2]);
            byte[] bytesEsperados = Encoding.UTF8.GetBytes(assinaturaEsperada);

            if (!CryptographicOperations.FixedTimeEquals(bytesRecebidos, bytesEsperados))
                return false;

            byte[] bytesPayload = DecodificarBase64Url(partes[1]);
            using JsonDocument doc = JsonDocument.Parse(bytesPayload);

            if (!doc.RootElement.TryGetProperty("exp", out JsonElement expElement))
                return false;

            long expUnix = expElement.GetInt64();
            long agoraUnix = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            return expUnix > agoraUnix;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Valida o token JWT e extrai o IdUsuario (claim "sub"), lançando UnauthorizedException se inválido.
    /// </summary>
    public static int ObterIdUsuarioDoToken(string token, string? chaveSecreta = null)
    {
        if (!ValidarToken(token, chaveSecreta))
            throw new UnauthorizedException("Token inválido ou expirado.");

        string[] partes = token.Split('.');
        byte[] bytesPayload = DecodificarBase64Url(partes[1]);
        using JsonDocument doc = JsonDocument.Parse(bytesPayload);

        if (doc.RootElement.TryGetProperty("sub", out JsonElement subElement) &&
            int.TryParse(subElement.GetString(), out int idUsuario))
        {
            return idUsuario;
        }

        throw new UnauthorizedException("Token não contém um identificador de usuário válido.");
    }

    private static string ObterChaveSecreta(string? chaveSecreta)
    {
        string? segredo = !string.IsNullOrWhiteSpace(chaveSecreta)
            ? chaveSecreta
            : Environment.GetEnvironmentVariable("JWT_SECRET_KEY");

        if (string.IsNullOrWhiteSpace(segredo))
            throw new InvalidOperationException("A chave secreta JWT não foi configurada.");

        return segredo;
    }

    private static string CalcularAssinaturaHmac(string dados, string segredo)
    {
        byte[] bytesChave = Encoding.UTF8.GetBytes(segredo);
        byte[] bytesDados = Encoding.UTF8.GetBytes(dados);
        byte[] hash = HMACSHA256.HashData(bytesChave, bytesDados);

        return CodificarBase64Url(hash);
    }

    private static string CodificarBase64Url(byte[] bytes)
    {
        return Convert.ToBase64String(bytes)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }

    private static byte[] DecodificarBase64Url(string entrada)
    {
        string base64 = entrada
            .Replace('-', '+')
            .Replace('_', '/');

        switch (base64.Length % 4)
        {
            case 2:
                base64 += "==";
                break;
            case 3:
                base64 += "=";
                break;
        }

        return Convert.FromBase64String(base64);
    }
}
