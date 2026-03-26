# Invoice System - SaaS Invoice Platform

A full-stack, production-ready invoice management system built with modern cloud-native architecture. Built for small to medium businesses to manage invoices, customers, and generate comprehensive financial reports.

## 🎯 Overview

Invoice System is a complete SaaS platform that enables users to:
- Create, manage, and track invoices
- Maintain customer databases
- Monitor invoice payment statuses
- Generate financial reports and analytics
- Export invoices and reports

The system is built with a **Clean Architecture** backend and a modern **Angular 18** frontend, designed for scalability, testability, and maintainability.

---

## 🏗️ Architectural Keypoints

### Backend Architecture (Clean Layers)

The backend follows **Clean Architecture** principles with strict separation of concerns:

```
┌─────────────────────────────────────────┐
│  Invoice.API (Presentation Layer)       │ ← REST API, Controllers, HTTP
├─────────────────────────────────────────┤
│  Invoice.Application (Business Logic)   │ ← MediatR Handlers, DTOs, Validators
├─────────────────────────────────────────┤
│  Invoice.Domain (Core Entities)         │ ← Domain Models, Value Objects, Rules
├─────────────────────────────────────────┤
│  Invoice.Infrastructure (Data Access)   │ ← EF Core, Repositories, Services
└─────────────────────────────────────────┘
```

### Key Architectural Patterns

- **CQRS (Command Query Responsibility Segregation)**
  - Commands: Write operations (CreateInvoice, UpdateCustomer)
  - Queries: Read operations (GetInvoices, GetInvoiceById)
  - Handled via MediatR for clean separation

- **Domain-Driven Design (DDD)**
  - Aggregate roots for Invoices, Customers
  - Value objects for types like Money, Address
  - Domain events for business rule violations

- **Repository Pattern**
  - Abstract persistence layer
  - Easy to mock for testing

- **Dependency Injection**
  - All dependencies automatically resolved
  - Service registration in layers

- **Pipeline Behaviors (Cross-Cutting Concerns)**
  - Validation Behavior: FluentValidation on all commands
  - Logging Behavior: Structured logging for all operations
  - Performance Behavior: Duration tracking for long operations

### Frontend Architecture

- **Feature-Based Organization**
  - Each feature (Auth, Invoices, Customers, Dashboard) is self-contained
  - Shared components library for reusable UI elements
  - Core services for API communication and state management

- **Lazy Loading**
  - Routes configured for lazy loading per feature module
  - Optimized bundle sizes

- **Angular 18 Standalone API**
  - Modern, simplified component structure
  - No need for NgModules for most features
  - Tree-shakeable code

- **Reactive Forms & Signals**
  - Type-safe reactive programming
  - RxJS observables for async data
  - Angular signals for fine-grained reactivity

---

## 🛠️ Tech Stack

### Backend
- **Framework**: ASP.NET Core 8.0
- **Language**: C# 12 with latest features
- **API Pattern**: RESTful API with versioning (Asp.Versioning)
- **Command/Query Bus**: MediatR
- **Validation**: FluentValidation
- **Mapping**: AutoMapper
- **Database**: PostgreSQL 16
- **ORM**: Entity Framework Core 8
- **Authentication**: JWT Bearer tokens with Identity
- **Logging**: Serilog (Console + File)
- **API Documentation**: Swagger/OpenAPI
- **Health Checks**: Built-in EF Core health checks
- **Rate Limiting**: ASP.NET Core Rate Limiting

### Frontend
- **Framework**: Angular 18.2
- **Language**: TypeScript 5.5
- **UI Library**: Angular Material 18.2
- **State Management**: Angular Signals, RxJS
- **Routing**: Angular Router (lazy loading)
- **Build Tool**: Angular CLI (webpack/esbuild)
- **Styling**: SCSS with custom theme (Azure Blue)
- **Testing**: Karma + Jasmine

### Infrastructure
- **Database**: PostgreSQL 16 (Alpine)
- **Containerization**: Docker & Docker Compose
- **Cloud Integration**: Azure Key Vault support (production) - <Still Ongoing>

---

## 📁 Project Structure

```
InvoiceSystem/
├── src/                                  # Backend source
│   ├── Invoice.API/                     # API Layer
│   │   ├── Controllers/                 # REST endpoints
│   │   ├── Middleware/                  # Exception handling, correlation IDs
│   │   ├── Services/                    # API-specific services
│   │   ├── Program.cs                   # Service registration & middleware
│   │   └── Dockerfile                   # Container definition
│   ├── Invoice.Application/             # Application Layer
│   │   ├── Customers/                   # Customer CQRS handlers
│   │   │   ├── Commands/                # Create, Update, Delete customer
│   │   │   └── Queries/                 # Get customers, GetById
│   │   ├── Invoices/                    # Invoice CQRS handlers
│   │   │   ├── Commands/                # Create, Update, Delete invoice
│   │   │   ├── EventHandlers/           # Domain event reactions
│   │   │   └── Queries/                 # Get invoices, GetById
│   │   ├── Reports/                     # Reporting queries
│   │   ├── Common/                      # Shared application logic
│   │   │   ├── Behaviours/              # Pipeline behaviors (validation, logging)
│   │   │   ├── DTOs/                    # Data Transfer Objects
│   │   │   ├── Interfaces/              # Service contracts
│   │   │   ├── Mappings/                # AutoMapper profiles
│   │   │   └── Settings/                # Configuration objects
│   ├── Invoice.Domain/                  # Domain Layer
│   │   ├── Entities/                    # Invoice, Customer aggregate roots
│   │   ├── ValueObjects/                # Money, EmailAddress, etc.
│   │   ├── Events/                      # Domain events
│   │   ├── Enums/                       # InvoiceStatus, PaymentMethod
│   │   ├── Exceptions/                  # Custom domain exceptions
│   │   └── Interfaces/                  # Repository interfaces
│   └── Invoice.Infrastructure/          # Infrastructure Layer
│       ├── Persistence/                 # EF Core DbContext, migrations
│       ├── Services/                    # Email, background jobs
│       └── DependencyInjection.cs       # Service registration
├── tests/                               # Test projects
│   ├── Invoice.UnitTests/              # Unit tests for domain & logic
│   └── Invoice.IntegrationTests/       # API integration tests
├── invoice-frontend/                    # Angular frontend
│   ├── src/
│   │   ├── app/
│   │   │   ├── core/                    # Core services, guards, interceptors
│   │   │   │   ├── services/            # AuthService, InvoiceService, etc.
│   │   │   │   ├── guards/              # Route guards
│   │   │   │   └── interceptors/        # HTTP interceptors
│   │   │   ├── features/                # Feature modules (lazy-loaded)
│   │   │   │   ├── auth/                # Login, register, password reset
│   │   │   │   ├── dashboard/          # Dashboard with stats
│   │   │   │   ├── invoices/           # Invoice CRUD & list
│   │   │   │   └── customers/          # Customer CRUD & list
│   │   │   ├── layout/                  # Shell layout, navbar, sidenav
│   │   │   ├── shared/                  # Shared UI components
│   │   │   │   ├── components/          # Page header, dialogs, etc.
│   │   │   │   └── pipes/               # Custom pipes
│   │   │   ├── app.routes.ts            # Route configuration
│   │   │   ├── app.config.ts            # Angular configuration
│   │   │   └── app.component.ts         # Root component
│   │   ├── environments/                # Environment configs
│   │   ├── styles.scss                  # Global styles & Material theme
│   │   └── index.html                   # Entry point
│   ├── angular.json                     # Angular CLI config
│   ├── package.json                     # Dependencies
│   ├── tsconfig.json                    # TypeScript config
│   └── proxy.conf.json                  # Dev server proxy for API
├── docker-compose.yml                   # Multi-container setup
├── InvoiceSystem.sln                    # Visual Studio solution
└── README.md                            # This file
```

---

## 🚀 Getting Started

### Prerequisites

#### For Backend
- **.NET 8 SDK** or later ([Download](https://dotnet.microsoft.com/download))
- **PostgreSQL 16+** or Docker
- **Visual Studio 2022** or VS Code with C# extension

#### For Frontend
- **Node.js 18+** and **npm 10+** ([Download](https://nodejs.org))
- **Angular CLI 18** (installed via npm)

#### Optional
- **Docker & Docker Compose** (for containerized setup)
- **Postman or Insomnia** (API testing)

### Local Development Setup

#### 1. Backend Setup

```bash
# Clone the repository
git clone <repo-url>
cd InvoiceSystem

# Restore NuGet packages
dotnet restore

# Create and apply database migrations (ensure PostgreSQL is running)
cd src/Invoice.API
dotnet ef database update

# Run the API (development mode)
dotnet run

# API will be available at: http://localhost:5000
# Swagger docs at: http://localhost:5000/swagger
```

**Configure DB Connection** (edit `appsettings.Development.json`):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=InvoiceDb;Username=postgres;Password=password"
  }
}
```

#### 2. Frontend Setup

```bash
# Navigate to frontend directory
cd invoice-frontend

# Install dependencies
npm install

# Start development server
npm start

# Frontend will be available at: http://localhost:4200
```

**Configure API endpoint** (edit `src/environments/environment.ts`):
```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5000'
};
```

---

## 🐳 Running with Docker

### Single Command Setup

```bash
# Start all services (API, Frontend, PostgreSQL)
docker-compose up --build

# Services:
# - API: http://localhost:8081
# - Frontend: http://localhost:4200 (via proxy)
# - PostgreSQL: localhost:5432
```

### Docker Compose Services

- **api**: ASP.NET Core 8 backend
- **postgres**: PostgreSQL 16 database with persistent volume

### Environment Variables (docker-compose.yml)

```yaml
ASPNETCORE_ENVIRONMENT: Development
ConnectionStrings__DefaultConnection: Host=postgres;Port=5432;Database=InvoiceDb;...
Jwt__Key: your-super-secret-key
Jwt__AccessTokenExpiryMinutes: 15
Email__SmtpHost: smtp.mailtrap.io
AllowedOrigins__0: http://localhost:4200
```

**⚠️ Note**: Update email and JWT settings before production deployment.

---

## 📡 API Structure

### Versioning
- API uses versioning: `/api/v1/...`
- Future versions can coexist without breaking changes

### Authentication
- **Bearer Token**: JWT tokens issued on login
- **Header**: `Authorization: Bearer <token>`
- **Token Expiry**: 15 minutes (configurable)
- **Refresh**: Use refresh token to get new access token

### Core Endpoints

#### Auth
```
POST   /api/v1/auth/register         # User registration
POST   /api/v1/auth/login            # User login
POST   /api/v1/auth/logout           # User logout
POST   /api/v1/auth/refresh          # Refresh token
```

#### Invoices
```
GET    /api/v1/invoices              # List invoices (paginated)
GET    /api/v1/invoices/{id}         # Get invoice by ID
POST   /api/v1/invoices              # Create invoice
PUT    /api/v1/invoices/{id}         # Update invoice
DELETE /api/v1/invoices/{id}         # Delete invoice
POST   /api/v1/invoices/{id}/send    # Send invoice via email
```

#### Customers
```
GET    /api/v1/customers             # List customers
GET    /api/v1/customers/{id}        # Get customer by ID
POST   /api/v1/customers             # Create customer
PUT    /api/v1/customers/{id}        # Update customer
DELETE /api/v1/customers/{id}        # Delete customer
```

#### Reports
```
GET    /api/v1/reports/summary       # Financial summary
GET    /api/v1/reports/invoices      # Invoice report
GET    /api/v1/reports/revenue       # Revenue report
```

### Response Format

**Success (200)**:
```json
{
  "data": { ... },
  "message": "Operation successful",
  "timestamp": "2026-03-27T10:30:00Z"
}
```

**Error (400/500)**:
```json
{
  "errors": ["Validation error 1", "Validation error 2"],
  "message": "Operation failed",
  "statusCode": 400,
  "timestamp": "2026-03-27T10:30:00Z"
}
```

---

## 🧪 Testing

### Backend Unit Tests

```bash
cd tests/Invoice.UnitTests
dotnet test
```

Tests cover:
- Domain logic and business rules
- Command/Query handlers
- Validator implementations

### Backend Integration Tests

```bash
cd tests/Invoice.IntegrationTests
dotnet test
```

Tests cover:
- Full API endpoint flows
- Database persistence
- Authentication & authorization

### Frontend Tests

```bash
cd invoice-frontend
npm test
```

---

## 📊 Database

### PostgreSQL Schema

- **Users**: ASP.NET Identity users for authentication
- **Invoices**: Invoice aggregates with line items
- **Customers**: Company/individual customer data
- **AuditLog**: Created/Modified tracking on entities
- **RefreshTokens**: Token management for JWT refresh

### Migrations

```bash
# Create a new migration
dotnet ef migrations add MigrationName

# Apply migrations
dotnet ef database update

# Revert last migration
dotnet ef database update PreviousMigration
```

---

## 🔐 Security

### Authentication & Authorization
- **JWT Bearer**: Stateless token-based auth
- **Password Policy**: Min 8 chars, uppercase, digit required
- **Email Verification**: Required for registration

### Input Validation
- **FluentValidation**: Server-side validation on all commands
- **CORS**: Restricted to configured origins only
- **Rate Limiting**: Policy-based endpoint throttling

### Data Protection
- **Secrets Management**: 
  - Development: `dotnet user-secrets`
  - Production: Azure Key Vault
- **Sensitive Fields**: Not logged or exposed in responses
- **SQL Injection**: Protected via EF Core parameterized queries

---

## 📝 Configuration

### Development (appsettings.Development.json)

```json
{
  "Jwt": {
    "Key": "your-dev-secret-key",
    "Issuer": "InvoiceAPI",
    "Audience": "InvoiceAPIUsers",
    "AccessTokenExpiryMinutes": 15
  },
  "Email": {
    "SmtpHost": "smtp.mailtrap.io",
    "SmtpPort": 587,
    "Username": "your-mailtrap-user",
    "Password": "your-mailtrap-pass",
    "FromAddress": "noreply@invoicesystem.com"
  }
}
```

### Environment Variables
All settings can be overridden via environment variables using `__` separator:
```bash
ConnectionStrings__DefaultConnection=...
Jwt__Key=...
Email__SmtpHost=...
```

---

## 📦 Dependencies Highlights

### Backend
- **MediatR**: CQRS command/query bus
- **FluentValidation**: Fluent validation API
- **AutoMapper**: Object mapping/DTOs
- **Serilog**: Structured logging
- **Entity Framework Core**: ORM for PostgreSQL
- **Asp.Versioning**: API versioning support

### Frontend
- **Angular Material**: Material Design UI components
- **RxJS**: Reactive programming
- **TypeScript**: Type-safe development
- **Karma/Jasmine**: Testing framework

---

## 🎯 Key Features

### User Features
✅ User authentication & profile management  
✅ Create and manage invoices  
✅ Track invoice status (Draft, Sent, Paid, Overdue)  
✅ Customer database management  
✅ Email invoice delivery  
✅ Financial dashboard with KPIs  
✅ Invoice and revenue reports  

### Developer Features
✅ Clean Architecture & CQRS pattern  
✅ Comprehensive API documentation (Swagger)  
✅ Structured logging & performance monitoring  
✅ Unit & integration test coverage  
✅ Docker containerization  
✅ Automated Entity Framework migrations  
✅ JWT authentication ready for production  

---

## 🚧 Deployment

### Build for Production

**Backend**:
```bash
dotnet publish -c Release -o ./publish
docker build -t invoice-api:latest .
```

**Frontend**:
```bash
cd invoice-frontend
npm run build
# Output in: dist/invoice-frontend/
```

### Environment Secrets

Before deploying, configure:
- PostgreSQL connection string
- JWT secret key (strong, random)
- SMTP credentials for emails
- Allowed CORS origins
- Azure Key Vault URL (if using Azure)

---

## 🤝 Git Workflow

```bash
# Create feature branch
git checkout -b feature/invoice-export

# Make changes and commit
git commit -am "feat: add invoice export to PDF"

# Push and create PR
git push origin feature/invoice-export
```

---

## 📚 Learning Resources

- [Clean Architecture by Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Domain-Driven Design (Eric Evans)](https://www.domainlanguage.com/ddd/)
- [CQRS Pattern](https://martinfowler.com/bliki/CQRS.html)
- [MediatR Docs](https://github.com/jbogard/MediatR)
- [Angular Official Docs](https://angular.io/docs)
- [PostgreSQL Docs](https://www.postgresql.org/docs/)

---

## 📞 Support & Troubleshooting

### Common Issues

**API connection refused**
```bash
# Ensure API is running
dotnet run
# Check port: http://localhost:5000/health
```

**Database connection error**
```bash
# Verify PostgreSQL is running
docker ps  # if using Docker
# Check connection string in appsettings.Development.json
```

**Frontend blank page**
```bash
# Clear cache and rebuild
npm cache clean --force
rm -rf node_modules dist
npm install && npm start
```

---

## 📄 License

This project is part of a SaaS Invoice Platform.

---

## ✨ Next Steps

- [ ] Add export to PDF feature
- [ ] Implement invoice templates
- [ ] Add multi-currency support
- [ ] Setup CI/CD pipeline
- [ ] Production deployment
- [ ] Performance monitoring & alerts

---

**Last Updated**: March 2026  
**Status**: Active Development
