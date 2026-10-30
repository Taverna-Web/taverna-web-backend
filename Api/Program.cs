using Api.Extensions;
using Api.Endpoints;
using Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer()
                .AddSwaggerGen()
                .AdicionarDependencias(builder.Configuration)
                .InjecaoDependencias();

builder.Services.AddSingleton<IDadoService, DadoService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.MapearDadoEndpoints();

app.Run();  
