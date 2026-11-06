using Api.Bootstrap;
using Api.Middleware;
using Api.Extensions;
using Api.Endpoints.Dados;
using Application.Interfaces;
using Api.Endpoints.Login;
using Api.Endpoints.Cadastro;
using Application.Service;

DotEnvExtensions.CarregarDotEnv();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer()
                .AddConfiguracaoSwagger()
                .AddConfiguracaoBanco(builder.Configuration)
                //.AddAutenticacaoSeguranca(builder.Configuration)
                .AddInjecaoDependencias();


builder.Services.AddScoped<IDadoService, DadoService>();

var app = builder.Build();

app.UseMiddleware<GlobalExcecaoMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

//app.UseAuthentication();
//app.UseAuthorization();

var api = app.MapGroup("/api");
api.MapLoginEndpoint();
api.MapCadastroEndpoint();
app.MapRolarDadoEndpoint();

app.Run();