var builder = WebApplication.CreateBuilder(args);

// Configuração do YARP (Reverse Proxy) lendo as rotas e clusters do appsettings.json
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// Configuração de CORS permissivo para desenvolvimento frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("AllowFrontend");

// Mapeia o proxy reverso para processar e redirecionar as requisições
app.MapReverseProxy();

app.Run();
