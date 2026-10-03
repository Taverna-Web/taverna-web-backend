# 🎲 Taverna Web - Backend

![Status](https://img.shields.io/badge/Status-Em%20Desenvolvimento-orange?style=for-the-badge)
![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp)

> **API, persistência e comunicação em tempo real para o ecossistema Taverna Web.**

Este é o repositório **backend** oficial do **Taverna Web**, o mais novo gerenciador de partidas e campanhas de RPG de mesa. 

Enquanto o repositório [frontend](https://github.com/Taverna-Web/taverna-web-frontend) cuida da interface visual (editor de fichas, grid de batalha interativo), **este repositório** é o motor que faz tudo acontecer: regras de negócio, banco de dados, e sincronização em tempo real para as mesas.

Para uma visão geral do projeto completo e seus módulos, visite nosso [Repositório da Organização (.github)](https://github.com/Taverna-Web/.github).

---

## 🛠️ Tecnologias e Stack

O desenvolvimento do backend é focado em performance, tipagem forte e escalabilidade, utilizando as ferramentas mais recentes do ecossistema Microsoft:

- **Framework:** .NET 10 (ASP.NET Core)
- **Linguagem:** C#
- **ORM:** Entity Framework Core
- **Comunicação em Tempo Real:** SignalR / WebSockets (para rolagem de dados simultânea e movimentação no mapa)
- **Arquitetura API:** RESTful

---

## 🏗️ Arquitetura e Design

Para garantir um sistema robusto, escalável e de fácil manutenção, o projeto adota uma abordagem de **Monolito Modular**, aliada à **Arquitetura de Camadas** e princípios de **Domain-Driven Design (DDD)**. O foco central do sistema é o domínio rico das mecânicas de RPG de mesa.

- 🧩 **Monolito Modular:** A aplicação é organizada em módulos coesos e independentes por contexto de negócio (ex: Usuários, Fichas, Mesas, Rolagens). Isso nos dá a excelente separação de responsabilidades dos microsserviços, mas com a simplicidade de deploy e infraestrutura de um monolito.
- 🥞 **Arquitetura de Camadas:** Estabelece uma separação clara de responsabilidades entre Apresentação (API / SignalR), Aplicação (Casos de Uso), Domínio (Regras de Negócio e Entidades) e Infraestrutura (Banco de Dados, Integrações Externas).
- 🎯 **Domain-Driven Design (DDD):** Utilizamos as táticas de DDD para modelar o coração da aplicação. As regras de jogo e a "linguagem ubíqua" (ex: Mestre, Ficha, Teste de Habilidade, Iniciativa) são puras e não dependem de detalhes técnicos.
- ✨ **Clean Code (Código Limpo):** Aplicamos constantemente os princípios SOLID e boas práticas de código para garantir que o projeto se mantenha limpo, legível, facilmente testável e agradável para que qualquer desenvolvedor possa contribuir.

---

## ⚙️ Funcionalidades do Backend

O escopo deste repositório inclui (mas não se limita a):

- 🔐 **Autenticação e Autorização:** Gestão de usuários, mestres e jogadores, protegendo rotas e campanhas.
- 📜 **API de Fichas de Personagem:** Endpoints para criar, atualizar e ler informações de fichas, inventários e atributos de forma dinâmica.
- 🧙‍♂️ **Ferramentas do Mestre:** Rotas para gerenciamento de iniciativa, controle de NPCs e gerenciamento de sessões.
- 🎲 **Motor de Rolagem e Sincronização:** Cálculos seguros de fórmulas matemáticas de dados poliédricos transmitidos via SignalR para todos os clientes conectados na mesa.
- 🗺️ **Estado dos Mapas:** Gerenciamento do estado atual dos grids táticos, posições de tokens e persistência do cenário.
- 🎵 **Integrações Externas:** Intermediação e gestão de tokens (OAuth) para uso seguro da API do Spotify pelos jogadores.

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

3. **Restaure as dependências do .NET:**
   ```bash
   dotnet restore
   ```

4. **Configuração do Banco de Dados:**
   - Certifique-se de configurar a sua string de conexão correta no arquivo `appsettings.Development.json`.
   - Aplique as migrações (se aplicável):
     ```bash
     dotnet ef database update
     ```

5. **Execute a aplicação:**
   ```bash
   dotnet run
   ```

A API estará disponível (por padrão) em `https://localhost:5001` (ou a porta configurada no seu `launchSettings.json`). Você pode acessar o **Swagger** para testar os endpoints!

---

## 🤝 Como Contribuir

Seja você um aventureiro iniciante ou um Arquimago do C#, suas contribuições são bem-vindas!
Nosso projeto está ativamente em desenvolvimento e precisamos de ajuda para construir as rotas, estruturar os modelos de dados e otimizar as conexões WebSocket.

1. Faça um **Fork** deste repositório.
2. Crie uma branch focada na sua funcionalidade: `git checkout -b feat/nova-rota-de-magias`
3. Siga o padrão de projetos e conventions de C# / .NET.
4. Envie seus commits: `git commit -m 'feat: adiciona endpoint para listagem de itens'`
5. Faça um push e abra um **Pull Request**.

> Se você deseja contribuir com a interface visual, dirija-se ao [Repositório Frontend](https://github.com/Taverna-Web/taverna-web-frontend).

---

## 📄 Licença

Distribuído sob a licença [MIT](LICENSE).

*Que seus testes passem de primeira! 🎲💻*
