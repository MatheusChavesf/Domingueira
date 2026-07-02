# Requirements Document

## Introduction

Football Stats Tracker é uma aplicação web para registrar e exibir estatísticas de desempenho dos jogos de futebol de domingo. O sistema permite gerenciar jogadores, registrar gols e assistências por partida, e visualizar um placar geral (leaderboard) classificando os jogadores pelo desempenho acumulado. O back-end utiliza C# com ASP.NET Core Minimal API e Entity Framework Core (.NET 8), o front-end utiliza Vue.js 3 com Vite, Composition API e TailwindCSS, e o banco de dados é PostgreSQL.

## Glossary

- **Sistema**: A aplicação web Football Stats Tracker como um todo (API back-end + UI front-end)
- **API**: O serviço back-end ASP.NET Core Minimal API
- **UI**: A aplicação front-end single-page em Vue.js 3
- **Jogador**: Uma pessoa cadastrada no sistema que participa dos jogos de futebol de domingo (armazenado na tabela Jogadores)
- **Registro_Partida**: Uma entrada registrando os gols e assistências de um jogador em uma data específica (armazenado na tabela RegistrosPartida)
- **Placar Geral**: Uma visão ranqueada mostrando o total de gols e assistências de cada jogador, ordenada pelo artilheiro
- **Gols**: O número de gols marcados por um jogador em uma partida
- **Assistências**: O número de assistências feitas por um jogador em uma partida

## Requirements

### Requisito 1: Cadastro de Jogadores

**História de Usuário:** Como usuário, quero adicionar novos jogadores ao sistema, para que eu possa acompanhar o desempenho deles nas partidas ao longo do tempo.

#### Critérios de Aceitação

1. QUANDO o usuário submeter um nome de jogador pela UI, A API DEVE criar um novo registro de Jogador no banco de dados e retornar o Jogador criado com seu identificador único e nome no corpo da resposta
2. QUANDO uma requisição POST for enviada para /api/jogadores com um nome de jogador válido, A API DEVE persistir o Jogador na tabela Jogadores e responder com status HTTP 201 e o recurso criado como JSON
3. SE o usuário submeter um nome de jogador vazio ou contendo apenas espaços em branco, ENTÃO A API DEVE rejeitar a requisição e retornar status HTTP 400 com uma mensagem de erro indicando que o nome do jogador é obrigatório
4. SE o usuário submeter um nome de jogador que exceda 100 caracteres, ENTÃO A API DEVE rejeitar a requisição e retornar status HTTP 400 com uma mensagem de erro indicando que o nome excede o comprimento máximo
5. A API DEVE remover espaços em branco iniciais e finais do nome do jogador antes de persistir

### Requisito 2: Listagem de Jogadores

**História de Usuário:** Como usuário, quero visualizar todos os jogadores cadastrados, para que eu possa selecionar um jogador ao registrar o desempenho da partida.

#### Critérios de Aceitação

1. QUANDO o usuário navegar para o formulário de registro de partida, A UI DEVE exibir uma lista dropdown preenchida com todos os jogadores cadastrados, mostrando o nome de cada jogador, permitindo selecionar exatamente um jogador
2. QUANDO uma requisição GET for enviada para /api/jogadores, A API DEVE retornar status HTTP 200 com um array JSON de todos os jogadores, cada entrada contendo o identificador único e o nome do jogador
3. SE nenhum jogador estiver cadastrado no sistema, ENTÃO A API DEVE retornar status HTTP 200 com um array JSON vazio
4. SE a UI falhar ao recuperar a lista de jogadores da API, ENTÃO A UI DEVE exibir uma mensagem de erro indicando que a lista de jogadores não pôde ser carregada

### Requisito 3: Exclusão de Jogadores

**História de Usuário:** Como usuário, quero excluir um jogador do sistema, para que eu possa remover jogadores que não participam mais dos jogos.

#### Critérios de Aceitação

1. QUANDO uma requisição DELETE for enviada para /api/jogadores/{id} com um identificador válido de um Jogador existente, A API DEVE remover o registro do Jogador da tabela Jogadores e responder com status HTTP 204 sem corpo de resposta
2. QUANDO um Jogador for excluído, A API DEVE também excluir todos os registros de Registro_Partida associados a esse Jogador na mesma operação, garantindo que não restem registros órfãos
3. SE o usuário solicitar a exclusão de um jogador com um identificador que não corresponde a nenhum Jogador existente, ENTÃO A API DEVE responder com status HTTP 404 e uma mensagem de erro indicando que o jogador não foi encontrado
4. SE o usuário enviar uma requisição DELETE para /api/jogadores/{id} onde o id não é um formato de identificador válido, ENTÃO A API DEVE responder com status HTTP 400 e uma mensagem de erro indicando que o formato do identificador é inválido

### Requisito 4: Registro de Partida

**História de Usuário:** Como usuário, quero registrar gols e assistências de um jogador em uma data específica, para que o placar geral reflita o desempenho mais recente.

#### Critérios de Aceitação

1. QUANDO o usuário selecionar um Jogador, selecionar uma data através de um campo de data com calendário (date picker) ou digitando manualmente no formato DD/MM/AAAA, inserir uma quantidade de Gols e uma quantidade de Assistências e submeter o formulário, A API DEVE criar um novo Registro_Partida na tabela RegistrosPartida e retornar o registro criado incluindo seu identificador único, identificador do jogador, data, quantidade de gols e quantidade de assistências
2. QUANDO uma requisição POST for enviada para /api/registros com um identificador de jogador válido, data, quantidade de gols e quantidade de assistências, A API DEVE persistir o Registro_Partida e responder com status HTTP 201 e o recurso criado no corpo da resposta
3. A API DEVE aceitar zero como valor válido para quantidade de Gols e quantidade de Assistências
4. A API DEVE aceitar quantidade de Gols e quantidade de Assistências como valores inteiros no intervalo de 0 a 99
5. SE o usuário submeter um Registro_Partida com quantidade de Gols negativa, quantidade de Assistências negativa, ou um valor excedendo 99, ENTÃO A API DEVE rejeitar a requisição e retornar status HTTP 400 com uma mensagem de erro indicando o campo inválido
6. SE o usuário submeter um Registro_Partida referenciando um Jogador que não existe, ENTÃO A API DEVE rejeitar a requisição e retornar status HTTP 404 com uma mensagem de erro indicando que o jogador não foi encontrado
7. SE o usuário submeter um Registro_Partida com identificador de jogador ausente ou vazio, data ausente, quantidade de gols ausente ou quantidade de assistências ausente, ENTÃO A API DEVE rejeitar a requisição e retornar status HTTP 400 com uma mensagem de erro indicando os campos faltantes
8. A API DEVE permitir múltiplas entradas de Registro_Partida para o mesmo Jogador na mesma data

### Requisito 5: Correção de Registro de Partida

**História de Usuário:** Como usuário, quero corrigir ou remover gols e assistências registrados anteriormente, para que erros de digitação possam ser corrigidos.

#### Critérios de Aceitação

1. QUANDO o usuário solicitar uma atualização de um Registro_Partida existente, A API DEVE atualizar a quantidade de Gols e a quantidade de Assistências daquele registro na tabela RegistrosPartida e retornar o Registro_Partida atualizado no corpo da resposta
2. QUANDO uma requisição PUT for enviada para /api/registros/{id} com valores atualizados de gols e assistências, A API DEVE persistir as alterações e responder com status HTTP 200 e o Registro_Partida atualizado contendo seu identificador, referência ao jogador, data, quantidade de Gols e quantidade de Assistências
3. QUANDO o usuário solicitar a exclusão de um Registro_Partida existente, A API DEVE remover o registro da tabela RegistrosPartida e responder com status HTTP 204
4. QUANDO uma requisição DELETE for enviada para /api/registros/{id}, A API DEVE excluir o Registro_Partida identificado pelo id fornecido
5. SE o usuário solicitar atualização ou exclusão de um Registro_Partida que não existe, ENTÃO A API DEVE responder com status HTTP 404 e uma mensagem de erro descritiva
6. SE o usuário submeter uma atualização com quantidade de Gols negativa ou quantidade de Assistências negativa, ENTÃO A API DEVE rejeitar a requisição e retornar status HTTP 400 com uma mensagem de erro descritiva
7. SE o usuário submeter uma requisição de atualização com quantidade de Gols ausente ou não-inteira, ou quantidade de Assistências ausente ou não-inteira, ENTÃO A API DEVE rejeitar a requisição e retornar status HTTP 400 com uma mensagem de erro indicando os campos inválidos

### Requisito 6: Exibição do Placar Geral

**História de Usuário:** Como usuário, quero ver um placar geral mostrando o total de gols e assistências por jogador, para que eu possa comparar o desempenho dos jogadores rapidamente.

#### Critérios de Aceitação

1. QUANDO o usuário abrir a aplicação, A UI DEVE exibir o Placar Geral como a visualização principal
2. QUANDO uma requisição GET for enviada para /api/estatisticas, A API DEVE retornar status HTTP 200 com um array JSON onde cada entrada contém o nome do jogador, soma total de Gols e soma total de Assistências
3. A API DEVE ordenar os resultados do Placar Geral por total de Gols em ordem decrescente; QUANDO dois ou mais jogadores tiverem o mesmo total de Gols, A API DEVE ordená-los por total de Assistências em ordem decrescente como critério de desempate
4. A API DEVE incluir jogadores com zero gols e zero assistências nos resultados do Placar Geral
5. QUANDO um novo Registro_Partida for salvo, A UI DEVE atualizar o Placar Geral para refletir os totais atualizados sem exigir um recarregamento completo da página
6. QUANDO um Registro_Partida for atualizado ou excluído, A UI DEVE atualizar o Placar Geral para refletir os totais corrigidos sem exigir um recarregamento completo da página

### Requisito 7: Integridade de Dados

**História de Usuário:** Como usuário, quero que o sistema mantenha relacionamentos de dados precisos, para que as estatísticas sejam sempre consistentes e confiáveis.

#### Critérios de Aceitação

1. A API DEVE aplicar um relacionamento de chave estrangeira entre Registro_Partida e Jogador, garantindo que todo Registro_Partida referencie um Jogador existente na tabela Jogadores
2. A API DEVE armazenar quantidade de Gols e quantidade de Assistências como valores inteiros não-negativos com valor máximo de 99 por partida
3. A API DEVE armazenar a data da partida como valor somente-data no formato ISO 8601 (YYYY-MM-DD) sem componente de hora
4. A API DEVE gerar identificadores únicos para cada Jogador e cada Registro_Partida na criação
5. A API DEVE permitir múltiplas entradas de Registro_Partida para o mesmo Jogador na mesma data

### Requisito 8: UI Limpa e Responsiva

**História de Usuário:** Como usuário, quero uma interface simples e limpa estilizada com TailwindCSS, para que eu possa usar a aplicação confortavelmente em qualquer dispositivo.

#### Critérios de Aceitação

1. A UI DEVE usar classes utilitárias do TailwindCSS para toda estilização, sem estilos inline ou frameworks CSS externos
2. A UI DEVE fornecer uma estrutura de navegação que permita ao usuário acessar o Placar Geral, a seção de gerenciamento de Jogadores e o formulário de Registro de Partida, e que permaneça totalmente operável em larguras de viewport de 320px a 1920px
3. QUANDO uma submissão de formulário falhar na validação, A UI DEVE exibir cada mensagem de erro de validação adjacente ao campo de entrada que causou o erro, e DEVE preservar todos os dados inseridos pelo usuário nos campos do formulário
4. A UI DEVE renderizar sem overflow horizontal, sem truncamento de conteúdo, e com todos os elementos interativos visíveis e operáveis em larguras de viewport de 320px a 1920px
5. SE uma submissão de formulário falhar na validação, ENTÃO A UI DEVE manter todos os valores previamente inseridos em seus respectivos campos de entrada para que o usuário possa corrigir apenas as entradas inválidas
6. A UI DEVE fornecer um campo de data no formulário de Registro de Partida que permita ao usuário selecionar a data via um calendário compacto (date picker) ou digitá-la manualmente no formato DD/MM/AAAA

### Requisito 9: Arquitetura da API

**História de Usuário:** Como desenvolvedor, quero que o back-end siga padrões de arquitetura limpa e modular, para que o código seja manutenível e escalável.

#### Critérios de Aceitação

1. A API DEVE ser implementada usando o padrão ASP.NET Core Minimal API no .NET 8
2. A API DEVE usar Entity Framework Core para todas as interações com o banco de dados PostgreSQL
3. A API DEVE separar responsabilidades em camadas distintas de modo que nenhum arquivo de definição de endpoint da API contenha lógica de consulta direta ao banco de dados, e nenhum arquivo de acesso a dados contenha lógica de tratamento de resposta HTTP
4. A API DEVE retornar todas as respostas de sucesso e erro como objetos JSON contendo, no mínimo, os dados do recurso relevante para respostas de sucesso ou um campo de mensagem de erro para respostas de falha
5. SE uma exceção não tratada ocorrer durante o processamento da requisição, ENTÃO A API DEVE retornar status HTTP 500 com uma resposta JSON de erro que não inclua stack traces, nomes de tipo de exceção, detalhes de conexão com banco de dados ou caminhos internos de arquivos
6. A API DEVE usar injeção de construtor ou injeção de método via o container de injeção de dependência nativo para fornecer dependências de lógica de negócio e acesso a dados às definições de endpoints
