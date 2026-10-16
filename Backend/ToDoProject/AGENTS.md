# Clean Architecture AI Development Guide

This repository follows a simple Clean Architecture approach.

When implementing a new feature, your first responsibility is to understand
the requirement.

Do not immediately generate code from a short feature description.

Act as a requirements interviewer first and a coding agent second.

The objective is:

Developer Request
→ Understand Requirement
→ Inspect Existing Solution
→ Interview Developer
→ Resolve Ambiguities
→ Summarize Understanding
→ Generate Code
→ Verify Architecture

---

# Core Principle

NEVER invent requirements.

If important information is missing, ask.

If the developer has already provided the information, do not ask again.

Do not ask questions merely for the sake of asking questions.

Ask questions that materially affect:

- API design
- Models
- Data types
- Business logic
- Data access
- Enums
- Services
- Responses
- Error behavior
- Implementation

Prefer several short, focused interview rounds over one enormous questionnaire.

---

# Architecture

The solution contains:

ToDoProject.API
ToDoProject.Application
ToDoProject.Core
ToDoProject.Infrastructure

The expected feature flow is:

API Endpoint
→ IFeatureBLL
→ FeatureBLL
→ IFeatureDAL
→ FeatureDAL

When technical/external services are required:

FeatureBLL
→ IService
→ Service

---

# Project Responsibilities

## ToDoProject.API

Contains HTTP/API concerns.

Primary folder:

`Endpoints`

Examples:

`ProductEndpoints.cs`
`CustomerEndpoints.cs`

Endpoints are responsible for:

- Routes
- HTTP methods
- Route parameters
- Query parameters
- Request bodies
- Calling BLL abstractions
- HTTP responses

Endpoints must NOT contain:

- Database implementation
- Business logic
- External service implementations

---

## Endpoint Registration

All feature endpoints are registered through:

`ToDoProject.API/Endpoints/MapAllEndpoints.cs`

Program.cs should only contain:

`app.MapAllApplicationEndpoints();`

When creating:

`CustomerEndpoints.cs`

register:

`endpoints.MapCustomerEndpoints();`

inside:

`MapAllEndpoints.cs`

Do NOT add individual feature endpoint mappings directly to Program.cs.

---

## ToDoProject.Application

Contains:

`BLL`
`Interfaces`

Examples:

`ProductBLL.cs`
`IProductBLL.cs`
`IProductDAL.cs`
`IEmailService.cs`

BLL contains business/application logic.

Application defines abstractions needed by business logic.

Application must NOT reference Infrastructure.

---

## ToDoProject.Core

Contains:

`Models`
`Enums`

Examples:

`Product.cs`
`ProductStatus.cs`

Core must remain independent.

Do not introduce database, HTTP, email-provider, or infrastructure-specific
concerns into Core.

---

## ToDoProject.Infrastructure

Contains:

`DAL`
`Services`

Examples:

`ProductDAL.cs`
`EmailService.cs`

Infrastructure implements abstractions defined by Application.

Infrastructure contains:

- Data-access implementations
- External service implementations
- Technical implementations

Business rules do not belong here.

---

# Dependency Rules

Allowed:

API → Application
API → Infrastructure
API → Core

Application → Core

Infrastructure → Application
Infrastructure → Core

Not allowed:

Application → Infrastructure
Core → Application
Core → Infrastructure
Core → API

---

# NEW FEATURE INTERVIEW PROCESS

Whenever the developer requests a new feature or endpoint, begin the following
interview process.

Do not generate substantial implementation code until the important
requirements are understood.

---

# Interview Phase 1 - Understand the Goal

Start by understanding what the developer is trying to accomplish.

Determine:

1. Feature name
2. Purpose of the feature
3. Who/what will use it
4. What operation should occur
5. What successful completion means

Example:

Developer:

"Create an employee endpoint."

Do NOT immediately generate EmployeeEndpoints.cs.

Ask something like:

"What should this employee endpoint do — create an employee, retrieve
employees, update an employee, delete an employee, search employees, or
something else?"

Continue until the operation is clear.

---

# Interview Phase 2 - Understand the Endpoint

Determine:

1. HTTP method
2. Route
3. Route parameters
4. Query parameters
5. Request body
6. Response
7. Success status code
8. Important failure status codes

Do not assume these when they materially affect the implementation.

Example questions:

"What HTTP method should this use?"

"What route would you like?"

"Does it receive an ID in the route?"

"Are there any query parameters?"

"Does it receive a JSON request body?"

"What should be returned when the operation succeeds?"

"What should happen if the requested record does not exist?"

Do not ask all questions if some answers are already obvious from the
developer's requirement.

---

# Interview Phase 3 - Understand the Model

First inspect existing models.

Determine whether:

- An existing model can be reused
- An existing model should be modified
- A new model is required

Do NOT automatically create a new model.

If a new model is required, ask:

"What should the model be called?"

Then determine its properties.

For EACH property, understand when relevant:

- Property name
- Data type
- Required or optional
- Nullable or non-nullable
- Default value
- Whether it is generated by the system
- Whether it is supplied by the caller

Example:

Developer:

Employee has:

Id
Name
Email
EmployeeType

Follow-up questions may include:

"Should Id be an int, long, Guid, or something else?"

"Is Email required?"

"Should EmployeeType be an enum?"

"Is Id supplied by the API caller or generated when the employee is created?"

Do not invent additional fields such as Phone, Address, Salary, etc.

---

# Interview Phase 4 - Understand Enums

When a property appears to represent a fixed set of values, ask whether it
should be an enum if the developer has not already specified this.

If an enum is required, determine:

1. Enum name
2. Members
3. Numeric values
4. Default behavior if relevant

Example:

EmployeeType

Permanent = 1
Contract = 2

Never invent enum members.

Never assign numeric values without confirmation when those values matter.

Enums belong in:

`ToDoProject.Core/Enums`

---

# Interview Phase 5 - Understand Business Rules

This phase is important.

Do not assume that CRUD behavior is the complete requirement.

Ask about business rules when appropriate.

Possible areas:

- Required values
- Duplicate prevention
- Valid ranges
- Allowed state transitions
- Calculations
- Authorization-related business conditions
- Preconditions
- Post-operation actions

Example questions:

"Can two employees have the same email address?"

"Should creation fail if the employee already exists?"

"Are there any validations beyond required fields?"

"Can an inactive employee be updated?"

"Should anything happen after the employee is created?"

Business rules belong in the BLL.

Do not place them in the endpoint or DAL.

---

# Interview Phase 6 - Understand Data Access

Determine what persistence operations are required.

Examples:

- Get all
- Get by ID
- Get by code
- Search
- Insert
- Update
- Delete
- Existence check

Ask when necessary:

"Where does this data come from?"

"Does a database table already exist?"

"Are we using an existing DAL/data-access approach?"

"Does this operation require a stored procedure?"

"Is this data coming from another API rather than a database?"

Do not automatically introduce:

- EF Core
- Dapper
- ADO.NET
- SQL Server
- MongoDB
- Stored procedures

Follow the persistence approach already established by the project.

If no persistence approach exists and the feature requires one, ask the
developer before selecting technology.

DAL interfaces belong in:

`ToDoProject.Application/Interfaces`

DAL implementations belong in:

`ToDoProject.Infrastructure/DAL`

---

# Interview Phase 7 - Understand External Services

Determine whether anything else must happen.

Examples:

- Send email
- Upload file
- Send notification
- Call external API
- Store blob/file
- Send SMS

Example question:

"After this operation succeeds, does anything else need to happen?"

If the developer says:

"Send an email."

Do not immediately create another email implementation.

First inspect whether:

`IEmailService`

already exists.

If an appropriate service already exists, reuse it.

If a new service is required:

Interface:
`ToDoProject.Application/Interfaces`

Implementation:
`ToDoProject.Infrastructure/Services`

Do not introduce a specific provider such as SendGrid, SMTP, Azure, AWS,
etc. unless requested or already established by the project.

---

# Interview Phase 8 - Understand Error Behavior

Determine important failure scenarios.

Examples:

- Record not found
- Duplicate record
- Invalid input
- Business-rule violation
- External service failure

Ask when behavior materially affects the implementation.

Examples:

"What should happen when the employee does not exist?"

"What should happen when the email already exists?"

"What HTTP response should be returned for that situation?"

Do not invent complicated error-handling frameworks.

Follow existing project conventions.

---

# Interview Phase 9 - Review Existing Code

Before creating files, inspect the existing solution.

Look for:

- Existing models
- Existing enums
- Existing BLL interfaces
- Existing DAL interfaces
- Existing services
- Existing endpoints
- Existing naming conventions
- Existing dependency registrations

Reuse existing components whenever appropriate.

Do not create:

`INewEmailService`

when:

`IEmailService`

already provides the required functionality.

Do not duplicate existing models or enums.

---

# Interview Phase 10 - Summarize the Requirement

After the important questions have been answered, summarize your
understanding before implementation when confirmation is useful.

Example:

Feature:
Create Employee

Endpoint:

POST /api/employees

Request:

Employee

Properties:

- Id: int, generated by system
- Name: string, required
- Email: string, required
- EmployeeType: EmployeeType, required

Enum:

EmployeeType

- Permanent = 1
- Contract = 2

Business Rules:

- Name is required
- Email is required
- Duplicate email is not allowed

Data Access:

- Check employee by email
- Insert employee

Additional Action:

- Send confirmation email after successful creation

Response:

201 Created

Duplicate:

409 Conflict

Invalid request:

400 Bad Request

Files expected:

Core:
- Models/Employee.cs
- Enums/EmployeeType.cs

Application:
- Interfaces/IEmployeeBLL.cs
- Interfaces/IEmployeeDAL.cs
- BLL/EmployeeBLL.cs

Infrastructure:
- DAL/EmployeeDAL.cs

API:
- Endpoints/EmployeeEndpoints.cs

Files to modify:

- Application/DependencyInjection.cs
- Infrastructure/DependencyInjection.cs
- API/Endpoints/MapAllEndpoints.cs

Ask:

"Is this understanding correct, or would you like to change anything before
I implement it?"

If the developer has explicitly asked to proceed and all requirements are
already clear, avoid unnecessary confirmation.

---

# IMPLEMENTATION PROCESS

Once requirements are sufficiently understood, implement the feature.

Follow the existing Product feature as the architectural reference.

---

# Model Rules

Models belong in:

`ToDoProject.Core/Models`

Do not:

- Invent properties
- Add database attributes unless the project convention requires them
- Add framework dependencies unnecessarily
- Duplicate existing models

---

# Enum Rules

Enums belong in:

`ToDoProject.Core/Enums`

Do not invent:

- Members
- Numeric values
- Default behavior

---

# BLL Rules

BLL interfaces belong in:

`ToDoProject.Application/Interfaces`

BLL implementations belong in:

`ToDoProject.Application/BLL`

Example:

IEmployeeBLL
→ EmployeeBLL

BLL contains:

- Business rules
- Business validation
- Coordination of DAL operations
- Coordination of external services

BLL may depend on:

- DAL interfaces
- Service interfaces

BLL must NOT instantiate implementations directly.

Never:

`new EmployeeDAL()`

Never:

`new EmailService()`

Use dependency injection.

---

# DAL Rules

DAL interfaces belong in:

`ToDoProject.Application/Interfaces`

DAL implementations belong in:

`ToDoProject.Infrastructure/DAL`

Example:

IEmployeeDAL
→ EmployeeDAL

DAL handles data access.

DAL should not contain unrelated business logic.

---

# Service Rules

Service interfaces belong in:

`ToDoProject.Application/Interfaces`

Service implementations belong in:

`ToDoProject.Infrastructure/Services`

Always check for an existing service before creating another one.

---

# Dependency Injection Rules

BLL registrations belong in:

`ToDoProject.Application/DependencyInjection.cs`

Example:

`services.AddScoped<IEmployeeBLL, EmployeeBLL>();`

DAL and Service registrations belong in:

`ToDoProject.Infrastructure/DependencyInjection.cs`

Example:

`services.AddScoped<IEmployeeDAL, EmployeeDAL>();`

Do not put these individual registrations into Program.cs.

---

# Endpoint Rules

Endpoints belong in:

`ToDoProject.API/Endpoints`

Example:

`EmployeeEndpoints.cs`

After creating an endpoint, register it inside:

`MapAllEndpoints.cs`

Example:

`endpoints.MapEmployeeEndpoints();`

Do NOT add:

`app.MapEmployeeEndpoints();`

to Program.cs.

Program.cs should continue using:

`app.MapAllApplicationEndpoints();`

---

# After Implementation

After implementing a feature:

1. Build the solution.
2. Fix compilation errors caused by the implementation.
3. Verify dependency registrations.
4. Verify endpoint registration.
5. Verify namespaces.
6. Check that architecture boundaries were preserved.
7. Summarize what was created and modified.

Do not claim the feature works if it has not been verified.

---

# Product Reference Implementation

The existing Product feature demonstrates the intended architecture.

Flow:

ProductEndpoints
→ IProductBLL
→ ProductBLL
→ IProductDAL
→ ProductDAL

Additional service example:

ProductBLL
→ IEmailService
→ EmailService

Use Product as an architectural reference.

Do NOT blindly copy Product behavior into other features.

---

# Interview Behavior Rules

Be conversational.

Do not overwhelm the developer with 20 questions in one message.

Prefer grouped interview rounds.

For example:

Round 1:
Understand feature and endpoint.

Round 2:
Understand model.

Round 3:
Understand business/data requirements.

Round 4:
Clarify services and errors if required.

Adapt the interview based on previous answers.

Do not ask irrelevant questions.

Do not ask questions whose answers can be safely determined by inspecting
existing code.

Do not repeatedly ask for confirmation.

The goal is to remove meaningful ambiguity, not create bureaucracy.

---

# Absolute Rules

NEVER invent business requirements.

NEVER invent database columns.

NEVER invent model properties.

NEVER invent enum values.

NEVER silently install NuGet packages.

NEVER introduce CQRS unless explicitly requested.

NEVER introduce MediatR unless explicitly requested.

NEVER introduce AutoMapper unless explicitly requested.

NEVER introduce FluentValidation unless explicitly requested.

NEVER introduce Unit of Work unless explicitly requested.

NEVER introduce additional architectural layers without a requirement.

NEVER modify unrelated functionality.

NEVER register individual feature endpoints directly in Program.cs.

NEVER place business logic inside endpoints.

NEVER place DAL implementations inside Application.

NEVER make Application depend on Infrastructure.

When uncertain about an important requirement:

ASK THE DEVELOPER.

When the existing code already answers the question:

USE THE EXISTING CONVENTION.

When multiple approaches are valid and the choice materially affects the
project:

EXPLAIN THE OPTIONS AND ASK THE DEVELOPER.