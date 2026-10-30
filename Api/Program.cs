using Api.Bootstrap;
using Api.Extensions;
using Api.Endpoints.Login;
using Api.Endpoints.Cadastro;

DotEnvExtensions.CarregarDotEnv();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer()
                .AddSwaggerGen()
                .AddConfiguracaoBanco(builder.Configuration)
                .AddAutenticacaoSeguranca(builder.Configuration)
                .AddInjecaoDependencias();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapLoginEndpoint();
app.MapCadastroEndpoint();

app.Run();
