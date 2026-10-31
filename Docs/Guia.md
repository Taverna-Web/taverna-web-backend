# 🚀 Guia de Desenvolvimento - Taverna Web Backend

Este guia orienta os desenvolvedores no passo a passo prático para implementar novas funcionalidades, executar migrations, rodar testes e manter o padrão de qualidade do projeto.

---

## 1. Passo a Passo para Criar uma Nova Feature

Sempre siga a ordem de dependências das camadas: de dentro para fora (`Domain` ➔ `Persistence` ➔ `Application` ➔ `Api` ➔ `Test`).

---

### Passo 1: Camada `Domain` (Regras e Contratos)
1. **Model ou Value Object:**
   * Crie ou atualize as entidades em `Domain/Model/` (ex: `Ficha.cs`, `Usuario.cs`).
   * Se houver tipos especiais com validação própria (como e-mail, pontos de vida, dados), crie em `Domain/ValueObject/`.
2. **Contrato do Repositório:**
   * Declare os métodos necessários na interface em `Domain/Interfaces/Repository/I{Entidade}Repository.cs`.
   * **Atenção à convenção:** Todo método assíncrono de repositório deve terminar com o sufixo **`RepositoryAsync`**.
   ```csharp
   Task<int> ObterIdPorEmailRepositoryAsync(Email email);
   Task AdicionarUsuarioRepositoryAsync(string nome, string senhaHash, Email email);
   ```

---

### Passo 2: Camada `Persistence` (Banco de Dados e Consultas)
1. **Mapeamento Fluent API:**
   * Crie ou edite a configuração em `Persistence/Configuration/{Entidade}Configuration.cs` implementando `IEntityTypeConfiguration<{Entidade}>`.
2. **DbContext:**
   * Adicione o `DbSet<{Entidade}>` no arquivo `Persistence/Context/ApplicationDbContext.cs`.
3. **Criar e Aplicar Migration:**
   * Crie uma migration incremental descrevendo a alteração:
     ```bash
     dotnet ef migrations add NomeDaAlteracao -p Persistence -s Api
     ```
   * Atualize o banco local (se necessário):
     ```bash
     dotnet ef database update -p Persistence -s Api
     ```
4. **Implementar o Repositório:**
   * No arquivo `Persistence/Repository/{Entidade}Repository.cs`, implemente os métodos declarados na interface usando o `ApplicationDbContext`.

---

### Passo 3: Camada `Application` (Casos de Uso e DTOs)
1. **Criar os DTOs em `Application/Dtos/{Funcionalidade}/`:**
   * `Request/{Acao}RequestDto.cs` (dados que chegam do cliente).
   * `Response/{Acao}ResponseDto.cs` (dados que voltam para o cliente).
   * `Command/{Acao}CommandDto.cs` (objeto interno validado).
2. **Criar o Mapper:**
   * Em `Application/Mapper/{Funcionalidade}/{Funcionalidade}Mapper.cs`, crie os métodos para converter `Request` em `Command` e `Entidade` em `Response`.
3. **Interface e Classe de Serviço:**
   * Defina o contrato em `Application/Interfaces/I{Funcionalidade}Service.cs`.
   * Implemente a lógica em `Application/Service/{Funcionalidade}Service.cs`.
   * Se houver violação de regra de negócio, lance exceções de domínio apropriadas (`ConflictException`, `NotFoundException`, `UnauthorizedException`).

---

### Passo 4: Camada `Api` (Endpoints e Injeção de Dependência)
1. **Criar o Endpoint:**
   * Crie o arquivo `Api/Endpoints/{Funcionalidade}/{Funcionalidade}Endpoint.cs`.
   * Use o método de extensão com `IEndpointRouteBuilder`:
   ```csharp
   public static class ExemploEndpoint
   {
       public static void MapExemploEndpoint(this IEndpointRouteBuilder app)
       {
           var group = app.MapGroup("/exemplo")
               .WithTags("Exemplo");

           group.MapPost("", async (ExemploRequestDto request, IExemploService service) =>
           {
               var response = await service.ExecutarAsync(request);
               return Results.Ok(response);
           })
           .WithName("ExecutarExemplo")
           .WithSummary("Descrição clara do que o endpoint faz")
           .Produces<ExemploResponseDto>(200)
           .ProducesProblem(400)
           .ProducesProblem(500);
       }
   }
   ```
2. **Registrar a Rota no `Program.cs`:**
   * Mapeie dentro do grupo `/api`:
     ```csharp
     var api = app.MapGroup("/api");
     api.MapExemploEndpoint();
     ```
3. **Registrar Dependências:**
   * Em `Api/Extensions/InjecaoDependencias.cs`, registre o novo repositório e serviço no container de DI:
     ```csharp
     services.AddScoped<IExemploRepository, ExemploRepository>();
     services.AddScoped<IExemploService, ExemploService>();
     ```

---

### Passo 5: Camada `Test` (Garantia de Qualidade)
1. Crie os testes unitários espelhando a camada correspondente:
   * Regras de negócio puras: `Test/Domain/`
   * Serviços, Mappers e Utilitários: `Test/Application/`
2. Execute todos os testes e confirme que passaram 100%:
   ```bash
   dotnet test
   ```

---

## 2. Comandos Úteis do Dia a Dia

| Ação | Comando |
| :--- | :--- |
| **Compilar a solução inteira** | `dotnet build` |
| **Rodar todos os testes** | `dotnet test` |
| **Criar uma nova Migration** | `dotnet ef migrations add NomeDaMigration -p Persistence -s Api` |
| **Aplicar Migrations no Banco** | `dotnet ef database update -p Persistence -s Api` |
| **Rodar a API localmente** | `dotnet run --project Api/Api.csproj` |
| **Subir o banco de dados SQL Server** | `docker compose -f Docker-compose.yml up -d` |

---

## 3. Checklist Obrigatório Antes de Abrir PR ou Commitar

- [ ] **Compilação estrita:** `dotnet build` executa com **0 erros** e **0 warnings**.
- [ ] **Testes automatizados:** `dotnet test` com **100% de sucesso**.
- [ ] **Sem alteração em migrations antigas:** Apenas crie novas migrations para evolução de schema.
- [ ] **Sem dados sensíveis:** Nenhuma senha, token ou secret commitado.
- [ ] **Padrão de Commit (Full Body):**
  ```text
  <tipo>: <título objetivo>

  <descrição do propósito da alteração>

  Principais mudanças:
  - <item 1>
  - <item 2>
  ```
