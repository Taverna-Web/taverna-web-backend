using Api.Extensions;
using Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer()
                .AddSwaggerGen()
                .AdicionarDependencias(builder.Configuration)
                .InjecaoDependencias();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.MapearDadoEndpoints();

app.Run();
