# 🩷 WebApiPJConnect

API para gerenciamento de empresas, usuários e dados corporativos, desenvolvida em **.NET 8** com **Entity Framework Core** e arquitetura em camadas (**Domain**, **Application**, **Infra.Data**, **API**).

---

## 🧠 Funcionalidades

- **Empresas**
  - Criar nova empresa
  - Atualizar dados cadastrais
  - Consultar empresa por CNPJ
- **Usuários**
  - Cadastrar usuário vinculado a uma empresa
  - Garantir unicidade de CPF dentro da empresa
  - Listar e consultar usuários

---

## 📂 Estrutura do Projeto

```
WebApiPJConnect/
├── WebApiPJConnect.API           # Controllers e Endpoints
├── WebApiPJConnect.Application   # Serviços, DTOs e casos de uso
├── WebApiPJConnect.Domain        # Entidades, Value Objects e Regras de Negócio
├── WebApiPJConnect.Infra.Data    # Configuração do EF Core, Migrations e Repositórios
└── WebApiPJConnect.Tests         # Testes unitários
```

---

## 🛠️ Tecnologias Utilizadas

- **.NET 8**
- **Entity Framework Core**
- **SQL Server** (padrão, adaptável)
- **Swagger** para documentação
- **xUnit** + **Moq** + **FluentAssertions** para testes

---

## ⚙️ Configuração do Ambiente

1️⃣ **Clonar o Repositório**

```bash
git clone https://github.com/seu-repositorio/WebApiPJConnect.git
cd WebApiPJConnect
```

2️⃣ **Configurar o Banco de Dados** Edite `appsettings.Development.json` no projeto **API**:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=PJConnectDB;User Id=sa;Password=SuaSenha;"
  }
}
```

3️⃣ **Criar e Aplicar Migrations**

```bash
cd WebApiPJConnect.Infra.Data
dotnet ef migrations add InitialCreate
dotnet ef database update
```

---

## ▶️ Executando a Aplicação

```bash
cd ../WebApiPJConnect.API
dotnet run
```

Acesse:

- API: `https://localhost:7019`
- Swagger: `https://localhost:7019/swagger`

---

## 🤓☝🏻 Exemplos de Uso

### Criar Empresa

**POST** `/companies`

```json
{
  "tradeName": "NovaTech Solutions",
  "legalName": "NovaTech Solutions LTDA",
  "cnpj": "56789012000155",
  "street": "Rua das Palmeiras",
  "number": "789",
  "city": "São Paulo",
  "state": "SP",
  "zipCode": "04567000",
  "type": "MEI",
  "partners": [
    {
      "name": "Carlos Souza",
      "cpf": "98765432100"
    }
  ]
}
```

### Atualizar Empresa

**PUT** `/companies/{id}`

```json
{
  "tradeName": "NovaTech Atualizada",
  "legalName": "NovaTech Solutions LTDA",
  "street": "Rua Alterada",
  "number": "1000",
  "city": "São Paulo",
  "state": "SP",
  "zipCode": "04567000",
  "type": "ME"
}
```

### Criar Usuário

**POST** `/companies/{companyId}/users`

```json
{
  "name": "Ana",
  "cpf": "52998224725",
  "profile": "Agencia"
}
```

---

## 🧪 Executando Testes

```bash
dotnet test
```

Testes cobrem:

- Criação de empresas e usuários
- Validação de CPF único
- Respostas HTTP (`200`, `404`, `409`)

---

## 📜 Licença

Licenciado sob a [MIT License](LICENSE).

