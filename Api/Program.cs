using Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AdicionarDependenciasUsuario(builder.Configuration).AddEndpointsApiExplorer().AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.Run();
