<div align="center">
  <img src="Custom\img\logoMP.png" width="120">
  <h1>CONSTRUTIVA (Back-End)</h1>
  <p><strong>Motor de Gestão para Construção Civil com Alta Performance</strong></p>
  
  [![.NET 9](https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet)](https://dotnet.microsoft.com/)
  [![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-4169E1?style=for-the-badge&logo=postgresql)](https://www.postgresql.org/)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=for-the-badge)](https://opensource.org/licenses/MIT)
</div>

---

## Sobre o Projeto

O **Construtiva API** é o núcleo de processamento e persistência da plataforma Construtiva. Desenvolvido para oferecer máxima confiabilidade, o back-end gerencia a lógica complexa de canteiros de obra, desde orçamentos e aditivos até o controle rigoroso de diários de obra e documentação técnica.

Esta API foi projetada para ser escalável, segura e fornecer dados em tempo real para a interface Angular, garantindo que engenheiros e arquitetos tenham informações precisas para tomadas de decisão cirúrgicas.

## Inspiração & Origem

Este projeto nasceu de uma ideia original de sistema da empresa **Medeiros e Pinheiro ARQ/ENG**, referência em soluções técnicas para construção civil.

🔗 **Visite o site oficial:** [medeirosepinheiro.com.br](https://medeirosepinheiro.com.br/)

## Funcionalidades Principais

- 🔐 **Autenticação Segura**: Implementação robusta de JWT (JSON Web Tokens) com ASP.NET Identity.
- 🏗️ **Core de Gestão de Obras**: CRUD completo de Obras, Aditivos, Manutenções e Documentos.
- 📝 **Diários de Obra Inteligentes**: Registro detalhado de atividades, clima e fotos do canteiro.
- ✅ **Sistemas de Checklist**: Validação técnica e de segurança parametrizável.
- 📁 **Central de Documentos**: Armazenamento e metadados de arquivos técnicos por obra.
- 📊 **Agregadores de Dashboard**: Endpoints otimizados para fornecimento de estatísticas em tempo real.

## Tecnologias Utilizadas

- **Core**: [.NET 9.0 (ASP.NET Core Web API)](https://dotnet.microsoft.com/)
- **ORM**: [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- **Banco de Dados**: [PostgreSQL](https://www.postgresql.org/)
- **Segurança**: [ASP.NET Core Identity](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity) & [JWT Bearer](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/jwt-auth)
- **Documentação**: [Swagger / OpenAPI](https://swagger.io/)

## Autor

Desenvolvido por **Lucas Alberto**.

---

## Como Executar o Projeto

### Pré-requisitos
- [.NET SDK 9.0+](https://dotnet.microsoft.com/download/dotnet/9.0)
- [PostgreSQL](https://www.postgresql.org/download/) rodando localmente (ou via Docker)

### Instalação & Configuração
1. Clone o repositório
2. Configure a string de conexão no arquivo `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Database=construtiva_db;Username=seu_usuario;Password=sua_senha"
   }
   ```
3. Restaure as dependências:
   ```bash
   dotnet restore
   ```
4. Aplique as migrações do banco de dados:
   ```bash
   dotnet ef database update
   ```

### Iniciando o Servidor de API
```bash
dotnet run
```
Acesse `http://localhost:5001/swagger` no seu navegador para visualizar a documentação interativa.

---

<div align="center">
  <p>© 2026 Construtiva - Todos os direitos reservados.</p>
</div>
