using Api.Bootstrap;
using Api.Extensions;

DotEnvExtensions.CarregarDotEnv();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer()
                .AddSwaggerGen()
                .AddAutenticacaoSeguranca(builder.Configuration)
                .AddInjecaoDependencias();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.Run();
