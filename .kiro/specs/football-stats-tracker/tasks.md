# Implementation Plan: Football Stats Tracker

## Overview

Implementação de uma aplicação full-stack para rastrear estatísticas de futebol de domingo. O back-end será construído com ASP.NET Core Minimal API (.NET 8), Entity Framework Core e PostgreSQL. O front-end será uma SPA em Vue.js 3 com Composition API, Vite e TailwindCSS. O plano segue uma abordagem incremental: infraestrutura e modelos → lógica de negócio → endpoints → front-end → integração.

## Tasks

- [x] 1. Set up back-end project structure and core infrastructure
  - [x] 1.1 Create ASP.NET Core Minimal API project with .NET 8 and configure dependencies
    - Create solution and project structure (`FootballStats.Api`)
    - Add NuGet packages: Entity Framework Core, Npgsql EF Core Provider, FsCheck, xUnit
    - Configure `Program.cs` with service registration, CORS, and global exception handler middleware
    - Configure `appsettings.json` with PostgreSQL connection string
    - _Requirements: 9.1, 9.5, 9.6_

  - [x] 1.2 Create Entity classes, DTOs, and EF Core DbContext
    - Implement `Jogador` and `RegistroPartida` entity classes
    - Implement `AppDbContext` with `OnModelCreating` configuration (PK, FK cascade delete, constraints)
    - Implement request DTOs (`CriarJogadorRequest`, `CriarRegistroRequest`, `AtualizarRegistroRequest`)
    - Implement response DTOs (`EstatisticaJogador`, `ErroResponse`)
    - _Requirements: 7.1, 7.2, 7.3, 7.4, 9.2, 9.4_

  - [x] 1.3 Create EF Core migration for initial database schema
    - Generate initial migration with `Jogadores` and `RegistrosPartida` tables
    - Ensure UUID primary keys, varchar(100) for Nome, FK with cascade delete, CHECK constraints for Gols/Assistencias [0-99]
    - _Requirements: 7.1, 7.2, 7.3, 7.4_

  - [x] 1.4 Create service layer interfaces
    - Define `IJogadorService`, `IRegistroPartidaService`, `IEstatisticaService` interfaces
    - Register interfaces in DI container in `Program.cs`
    - _Requirements: 9.3, 9.6_

- [x] 2. Implement Jogador (Player) service and endpoints
  - [x] 2.1 Implement `JogadorService` with creation, listing, and deletion logic
    - Implement `CriarAsync`: validate name not empty/whitespace, max 100 chars, trim whitespace, generate Guid, persist
    - Implement `ListarTodosAsync`: return all players ordered by name
    - Implement `ExcluirAsync`: validate Guid format, check existence, delete (cascade handles records)
    - _Requirements: 1.1, 1.2, 1.3, 1.4, 1.5, 2.2, 2.3, 3.1, 3.2, 3.3_

  - [ ]* 2.2 Write property test for Player Creation Round-Trip
    - **Property 1: Player Creation Round-Trip**
    - **Validates: Requirements 1.1, 1.2, 1.5**

  - [ ]* 2.3 Write property test for Whitespace-Only Names Rejection
    - **Property 2: Whitespace-Only Names Are Rejected**
    - **Validates: Requirements 1.3**

  - [ ]* 2.4 Write property test for Listing Returns All Created Players
    - **Property 3: Listing Returns All Created Players**
    - **Validates: Requirements 2.2**

  - [x] 2.5 Implement Jogadores API endpoints
    - Map `POST /api/jogadores` → `IJogadorService.CriarAsync`, return 201 with created resource
    - Map `GET /api/jogadores` → `IJogadorService.ListarTodosAsync`, return 200 with array
    - Map `DELETE /api/jogadores/{id}` → `IJogadorService.ExcluirAsync`, return 204
    - Handle validation errors (400), not found (404), invalid ID format (400)
    - _Requirements: 1.2, 1.3, 1.4, 2.2, 2.3, 3.1, 3.3, 3.4, 9.3, 9.4_

  - [ ]* 2.6 Write unit tests for Jogadores endpoints
    - Test 201 on successful creation
    - Test 400 on empty/whitespace name and name exceeding 100 chars
    - Test 200 with player array and empty array scenarios
    - Test 204 on successful deletion, 404 on missing player, 400 on invalid ID
    - _Requirements: 1.2, 1.3, 1.4, 2.2, 2.3, 3.1, 3.3, 3.4_

- [x] 3. Implement RegistroPartida (Match Record) service and endpoints
  - [x] 3.1 Implement `RegistroPartidaService` with creation, update, and deletion logic
    - Implement `CriarAsync`: validate player exists, validate date present, validate gols/assists [0-99], generate Guid, persist
    - Implement `AtualizarAsync`: validate record exists, validate gols/assists [0-99], update and persist
    - Implement `ExcluirAsync`: validate record exists, delete
    - _Requirements: 4.1, 4.2, 4.3, 4.4, 4.5, 4.6, 4.7, 4.8, 5.1, 5.2, 5.3, 5.4, 5.5, 5.6, 5.7_

  - [ ]* 3.2 Write property test for Match Record Creation Round-Trip
    - **Property 5: Match Record Creation Round-Trip**
    - **Validates: Requirements 4.1, 4.2, 4.4, 4.8, 7.3**

  - [ ]* 3.3 Write property test for Invalid Goals or Assists Range Rejection
    - **Property 6: Invalid Goals or Assists Range Is Rejected**
    - **Validates: Requirements 4.5, 5.6**

  - [ ]* 3.4 Write property test for Match Record Update Round-Trip
    - **Property 7: Match Record Update Round-Trip**
    - **Validates: Requirements 5.1, 5.2**

  - [x] 3.5 Implement Registros API endpoints
    - Map `POST /api/registros` → `IRegistroPartidaService.CriarAsync`, return 201
    - Map `PUT /api/registros/{id}` → `IRegistroPartidaService.AtualizarAsync`, return 200
    - Map `DELETE /api/registros/{id}` → `IRegistroPartidaService.ExcluirAsync`, return 204
    - Handle validation errors (400), not found (404)
    - _Requirements: 4.2, 4.5, 4.6, 4.7, 5.2, 5.3, 5.4, 5.5, 5.6, 5.7, 9.3, 9.4_

  - [ ]* 3.6 Write unit tests for Registros endpoints
    - Test 201 on successful creation with valid data
    - Test 400 on negative/over-99 gols/assists, missing fields
    - Test 404 on non-existent player reference
    - Test 200 on successful update, 204 on delete, 404/400 on invalid records
    - _Requirements: 4.2, 4.5, 4.6, 4.7, 5.2, 5.5, 5.6, 5.7_

- [x] 4. Implement Estatisticas (Leaderboard) service and endpoint
  - [x] 4.1 Implement `EstatisticaService` with leaderboard aggregation logic
    - Query all players with LEFT JOIN on registros to include zero-stat players
    - Aggregate SUM of gols and SUM of assistencias per player
    - Order by total gols descending, then total assistencias descending as tiebreaker
    - Return `EstatisticaJogador` list
    - _Requirements: 6.2, 6.3, 6.4_

  - [ ]* 4.2 Write property test for Leaderboard Aggregation and Ordering
    - **Property 8: Leaderboard Aggregation and Ordering**
    - **Validates: Requirements 6.2, 6.3, 6.4**

  - [x] 4.3 Implement Estatisticas API endpoint
    - Map `GET /api/estatisticas` → `IEstatisticaService.ObterPlacarGeralAsync`, return 200
    - _Requirements: 6.2, 6.3, 6.4, 9.4_

  - [ ]* 4.4 Write unit tests for Estatisticas endpoint
    - Test correct aggregation with multiple players and records
    - Test ordering by gols desc, then assists desc
    - Test inclusion of players with zero records
    - Test empty response when no players exist
    - _Requirements: 6.2, 6.3, 6.4_

- [x] 5. Checkpoint - Back-end complete
  - Ensure all tests pass, ask the user if questions arise.

- [x] 6. Set up front-end project structure
  - [x] 6.1 Create Vue.js 3 project with Vite and configure dependencies
    - Initialize Vue 3 project with Vite and TypeScript
    - Install and configure TailwindCSS
    - Install Vue Router and Axios
    - Configure API base URL via environment variable (`VITE_API_URL`)
    - _Requirements: 8.1, 9.1_

  - [x] 6.2 Create TypeScript interfaces and API client service
    - Define `Jogador`, `RegistroPartida`, `EstatisticaJogador`, `CriarRegistroDto`, `AtualizarRegistroDto` interfaces
    - Implement `api.ts` with Axios instance and endpoint functions (`jogadoresApi`, `registrosApi`, `estatisticasApi`)
    - Add Axios error interceptor for network/500 errors
    - _Requirements: 8.3, 9.4_

  - [x] 6.3 Create navigation layout and routing
    - Implement `App.vue` with responsive navigation (Placar Geral, Jogadores, Registros)
    - Configure Vue Router with routes: `/` (PlacarGeral), `/jogadores` (GerenciarJogadores), `/registros` (RegistroPartida)
    - Ensure navigation is fully operable from 320px to 1920px viewport
    - _Requirements: 8.2, 8.4_

- [x] 7. Implement front-end pages and composables
  - [x] 7.1 Implement `useJogadores` composable and `GerenciarJogadores.vue` page
    - Create composable with reactive state, `carregar()`, `criar(nome)`, `excluir(id)` functions
    - Build page with form to add player (name input + submit button) and list of players with delete button
    - Display validation errors adjacent to form fields, preserve input on validation failure
    - _Requirements: 1.1, 2.1, 3.1, 8.3, 8.5_

  - [x] 7.2 Implement `useRegistros` composable and `RegistroPartida.vue` page
    - Create composable with `criar(dto)`, `atualizar(id, dto)`, `excluir(id)` functions
    - Build page with form: player dropdown (from `useJogadores`), date picker (DD/MM/AAAA format), gols input, assists input
    - Implement date field with calendar picker and manual input support
    - Display validation errors adjacent to fields, preserve all input values on error
    - _Requirements: 4.1, 2.1, 8.3, 8.5, 8.6_

  - [x] 7.3 Implement `useEstatisticas` composable and `PlacarGeral.vue` page
    - Create composable with reactive state, `carregar()` function
    - Build leaderboard table showing rank, player name, total gols, total assists
    - Set as default route `/` (main view on app open)
    - Style with TailwindCSS for responsive display
    - _Requirements: 6.1, 6.2, 8.1, 8.2, 8.4_

  - [x] 7.4 Implement UI auto-refresh after record changes
    - After creating/updating/deleting a RegistroPartida, automatically refresh the leaderboard data
    - Ensure no full page reload is needed
    - _Requirements: 6.5, 6.6_

  - [ ]* 7.5 Write unit tests for front-end composables
    - Test `useJogadores`: loading, creating, deleting, error handling
    - Test `useRegistros`: creating, updating, deleting, validation
    - Test `useEstatisticas`: loading, data formatting
    - _Requirements: 2.1, 2.4, 4.1, 6.1_

- [ ] 8. Implement Cascade Delete property test
  - [ ]* 8.1 Write property test for Cascade Delete
    - **Property 4: Cascade Delete Removes Player and All Associated Records**
    - **Validates: Requirements 3.1, 3.2**

- [x] 9. Integration and wiring
  - [x] 9.1 Configure CORS and connect front-end to back-end
    - Ensure CORS policy allows front-end origin in development
    - Verify all API calls work end-to-end (create player → create record → view leaderboard)
    - _Requirements: 9.1, 6.1_

  - [ ]* 9.2 Write integration tests for API endpoints
    - Use `WebApplicationFactory` to test full HTTP request/response cycle
    - Test complete flows: create player, create record, view stats, update record, delete record, delete player
    - Verify cascade delete behavior at HTTP level
    - _Requirements: 1.2, 3.1, 3.2, 4.2, 5.2, 6.2_

- [x] 10. Final checkpoint - Ensure all tests pass
  - Ensure all tests pass, ask the user if questions arise.

## Notes

- Tasks marked with `*` are optional and can be skipped for faster MVP
- Each task references specific requirements for traceability
- Checkpoints ensure incremental validation
- Property tests validate universal correctness properties from the design document
- Unit tests validate specific examples and edge cases
- Back-end uses C# with ASP.NET Core Minimal API (.NET 8), xUnit, and FsCheck for property tests
- Front-end uses TypeScript with Vue.js 3, Vitest for unit tests
- Database is PostgreSQL accessed via Entity Framework Core

## Task Dependency Graph

```json
{
  "waves": [
    { "id": 0, "tasks": ["1.1"] },
    { "id": 1, "tasks": ["1.2", "1.4"] },
    { "id": 2, "tasks": ["1.3", "6.1"] },
    { "id": 3, "tasks": ["2.1", "6.2"] },
    { "id": 4, "tasks": ["2.2", "2.3", "2.4", "2.5", "6.3"] },
    { "id": 5, "tasks": ["2.6", "3.1"] },
    { "id": 6, "tasks": ["3.2", "3.3", "3.4", "3.5"] },
    { "id": 7, "tasks": ["3.6", "4.1"] },
    { "id": 8, "tasks": ["4.2", "4.3"] },
    { "id": 9, "tasks": ["4.4", "7.1", "7.3"] },
    { "id": 10, "tasks": ["7.2"] },
    { "id": 11, "tasks": ["7.4", "7.5", "8.1"] },
    { "id": 12, "tasks": ["9.1"] },
    { "id": 13, "tasks": ["9.2"] }
  ]
}
```
