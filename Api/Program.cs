using DotNetEnv;
using Api.Middleware;
using Api.Extensions;
using Api.Endpoints.Login;
using Api.Endpoints.Cadastro;

Env.TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer()
                .AddConfiguracaoSwagger()
                .AddConfiguracaoBanco(builder.Configuration)
                .AddAutenticacaoSeguranca(builder.Configuration)
                .AddInjecaoDependencias();

var app = builder.Build();

app.UseMiddleware<GlobalExcecaoMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

var api = app.MapGroup("/api");

api.MapLoginEndpoint();
api.MapCadastroEndpoint();

app.Run();
