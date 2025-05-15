# ?? FIAP Cloud Games - API de Cadastro e Biblioteca de Jogos

Este projeto é a entrega da **Fase 1** do Tech Challenge da FIAP e consiste em uma **API RESTful desenvolvida em
.NET 8** para gerenciar o **cadastro de usuários** e sua **biblioteca de jogos adquiridos**.
Essa API é o ponto de partida para uma plataforma robusta de games voltada à educação em tecnologia.

---

## ?? Objetivo

Criar um MVP funcional, utilizando arquitetura limpa, boas práticas de desenvolvimento, persistência de dados com Entity Framework Core 
e autenticação com JWT, que sirva como base sólida para as próximas fases do projeto.

---

## ??? Tecnologias Utilizadas

- .NET 8 (ASP.NET Core MVC)
- Entity Framework Core + Migrations
- Docker (para PostgreSQL)
- JWT Authentication & Authorization
- xUnit + Moq para testes unitários
- Testes automatizados
- Swagger (Swashbuckle)
- Domain-Driven Design (DDD)
- Middlewares personalizados (log e tratamento de exceções)

---

## ?? Funcionalidades

### ? Cadastro e Gerenciamento de Usuários
- Cadastro com nome, e-mail e senha segura
- Validação de e-mail e senha forte
- Atualização e exclusão de usuários
- Filtro por nome ou e-mail

### ?? Autenticação e Permissões
- Login com geração de token JWT
- Controle de acesso por roles (`Admin`, `User`)
- Permissões separadas em entidade `UserAuthorization` (relacionamento 1:1)

### ?? Biblioteca de Jogos
- Associação de jogos a usuários
- Listagem de jogos adquiridos por usuário
- Exclusão de jogos da biblioteca

---

## ?? Arquitetura
?? UserRegistrationAndGameLibrary
??? ?? Api # Controllers, Middlewares, Program.cs
??? ?? Application # DTOs, Interfaces de Serviço
??? ?? Domain # Entidades, Enums, Value Objects
??? ?? Infra # DbContext, Migrations, Repositórios

- Arquitetura em camadas com separação clara de responsabilidades
- Uso de DDD e boas práticas REST
- Injeção de dependência configurada com `AddScoped`

---

### ? Passos

1. Clone o repositório:
```bash
git clone https://github.com/leticiacarolinesilva/UserRegistrationAndGameLibrary.git

2. Caso não tenha o postgreSQL instalado você pode iniciar um container PostgreSQL localmente com o seguinte comando:

```bash
docker run --name meu-postgres \
  -p 5432:5432 \
  -e POSTGRES_USER=gameuser \
  -e POSTGRES_PASSWORD=gamepassword \
  -e POSTGRES_DB=gameplatform \
  -d postgres

  3. Execute o projeto

  4. Acesse o Swagger: https://localhost:7213/swagger


