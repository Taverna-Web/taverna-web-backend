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
- **API Gateway:** YARP (Yet Another Reverse Proxy)
- **ORM:** Entity Framework Core
- **Comunicação em Tempo Real:** SignalR / WebSockets (para rolagem de dados simultânea e movimentação no mapa)
- **Arquitetura:** Monorepo de Microsserviços com Clean Architecture e Domain-Driven Design (DDD)
- **Testes:** xUnit

---

## 🏗️ Arquitetura e Design

O projeto adota uma arquitetura de **Monorepo de Microsserviços** combinada com **Clean Architecture** em cada serviço e táticas de **Domain-Driven Design (DDD)**.

```text
taverna-web-backend/
├── Gateway/                     <-- Ponto de entrada do tráfego (YARP)
│   ├── Gateway.Api/
│   └── Gateway.sln
│
├── Shared/                      <-- Caixa de ferramentas compartilhada neutra
│   ├── Shared.Common/
│   └── Shared.sln
│
├── Ficha/                       <-- Microsserviço de Fichas e Inventários
│   ├── Ficha.Api/ ... Ficha.Persistence/
│   └── Ficha.sln
│
├── Gerenciador/                 <-- Microsserviço do Mestre, Sessões e Rolagens
│   ├── Gerenciador.Api/ ... Gerenciador.Persistence/
│   └── Gerenciador.sln
│
├── Mapa/                        <-- Microsserviço de Grids e Tokens
│   ├── Mapa.Api/ ... Mapa.Persistence/
│   └── Mapa.sln
│
├── Usuario/                     <-- Microsserviço de Autenticação e Perfis
│   ├── Usuario.Api/ ... Usuario.Persistence/
│   └── Usuario.sln
│
├── Test/                        <-- Testes automatizados centralizados (xUnit)
└── TavernaWeb.sln               <-- Solução unificada na raiz
```

### Pilares Arquiteturais:
- 🌐 **Monorepo de Microsserviços:** Cada contexto de negócio (`Usuario`, `Ficha`, `Gerenciador`, `Mapa`) é um microsserviço independente com seu próprio ciclo de vida, agrupados no mesmo repositório para facilidade de versionamento.
- 🚪 **API Gateway Centralizado (YARP):** O serviço `Gateway.Api` é o ponto único de contato para clientes externos. O frontend acessa uma única porta e o Gateway direciona as chamadas para a porta interna de cada serviço.
- 🧰 **Shared Kernel Neutro (`Shared.Common`):** Biblioteca técnica compartilhada contendo contratos de WebSocket/SignalR, DTOs e helpers transversais. Proibido conter regras de negócio para evitar acoplamento.
- 🥞 **Clean Architecture por Serviço:** Cada microsserviço é isolado em 4 camadas:
  - `[Serviço].Api`: Apresentação, endpoints e injeção de dependência.
  - `[Serviço].Application`: Casos de uso, DTOs e handlers.
  - `[Serviço].Domain`: Entidades ricas, value objects e regras de negócio puras (sem dependência de banco).
  - `[Serviço].Persistence`: DbContext, mapeamentos EF Core e repositórios.
- 🎯 **Domain-Driven Design (DDD):** Regras de jogo e a linguagem ubíqua (Mestre, Ficha, Iniciativa, Dados) são isoladas no núcleo do domínio.
- ✨ **Clean Code (Código Limpo):** Aplicação estrita dos princípios SOLID e padrões de código limpo.

---

## ⚙️ Escopo dos Microsserviços

- 🔐 **`Usuario` (Autenticação e Perfis):** Gestão de usuários, mestres e jogadores, proteção de rotas com JWT e autorização.
- 📜 **`Ficha` (Personagens e Inventário):** Criação dinâmica, leitura e atualização de atributos, perícias, raças, classes e equipamentos.
- 🧙‍♂️ **`Gerenciador` (Mestre, Sessões e Rolagens):** Gestão de sessões de RPG, NPCs, ordem de iniciativa de combate, motor de rolagem de dados poliédricos e integração com áudio ambiente (Spotify).
- 🗺️ **`Mapa` (Grid Tático e Cenário):** Estado dos mapas de batalha, posicionamento e movimentação de tokens/miniaturas em tempo real e fog of war.
- 🚪 **`Gateway` (Roteamento Reverso):** Centralização de CORS e distribuição de tráfego HTTP/WebSocket para os serviços.

---

## 🚀 Como começar (Para Desenvolvedores)

Siga os passos abaixo para configurar o ambiente de desenvolvimento backend localmente.

### Pré-requisitos
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) instalado.
- Banco de dados (SQL Server).
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

3. **Restaure as dependências e compile a solução completa:**
   ```bash
   dotnet restore
   dotnet build
   ```

4. **Execute os testes automatizados:**
   ```bash
   dotnet test
   ```

5. **Executando os Serviços:**
   - Para rodar um serviço individual (ex: Gateway):
     ```bash
     dotnet run --project Gateway/Gateway.Api/Gateway.Api.csproj
     ```
   - Ou abra a solução específica do serviço que você deseja desenvolver (ex: `Usuario/Usuario.sln` ou `Ficha/Ficha.sln`).

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
