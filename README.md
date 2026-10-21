# 🎲 Taverna Web - Backend

![Status](https://img.shields.io/badge/Status-Em%20Desenvolvimento-orange?style=for-the-badge)
![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp)

> **API, persistência e comunicação em tempo real para o ecossistema Taverna Web.**

Este é o repositório **backend** oficial do **Taverna Web**, o mais novo gerenciador de partidas e campanhas de RPG de mesa (Virtual Tabletop - VTT). 

Enquanto o repositório [frontend](https://github.com/Taverna-Web/taverna-web-frontend) cuida da interface visual (editor de fichas dinâmico, grid de batalha interativo), **este repositório** é o motor que faz tudo acontecer: regras de jogo, persistência e sincronização em tempo real para as mesas.

Para uma visão geral do projeto completo e seus módulos, visite nosso [Repositório da Organização (.github)](https://github.com/Taverna-Web/.github).

---

## 🛠️ Tecnologias e Stack

O desenvolvimento do backend é focado em performance, tipagem forte e escalabilidade, utilizando as ferramentas mais recentes do ecossistema Microsoft:

- **Plataforma:** .NET 10 (ASP.NET Core)
- **Linguagem:** C#
- **ORM:** Entity Framework Core (SQL Server)
- **Comunicação em Tempo Real:** SignalR / WebSockets (para rolagem de dados simultânea e movimentação no mapa)
- **Arquitetura:** Monolito em Camadas com Clean Architecture e Domain-Driven Design (DDD)
- **Testes:** xUnit

---

## 🏗️ Arquitetura e Design

O projeto adota uma arquitetura de **Monolito em Camadas** combinada com **Clean Architecture** e blocos táticos de **Domain-Driven Design (DDD)**.

```text
taverna-web-backend/
├── Api/                         <-- Camada de Apresentação, Endpoints, SignalR Hubs e DI
│   ├── Extensions/              <-- Registro de dependências e configurações de ambiente
│   ├── Properties/              <-- Configurações de inicialização (launchSettings.json)
│   ├── Program.cs
│   └── Api.csproj
│
├── Application/                 <-- Camada de Aplicação, Casos de Uso, DTOs e Utilitários
│   ├── Utils/                   <-- Utilitários transversais (Hash PBKDF2/SHA256, Token JWT)
│   └── Application.csproj
│
├── Domain/                      <-- Camada de Domínio (Núcleo Puro, sem dependências externas)
│   ├── Exceptions/              <-- Exceções de domínio
│   │   └── Common/              <-- Exceções HTTP/comuns (BadRequest, Conflict, NotFound, Unauthorized)
│   ├── Interfaces/
│   │   └── Repository/          <-- Contratos de repositórios (ex: IUsuarioRepository)
│   ├── Model/                   <-- Entidades de domínio (ex: Usuario)
│   ├── ValueObject/             <-- Objetos de valor (ex: Email)
│   └── Domain.csproj
│
├── Persistence/                 <-- Camada de Persistência e Acesso a Dados (EF Core)
│   ├── Configuration/           <-- Mapeamentos Fluent API (IEntityTypeConfiguration)
│   ├── Context/                 <-- Contexto do banco de dados (ApplicationDbContext)
│   ├── Migrations/              <-- Histórico de migrações do Entity Framework Core
│   ├── Repository/              <-- Implementação concreta dos repositórios
│   └── Persistence.csproj
│
├── Test/                        <-- Projeto centralizado de testes automatizados (xUnit)
│   ├── Application/             <-- Testes da camada Application (Hash, Token JWT)
│   ├── Domain/                  <-- Testes das regras de negócio e Value Objects do Domain
│   └── Test.csproj
│
├── .editorconfig                <-- Padronização de estilo e formatação de código
├── .env.example                 <-- Modelo de variáveis de ambiente
├── AGENTS.md                    <-- Diretrizes arquiteturais e operacionais para agentes de IA
├── Directory.Build.props        <-- Configurações globais de compilação (.NET 10)
├── Docker-compose.yml           <-- Orquestração do banco de dados SQL Server 2022
└── TavernaWeb.sln               <-- Solução unificada na raiz
```

### Fluxo de Dependências (Clean Architecture):

```text
┌───────────────────┐
│        Api        │ ──► Referencia: Application, Persistence
└─────────┬─────────┘
          │
          ▼
┌───────────────────┐
│    Persistence    │ ──► Referencia: Application, Domain
└─────────┬─────────┘
          │
          ▼
┌───────────────────┐
│    Application    │ ──► Referencia: Domain
└─────────┬─────────┘
          │
          ▼
┌───────────────────┐
│      Domain       │ ──► Núcleo puro (zero dependências externas)
└───────────────────┘
```

### Pilares Arquiteturais:
- 🥞 **Clean Architecture em Camadas:** O monolito é dividido em 4 camadas principais com isolamento estrito de responsabilidades:
  - `Api`: Ponto de entrada HTTP e WebSockets/SignalR, middlewares e composição de injeção de dependência (`Api -> Application, Persistence`).
  - `Application`: Casos de uso, DTOs, handlers, validações e utilitários (`Application -> Domain`).
  - `Domain`: Entidades (`Model`), objetos de valor (`ValueObject`), contratos de repositórios (`Interfaces/Repository`), exceções (`Exceptions`) e regras de negócio puras (zero dependência de banco ou frameworks externos).
  - `Persistence`: Contexto do Entity Framework Core (`Context`), mapeamentos (`Configuration`), migrations e implementação concreta dos repositórios (`Repository`), referenciando `Application` e `Domain`.
- 🎯 **Domain-Driven Design (DDD):** As regras de jogo e a linguagem ubíqua (Usuário, Mestre, Ficha, Iniciativa, Dados, Mapa) são isoladas no núcleo do domínio através de entidades ricas, objetos de valor e contratos bem definidos.
- ✨ **Clean Code (Código Limpo):** Aplicação estrita dos princípios SOLID e padrões de código limpo.

---

## ⚙️ Escopo Funcional do Sistema

- 🔐 **Usuários (Autenticação e Perfis):** Gestão de usuários, mestres e jogadores, proteção de rotas com JWT e autorização.
- 📜 **Fichas (Personagens e Inventário):** Criação dinâmica, leitura e atualização de atributos, perícias, raças, classes e equipamentos.
- 🧙‍♂️ **Gerenciador (Mestre, Sessões e Rolagens):** Gestão de sessões de RPG, NPCs, ordem de iniciativa de combate, motor de rolagem de dados poliédricos e integração com áudio ambiente (Spotify).
- 🗺️ **Mapas (Grid Tático e Cenário):** Estado dos mapas de batalha, posicionamento e movimentação de tokens/miniaturas em tempo real e fog of war.

---

## 🚀 Como começar (Para Desenvolvedores)

Siga os passos abaixo para configurar o ambiente de desenvolvimento backend localmente.

### Pré-requisitos
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) instalado.
- [Docker](https://docs.docker.com/get-docker/) e Docker Compose (ou instância local do SQL Server 2022).
- (Opcional, mas recomendado) IDE de sua preferência: Visual Studio 2022, Rider ou VS Code com extensão C# Dev Kit.

### Passo a Passo

1. **Clone este repositório:**
   ```bash
   git clone https://github.com/Taverna-Web/taverna-web-backend.git
   ```

2. **Acesse a pasta do projeto:**
   ```bash
   cd taverna-web-backend
   ```

3. **Configure as variáveis de ambiente:**
   Copie o arquivo `.env.example` para `.env` na raiz do projeto e preencha sua senha do banco, connection string e chave JWT:
   ```bash
   cp .env.example .env
   ```

4. **Suba o banco de dados SQL Server via Docker:**
   ```bash
   docker compose -f Docker-compose.yml up -d
   ```

5. **Restaure as dependências, compile a solução e aplique as migrations:**
   ```bash
   dotnet restore
   dotnet build
   dotnet ef database update -p Persistence -s Api
   ```

6. **Execute os testes automatizados:**
   ```bash
   dotnet test
   ```

7. **Executando a API:**
   ```bash
   dotnet run --project Api/Api.csproj
   ```

---

## 🤝 Como Contribuir

Seja você um aventureiro iniciante ou um Arquimago do C#, suas contribuições são bem-vindas!
Antes de contribuir, consulte as diretrizes detalhadas em nosso [`AGENTS.md`](AGENTS.md).

1. Faça um **Fork** deste repositório.
2. Crie uma branch focada na sua funcionalidade: `git checkout -b feat/nova-rota-de-magias`
3. Siga o padrão de projetos e conventions de C# / .NET.
4. Envie seus commits no padrão **Full Body**:
   ```text
   feat: adiciona endpoint para listagem de itens

   Implementa o endpoint GET /api/fichas/itens para consulta de catalogo.

   Principais mudancas:
   - Adiciona use case ObterItensQuery no Application
   - Configura controller e rota no Api
   ```
5. Faça um push e abra um **Pull Request**.

> Se você deseja contribuir com a interface visual, dirija-se ao [Repositório Frontend](https://github.com/Taverna-Web/taverna-web-frontend).

---

## 📄 Licença

Distribuído sob a licença [MIT](LICENSE).

*Que seus testes passem de primeira! 🎲💻*
