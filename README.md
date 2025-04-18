# UserService API (.NET 8 - Clean Architecture)

This is a fully containerized, self-contained .NET 8 Web API with:
- Clean Architecture + CQRS with MediatR
- SQL Server + EF Core
- Full test coverage with xUnit, Moq, FluentAssertions

## 🚀 One-liner to Run

```bash
docker-compose up --build
```

This spins up:
- ✅ API (localhost:5000)
- 🛢️ SQL Server (localhost:1433)

## 🛠️ Dev Tools

- Swagger: http://localhost:5000/swagger

## 🧪 Test the API

```bash
dotnet test
```


## 📦 Migrations (DB Schema Setup)

These are auto-applied on app start via:

```csharp
db.Database.Migrate();
```

To generate one manually:

```bash
dotnet ef migrations add InitialCreate -s src/UserService.API -p src/UserService.Persistence
dotnet ef database update -s src/UserService.API -p src/UserService.Persistence
```

---

### 📁 Folder Structure

```
src/
├── UserService.API
├── UserService.Application
├── UserService.Domain
└── UserService.Persistence
tests/
└── UserService.API.Unit.Tests
└── UserService.Application.UnitTests
└── UserService.Persistenct.UnitTests
└── UserService.API.Integrtion.Tests
```

### 📦Future Enhancemeent/Improvement

- Authentication OAUTH JWT and Authorization (Optional)
- Serilog + Seq logging
- OpenTelemetry observability
- Global Exception Handling
- Fluent validation for Requent in pipeline