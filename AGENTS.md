# Diretrizes para Agentes de IA - Taverna Web Backend

Este documento define regras operacionais, contexto de domínio e convenções arquiteturais obrigatórias para agentes de IA neste repositório.

---

## 1. Contexto do Produto

O Taverna Web é um ecossistema backend para RPG de Mesa Virtual (Virtual Tabletop - VTT).
Responsável por orquestrar regras de jogo, persistência e comunicação em tempo real (SignalR/WebSockets) consumidas pelo frontend.

### Escopo Funcional:
- `Usuarios`: Autenticação, perfis de usuários (mestres e jogadores), segurança (JWT) e autorização.
- `Fichas`: Fichas de personagens, atributos, raças, classes, inventários dinâmicos e equipamentos.
- `Gerenciador`: Ferramentas do mestre, NPCs, ordem de iniciativa de combate, rolagens de dados poliédricos e integração com música ambiente (Spotify).
- `Mapas`: Estado dos mapas de batalha, grids táticos, posicionamento de tokens e visibilidade (fog of war).

---

## 2. Checklist Obrigatório de Execução

1. Leitura prévia: Antes de propor ou realizar alterações, consulte a documentação aplicável (`README.md`, `AGENTS.md`).
2. Compilação estrita: O código DEVE compilar com 0 erros e 0 warnings.
3. Testes obrigatórios: Sempre execute os testes antes de concluir qualquer tarefa (`dotnet test`). 100% de sucesso exigido.

---

## 3. Padrão Arquitetural e Stack

- Plataforma: .NET 10 (C#)
- Arquitetura: Monolito em Camadas com Domain-Driven Design (DDD) e Clean Architecture
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

1. Nomenclatura: Pastas e projetos sempre em PascalCase (`Api`, `Application`, `Domain`, `Persistence`, `Test`).
2. Camadas do Monolito (Clean Architecture):
   - `Api`: Apresentação, endpoints, middlewares, SignalR hubs e injeção de dependência (`Extensions/`). Referencia `Application` e `Persistence`.
   - `Application`: Casos de uso, DTOs, handlers, validações e utilitários (`Utils/`). Referencia `Domain`.
   - `Domain`: Entidades (`Model/`), value objects (`ValueObject/`), interfaces de repositórios (`Interfaces/Repository/`), exceções (`Exceptions/` e `Exceptions/Common/`) e regras de negócio puras (sem dependência externa ou de banco).
   - `Persistence`: DbContext (`Context/`), mapeamentos EF Core (`Configuration/`), migrations e implementação de repositórios (`Repository/`). Referencia `Application` e `Domain`.
   - `Test`: Projeto centralizado de testes automatizados (xUnit) organizado por camada (`Test/Application/`, `Test/Domain/`).
3. Arquivo de Solução:
   - A raiz mantém `TavernaWeb.sln` unificando todos os projetos do monolito.

---

## 6. Restrições e Proibições

- Proibido violar princípios SOLID, isolamento de camadas da Clean Architecture ou padrões de código limpo.
- Proibido adicionar pacotes NuGet ou dependências externas sem aprovação prévia e explícita do usuário.
- Proibido expor credenciais, senhas, connection strings, chaves de API ou tokens.
- Proibido alterar migrations pré-existentes; crie novas migrations para evolução do banco.
