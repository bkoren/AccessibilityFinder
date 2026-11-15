<div align="center">

# AccessibilityFinder

**Find activities, restaurants and accommodation that are accessible to people with disabilities.**

[![Build and deploy](https://github.com/bkoren/AccessibilityFinder/actions/workflows/main_accessibility-finder.yml/badge.svg)](https://github.com/bkoren/AccessibilityFinder/actions/workflows/main_accessibility-finder.yml)
![.NET](https://img.shields.io/badge/.NET-8.0-512BD4)
![C#](https://img.shields.io/badge/C%23-12-239120)
![SQL Server](https://img.shields.io/badge/Azure%20SQL-SQL%20Server-CC2927)
![Azure](https://img.shields.io/badge/hosted%20on-Azure%20App%20Service-0078D4)

### [🌐 Live demo](https://accessibility-finder-web-ctejd5c0duh5b7cu.italynorth-01.azurewebsites.net) · [📘 API (Swagger)](https://accessibility-finder-h0fdfjhta7ckgjbx.italynorth-01.azurewebsites.net/swagger)

</div>

> [!NOTE]
> **This is a demo project.** It was built to demonstrate full-stack .NET development skills, and it is also used as a target for security testing. The data is fictional, and the [Vulnerability Report](#vulnerability-report) below lists known weaknesses on purpose. Do not use it for real accessibility information.

> The apps run on Azure's free tier, which sleeps when idle. The first visit can take 30–60 seconds to wake up.

## About

A web application that helps people with disabilities find places that fit their access needs. Administrators catalog activities by type and tag each one with concrete accessibility features (step-free access, transport options, specialized guides, and so on). Registered users search and filter activities and leave reviews with ratings.

Started as a second-year project for the course *Web Application Development* at Algebra University, Zagreb, then extended with cloud deployment, CI/CD, integration tests, and a security review.

## Features

**Users**
- Register, log in, update profile, change password
- Search activities by name, filter by type and accessibility features, with server-side paging
- View activity details and add reviews with a 1–5 star rating

**Administrators**
- Create, update and delete activities, types and accessibility features
- Manage users and moderate reviews
- View the action log of the whole system

## What This Project Demonstrates

- **REST API** with CRUD, search and server-side paging, and meaningful HTTP status codes.
- **JWT authentication** with salted password hashing and role-based authorization (`Admin` / `User`).
- **Multi-tier design:** WebAPI owns the database, WebApp calls it over HTTP through a proxy controller, and shared DTOs live in `Common`. AutoMapper maps between tiers.
- **Integration tests** that boot the real API in memory (see [Tests](#-tests)).
- **CI/CD** with GitHub Actions: build, test and deploy both apps to Azure on every push.
- **Secrets management:** no secrets in the repo (User Secrets locally, environment variables on Azure).

## Tech Stack

**Backend:** .NET 8 · ASP.NET Core Web API · Entity Framework Core · JWT Bearer · AutoMapper · Swagger / OpenAPI
**Frontend:** ASP.NET Core MVC · Razor · Bootstrap · vanilla JavaScript (fetch)
**Data:** SQL Server / Azure SQL (database-first)
**Testing:** xUnit · Microsoft.AspNetCore.Mvc.Testing · EF Core InMemory
**DevOps:** GitHub Actions · Azure App Service

## 🧪 Tests

**13 integration tests** · xUnit · `WebApplicationFactory` · EF Core InMemory

Each test starts the real WebAPI in memory, replaces SQL Server with an in-memory database, and sends real HTTP requests.

| Area | What is tested |
|:--|:--|
| 🔐 **Auth** | Login, registration, and role access (401 / 403 / 200) |
| 📍 **Activities** | Adding and listing activities |
| 🏷️ **Types** | Adding and listing types |
| ♿ **Accessibility** | Adding and listing accessibility features |
| ⭐ **Reviews** | Adding a review and reading it back |

```bash
dotnet test
```

> [!TIP]
> The tests also run in the CI pipeline. If any test fails, the deploy stops.

## Vulnerability Report

Found a security issue? Reports are welcome. This project is also used for security testing practice, so every finding helps.

Please report it privately through the repository’s Security tab → Report a vulnerability. Include the endpoint or page, the steps to reproduce, and what an attacker could do.

## Running Locally

### Prerequisites

- .NET SDK 8.0
- SQL Server 2019+ (or SQL Server Express / LocalDB)
- Visual Studio 2022 or the `dotnet` CLI

### 1. Clone

```bash
git clone https://github.com/bkoren/AccessibilityFinder.git
cd AccessibilityFinder
```

### 2. Create the database

```bash
sqlcmd -S localhost -E -b -i "Database/database.sql"
```

For a named instance, replace `localhost` with `.\SQLEXPRESS`.

### 3. Set the secrets

Both apps need the **same** JWT key (at least 32 bytes). Use User Secrets so nothing secret is committed:

```bash
dotnet user-secrets set "Jwt:SecureKey" "<your-long-random-key>" --project WebAPI
dotnet user-secrets set "Jwt:SecureKey" "<your-long-random-key>" --project WebApp
```

The local connection string (`WebAPI/appsettings.Development.json`) and the API address (`WebApp/appsettings.Development.json` → `ApiBaseUrl`) are already set for a default local SQL Server.

### 4. Run both apps

Each command blocks its terminal, so open two terminals.

```bash
dotnet dev-certs https --trust     # first time only
dotnet run --project WebAPI        # terminal 1
dotnet run --project WebApp        # terminal 2
```

| App           | URL                            |
|---------------|--------------------------------|
| API & Swagger | https://localhost:7263/swagger |
| Web app       | https://localhost:7208         |

## Deployment

Pushing to `main` triggers `.github/workflows/main_accessibility-finder.yml`, which:

1. Builds the solution and runs the integration tests.
2. Publishes WebAPI and WebApp.
3. Deploys each app to its own Azure App Service.

Configuration on Azure comes from App Service environment variables: `Jwt__SecureKey` (both apps), `ConnectionStrings__DefaultConnection` (WebAPI) and `ApiBaseUrl` (WebApp).
