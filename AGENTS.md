# Diretrizes para Agentes de IA - Taverna Web Backend

Este documento define regras operacionais, contexto de domínio e convenções arquiteturais obrigatórias para agentes de IA neste repositório.

---

## 1. Contexto do Produto

O Taverna Web é um ecossistema backend para RPG de Mesa Virtual (Virtual Tabletop - VTT).
Responsável por orquestrar regras de jogo, persistência e comunicação em tempo real (SignalR/WebSockets) consumidas pelo frontend.

### Escopo por Serviço:
- `Gateway/Gateway.Api`: API Gateway reverso (YARP). Ponto único de entrada para clientes externos.
- `Usuario/`: Autenticação, perfis de usuários (mestres e jogadores), segurança (JWT) e autorização.
- `Ficha/`: Fichas de personagens, atributos, raças, classes, inventários dinâmicos e equipamentos.
- `Gerenciador/`: Ferramentas do mestre, NPCs, ordem de iniciativa de combate, rolagens de dados poliédricos e integração com música ambiente (Spotify).
- `Mapa/`: Estado dos mapas de batalha, grids táticos, posicionamento de tokens e visibilidade (fog of war).
- `Shared/Shared.Common`: Caixa de ferramentas transversal genérica (helpers técnicos, DTOs padrão de resposta, contratos de WebSocket/SignalR). Proibido conter regras de negócio.
- `Test/`: Projeto centralizado de testes automatizados (xUnit).

---

## 2. Checklist Obrigatório de Execução

1. Leitura prévia: Antes de propor ou realizar alterações, consulte a documentação aplicável (`README.md`, `AGENTS.md`).
2. Compilação estrita: O código DEVE compilar com 0 erros e 0 warnings.
3. Testes obrigatórios: Sempre execute os testes antes de concluir qualquer tarefa (`dotnet test`). 100% de sucesso exigido.

---

## 3. Padrão Arquitetural e Stack

- Plataforma: .NET 10 (C#)
- Arquitetura: Monorepo de Microsserviços com Domain-Driven Design (DDD) e Clean Architecture
- Persistência: Entity Framework Core
- Comunicação: REST APIs + SignalR

---

## 4. Git e Convenção de Commits

### Gitflow:
- Padrão geral: Branches dedicadas (`feat/nome-da-feat`, `fix/nome-do-fix`), sem alteração direta em `main`/`develop`, solicitando abertura de Pull Request ao usuário.
- Exceção para fase atual: Durante a etapa de setup inicial e estruturação de arquitetura, commits podem ser feitos diretamente na branch `develop`.

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

## 5. Convenções Técnicas

1. Nomenclatura: Pastas e projetos sempre em PascalCase (ex: `Ficha.Api`, `Usuario.Domain`, `Shared.Common`).
2. Camadas por microsserviço:
   - `[Serviço].Api`: Apresentação, endpoints, middlewares, injeção de dependência.
   - `[Serviço].Application`: Casos de uso, DTOs, handlers, validações.
   - `[Serviço].Domain`: Entidades ricas, value objects, regras de negócio puras (sem dependência de banco).
   - `[Serviço].Persistence`: DbContext, mapeamentos EF Core, migrations, repositórios.
3. Isolamento entre serviços:
   - Proibido criar dependência direta de código entre serviços (ex: `Ficha` nunca referencia `Usuario`).
   - A pasta `Shared` é estritamente neutra. Nunca adicione modelos ou regras de negócio em `Shared`.
4. Arquivos de Solução:
   - Cada serviço mantém sua própria solução (`Usuario/Usuario.sln`, `Ficha/Ficha.sln`, etc.).
   - A raiz mantém `TavernaWeb.sln` unificando todos os projetos para compilação global.

---

## 6. Restrições e Proibições

- Proibido violar princípios SOLID ou padrões de código limpo.
- Proibido adicionar pacotes NuGet ou dependências externas sem aprovação prévia e explícita do usuário.
- Proibido expor credenciais, senhas, connection strings, chaves de API ou tokens.
- Proibido alterar migrations pré-existentes; crie novas migrations para evolução do banco.
