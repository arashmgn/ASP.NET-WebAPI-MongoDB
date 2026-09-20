# Auth Service (ASP.NET Core Web API + MongoDB + JWT)

An authentication and user management service built with **Clean Architecture**, **MongoDB** and **JWT Bearer** authentication.

## Architecture

```
src/
  AuthService.Domain          Entities and repository contracts. No dependencies.
  AuthService.Application     Use cases (services), DTOs, mappers and abstractions
                              (IPasswordHasher, ITokenService). Depends on Domain only.
  AuthService.Infrastructure  MongoDB repositories, JWT token service, PBKDF2 password hasher.
  AuthService.Api             Controllers, exception middleware, composition root.
```

Dependencies point inward: `Api -> Infrastructure -> Application -> Domain`.

## Features

- User registration and login with JWT
- Password hashing with PBKDF2 and a per-user salt
- User CRUD where users can only modify their own account (admins can modify any)
- Unique indexes on username and email
- Problem Details error responses

## Run locally

Start MongoDB:

```bash
cp .env.example .env
docker compose up -d mongodb
```

Configure secrets and run the API:

```bash
cd src/AuthService.Api
dotnet user-secrets set "Jwt:Key" "<random string, at least 32 characters>"
dotnet user-secrets set "MongoDbSettings:ConnectionString" "mongodb://admin:<password>@localhost:27017"
dotnet run
```

Swagger UI is available at `/swagger`. Run the whole stack with `docker compose up --build` (values come from `.env`).

## Tech stack

ASP.NET Core 8, MongoDB.Driver, JWT Bearer, Swagger, Docker.
