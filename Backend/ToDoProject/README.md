# Clean Architecture Web API Template

A simple and practical Clean Architecture starter template for building .NET Web APIs.

The template provides a clean separation between API, application logic, domain models, and infrastructure while intentionally avoiding unnecessary framework and architectural complexity.

## Architecture

The generated solution contains four projects:

```text
ToDoProject
├── ToDoProject.API
├── ToDoProject.Application
├── ToDoProject.Core
└── ToDoProject.Infrastructure
```

### ToDoProject.API

Contains the Web API layer.

Responsibilities include:

- Minimal API endpoints
- HTTP request and response handling
- Endpoint registration
- Swagger configuration

Endpoints are organized inside the `Endpoints` folder and registered centrally through `MapAllEndpoints.cs`.

### ToDoProject.Application

Contains application and business logic.

Main folders:

```text
BLL/
Interfaces/
```

Responsibilities include:

- Business logic
- Business rules
- BLL interfaces
- DAL interfaces
- External service interfaces

### ToDoProject.Core

Contains the core models and enums used by the application.

Main folders:

```text
Models/
Enums/
```

This project has no dependency on the other application projects.

### ToDoProject.Infrastructure

Contains implementations for infrastructure concerns.

Main folders:

```text
DAL/
Services/
```

Responsibilities include:

- Data access implementations
- Email services
- File/storage services
- External API integrations
- Other infrastructure concerns

## Dependency Flow

```text
API
 ↓
Application
 ↓
Core

Infrastructure
 ↓
Application
 ↓
Core
```

The API also references Infrastructure for dependency registration.

## Example Feature

The template includes a small `Product` example demonstrating the intended structure:

```text
ProductEndpoints
      ↓
IProductBLL
      ↓
ProductBLL
      ↓
IProductDAL
      ↓
ProductDAL
```

An example `IEmailService` / `EmailService` is also included to demonstrate how external or technical services should be organized.

The example uses in-memory data and is intended only to demonstrate the architecture.

## Install

Install the template from NuGet:

```bash
dotnet new install HammadMaqbool.CleanArchitecture.Template
```

## Create a Project

Create a new application:

```bash
dotnet new cleanarch -n MyProject
```

This generates:

```text
MyProject/
├── MyProject.slnx
├── AGENTS.md
├── MyProject.API/
├── MyProject.Application/
├── MyProject.Core/
└── MyProject.Infrastructure/
```

Open `MyProject.slnx` in Visual Studio and run the API project.

## AI Coding Agent Instructions

The generated solution includes an `AGENTS.md` file.

It provides architectural guidance for compatible AI coding agents and instructs them to understand and clarify requirements before implementing features.

The instructions also describe where BLL, DAL, services, models, enums, endpoints, and dependency registrations belong.

`AGENTS.md` is development guidance only and has no effect on application runtime.

## Intentionally Not Included

The template does not automatically introduce:

- CQRS
- MediatR
- AutoMapper
- FluentValidation
- Unit of Work
- Result patterns

These can be added later when a project's requirements justify them.

The goal is to provide a clean starting architecture without forcing additional patterns or libraries on every application.

## Requirements

- .NET 10 SDK
- Visual Studio 2026 or another .NET 10 compatible development environment

## License

This project is licensed under the MIT License.

## Source Code

Source code and development are available on GitHub:

https://github.com/HammadMaqbool/CleanArchitectureTemplate