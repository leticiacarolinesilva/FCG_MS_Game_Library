# FIAP Cloud Games - API de Cadastro e Biblioteca de Jogos

Desenvolvido por **Letícia Caroline** e **Daniel** como parte da fase 2 do curso de Arquitetura .NET na FIAP. Neste projeto, evoluímos uma aplicação monolítica para um ambiente real de produção na AWS com Docker, CI/CD e monitoramento.

---

# 📌 Principais Tecnologias

- **.NET 8** – API e camadas de domínio, aplicação e infraestrutura  
- **Docker (multi-stage)** – Build, teste e imagem final com aspnet:8.0  
- **GitHub Actions (CI/CD)** – Build/testes automáticos e publicação no ECR  
- **AWS EC2** – Deploy manual da imagem Docker  
- **AWS ECR** – Registro de imagens da aplicação  
- **Amazon RDS (PostgreSQL)** – Banco persistente em nuvem  
- **New Relic** – Monitoramento de performance e logs em produção

---

##  Funcionalidades

### Cadastro e Gerenciamento de Usuários
- Cadastro com nome, e-mail e senha segura
- Validação de e-mail e senha forte
- Atualização e exclusão de usuários
- Filtro por nome ou e-mail

###  Autenticação e Permissões
- Login com geração de token JWT
- Controle de acesso por roles (`Admin`, `User`)
- Permissões separadas em entidade `UserAuthorization` (relacionamento 1:1)

### Biblioteca de Jogos
- Associação de jogos a usuários
- Listagem de jogos adquiridos por usuário
- Exclusão de jogos da biblioteca

---

##  Arquitetura
- UserRegistrationAndGameLibrary
  - Api # Controllers, Middlewares, Program.cs
  - Application # DTOs, Interfaces de Serviço
  - Domain # Entidades, Enums, Value Objects
  - Infra # DbContext, Migrations, Repositórios

- Arquitetura em camadas com separação clara de responsabilidades
- Uso de DDD e boas práticas REST
- Injeção de dependência configurada com `AddScoped`

---

##  CI/CD com GitHub Actions

- **CI (Pull Request)**: build da solução e execução dos testes unitários (dotnet test)
- **CD (Merge para master)**: construção da imagem Docker e envio automático ao ECR com a tag latest

Dessa forma, garantimos entregas consistentes e automatizadas com validação prévia.

---

##  Monitoramento com New Relic

O agent do New Relic foi instalado na instância EC2

Coleta de métricas de CPU, memórias, latência e throughput

Logs da aplicação são enviados com estrutura JSON para o New Relic Logs

Dashboards customizados monitoram erros, status codes e desempenho em tempo real

---

###  Passos

1. Clone o repositório:
```bash
git clone https://github.com/leticiacarolinesilva/UserRegistrationAndGameLibrary.git
```

2. Entre na pasta e rode Docker Compose (para testes locais com PostgreSQL):
```bash
docker-compose up --build
```

4. Acesse Swagger: http://localhost:3001/swagger/index.html



