namespace Api.Bootstrap;

public static class DotEnvExtensions
{
    public static void CarregarDotEnv()
    {
        string? diretorioBusca = Directory.GetCurrentDirectory();

        while (!string.IsNullOrEmpty(diretorioBusca))
        {
            string caminhoEnv = Path.Combine(diretorioBusca, ".env");
            if (File.Exists(caminhoEnv))
            {
                foreach (string linha in File.ReadAllLines(caminhoEnv))
                {
                    string linhaLimpa = linha.Trim();
                    if (string.IsNullOrEmpty(linhaLimpa) || linhaLimpa.StartsWith('#'))
                        continue;

                    int indiceIgual = linhaLimpa.IndexOf('=');
                    if (indiceIgual <= 0)
                        continue;

                    string chave = linhaLimpa[..indiceIgual].Trim();
                    string valor = linhaLimpa[(indiceIgual + 1)..].Trim().Trim('"');

                    if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(chave)))
                    {
                        Environment.SetEnvironmentVariable(chave, valor);
                    }
                }
                break;
            }

            diretorioBusca = Directory.GetParent(diretorioBusca)?.FullName;
        }
    }
}
