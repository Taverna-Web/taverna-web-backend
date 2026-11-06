using Api.Bootstrap;
using Api.Extensions;
using Api.Endpoints.Dados;
using Application.Interfaces;
using Api.Endpoints.Login;
using Api.Endpoints.Cadastro;

DotEnvExtensions.CarregarDotEnv();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer()
                .AddConfiguracaoSwagger()
                .AddConfiguracaoBanco(builder.Configuration)
                .AddAutenticacaoSeguranca(builder.Configuration)
                .AddInjecaoDependencias();

builder.Services.AddSingleton<IDadoService, DadoService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapearDadoEndpoints();

var api = app.MapGroup("/api");
api.MapLoginEndpoint();
api.MapCadastroEndpoint();

app.Run();