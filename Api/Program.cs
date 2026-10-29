using Api.Bootstrap;
using Api.Extensions;

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

app.Run();
