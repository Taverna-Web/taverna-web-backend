# Diretrizes para Agentes de IA - Taverna Web Backend

Este documento define regras operacionais, contexto de domínio, convenções arquiteturais e diretrizes de comportamento obrigatórias para agentes de IA neste repositório.

---

## 1. Contexto do Produto e Arquitetura

O Taverna Web é um ecossistema backend para RPG de Mesa Virtual (Virtual Tabletop - VTT).
Responsável por orquestrar regras de jogo, persistência e comunicação em tempo real (SignalR/WebSockets) consumidas pelo frontend.

### Modelo Arquitetural: Monólito Modular em Camadas
> ⚠️ **Atenção:** Este projeto **NÃO é uma arquitetura de microsserviços**. Trata-se de um **Monólito Modular** unificado na solução `TavernaWeb.sln`. Todos os módulos convivem dentro das mesmas 4 camadas do monolito, comunicando-se via código C# e injeção de dependências direta.

### Módulos / Domínios do Sistema:
- `Usuarios`: Autenticação, perfis de usuários (mestres e jogadores), segurança (JWT com claims `sub`, `email`, `iss`, `aud`), hash de senhas (PBKDF2) e autorização.
- `Fichas`: Fichas de personagens dinâmicas, atributos, perícias, raças, classes, inventários e equipamentos.
- `Gerenciador`: Ferramentas do mestre, NPCs, gerenciamento de sessões, ordem de iniciativa de combate, rolagens de dados poliédricos e integração com música ambiente.
- `Mapas`: Estado dos mapas de batalha, grids táticos, posicionamento/movimentação de tokens em tempo real e visibilidade (fog of war).

---

## 2. Postura de Alinhamento e Perguntas Ativas (Regra da "Ponte")

O usuário frequentemente envia comandos objetivos, curtos ou de alto nível.
**O agente NUNCA deve chutar ou presumir regras de negócio, campos de entidades ou decisões arquiteturais importantes.**

> 🌉 **Analogia da Ponte:** Se o usuário disser *"construa uma ponte"*, não saia construindo qualquer ponte. Pergunte primeiro:  
> *Uma ponte até onde? Com qual material? Precisa de pilares para suportar peso pesado?*

### Regras para o Agente:
1. **Pergunte antes de chutar:** Se a instrução do usuário for ampla (ex: *"faça a rota de trocar senha"*, *"crie a entidade de ficha"*), identifique as lacunas e faça perguntas objetivas de esclarecimento (ex: quais campos são necessários, regras de validação, se a rota deve ser autenticada, quais status de retorno espera).
2. **Proponha opções:** Quando fizer perguntas, sugira alternativas com recomendações claras para facilitar a decisão rápida do usuário.
3. **Não trave para o trivial:** Para detalhes puramente técnicos e já padronizados (como convenção de nomes de DTOs ou adicionar sufixo `RepositoryAsync`), siga os padrões existentes sem incomodar o usuário desnecessariamente.

---

## 3. Checklist Obrigatório de Execução

1. **Leitura prévia obrigatória:** Antes de propor ou realizar alterações, consulte sempre a documentação do projeto:
   * `README.md`
   * `AGENTS.md`
   * Toda a pasta `Docs/` e seus documentos vigentes.
2. **Sincronização contínua de documentação (Documentação Viva):** Sempre que introduzir uma nova pasta estrutural (ex: `Middlewares/`, `Hubs/`), um novo domínio, novo endpoint ou alterar convenções existentes, o agente **DEVE atualizar a documentação correspondente em `Docs/`** (ou criar o documento pertinente lá dentro), mantendo a documentação sempre fiel ao código.
3. **Compilação estrita:** O código DEVE compilar com **0 erros** e **0 warnings** (`dotnet build`).
4. **Testes obrigatórios:** Sempre execute os testes antes de concluir qualquer tarefa (`dotnet test`). **100% de sucesso exigido**.

---

## 4. Memória Perene do Projeto (Convenções Técnicas Estritas)

Consulte esta seção para manter a consistência do código sem se perder entre sessões:

### 4.1. Camadas da Clean Architecture:
* `Domain`: Entidades puras (`Model/`), objetos de valor (`ValueObject/`), interfaces de repositórios (`Interfaces/Repository/`), exceções de negócio (`Exceptions/` e `Exceptions/Common/`). **Zero dependência de banco de dados ou frameworks externos**.
* `Application`: Casos de uso (`Service/`), DTOs divididos por intenção (`Dtos/{Func}/Request/`, `Response/`, `Command/`), mappers dedicados (`Mapper/{Func}/`), utilitários puros (`Utils/` com `Hash` e `Token`) e abstrações como `IAspNetUser`. Referencia apenas `Domain`.
* `Persistence`: `ApplicationDbContext` (`Context/`), mapeamentos Fluent API (`Configuration/`), migrations versionadas (`Migrations/`) e implementação concreta de repositórios (`Repository/`). Referencia `Application` e `Domain`.
* `Api`: Minimal APIs (`Endpoints/{Func}/`), extensões modulares (`Extensions/` com Swagger, Banco, Segurança, DI), segurança (`Security/AspNetUser.cs`), middlewares e SignalR hubs. Referencia `Application` e `Persistence`.
* `Test`: Projeto xUnit centralizado espelhando a arquitetura (`Test/Domain/`, `Test/Application/`).

### 4.2. Convenções Obrigatórias de Nomenclatura:
* **Repositórios:** Todos os métodos assíncronos terminam obrigatoriamente com o sufixo **`RepositoryAsync`** (ex: `AdicionarUsuarioRepositoryAsync`, `EmailJaCadastradoRepositoryAsync`).
* **Serviços:** Interfaces `I{Nome}Service`, classes `{Nome}Service`, e métodos com sufixo **`Async`** (ex: `AutenticarAsync`, `CadastrarUsuarioAsync`).
* **Endpoints:** Classes estáticas `{Nome}Endpoint` com método de extensão `Map{Nome}Endpoint(this IEndpointRouteBuilder app)`.
* **Rotas HTTP:** Agrupadas sob o prefixo global **`/api`** com metadados OpenAPI (`WithName`, `WithSummary`, `.Produces<T>`, `.ProducesProblem`).
* **DTOs:** Nunca expor entidades de banco ou receber tipos primitivos soltos. Sempre criar DTOs em `Application/Dtos/{Func}/` com sufixos **`RequestDto`**, **`ResponseDto`** e **`CommandDto`**.
* **Mappers:** Classes estáticas `{Funcionalidade}Mapper.cs`.

### 4.3. Padrões de Segurança e Autenticação:
* **JWT:** Assinado em HS256 contendo `sub`, `email`, `iss` (Issuer) e `aud` (Audience).
* **Usuário Autenticado:** Resgatado na camada `Application` exclusivamente via injeção de **`IAspNetUser`**, sem acoplamento com o `HttpContext`.
* **Senhas:** Criptografadas com PBKDF2 e Salt embutido via [`Hash.cs`](file:///home/brunin/Projetos/Taverna-Web/backend/taverna-web-backend/Application/Utils/Hash.cs).
* **Swagger:** Configurado com suporte a Bearer JWT através de `AddConfiguracaoSwagger()` para teste interativo com cadeado de autorização.

---

## 5. Git e Convenção de Commits

### Gitflow:
* Padrão geral: Branches dedicadas (`feat/nome-da-feat`, `fix/nome-do-fix`, `docs/nome-da-doc`), sem alteração direta em `main`/`develop`, abrindo Pull Request ao final.
* Exceção inicial: Durante a etapa de setup inicial e estruturação de arquitetura, commits podem ser feitos diretamente na branch `develop`.

### Formato Obrigatório de Commit (Full Body):
Todo commit deve conter título objetivo e corpo descritivo estruturado:
```text
<tipo>: <título objetivo>

<descrição do propósito da alteração>

Principais mudanças:
- <item 1>
- <item 2>
```

---

## 6. Restrições e Proibições

- Proibido violar princípios SOLID, isolamento de camadas da Clean Architecture ou padrões de código limpo.
- Proibido adicionar pacotes NuGet ou dependências externas sem aprovação prévia e explícita do usuário.
- Proibido expor credenciais, senhas, connection strings, chaves de API ou tokens.
- Proibido alterar migrations pré-existentes; crie novas migrations para evolução do banco.
- Proibido chutar requisitos de negócio sem consultar o usuário quando a instrução for ambígua.
