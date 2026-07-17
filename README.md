<div align="center">

# AccessibilityFinder

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4)
![C#](https://img.shields.io/badge/C%23-11-239120)
![SQL Server](https://img.shields.io/badge/SQL%20Server-2019%2B-CC2927)
![License](https://img.shields.io/badge/license-MIT-blue)

</div>

> **Academic project.** Built as the capstone for the Web Application Development course at Algebra University. Not deployed — clone and run locally (see [Getting Started](#getting-started)).

---

## About

Demonstration of application that helps people with disabilities find activities — accommodation, restaurants, landmarks, and more — that fit their access needs. Administrators catalog activities by type and tag each one with concrete accessibility features (transport options, specialized guides, and so on); users add reviews and accessibility ratings. Accessibility is treated as a first-class, searchable dimension rather than an afterthought.

The point of the build was to implement a full multi-tier ASP.NET Core solution end to end: a JWT-secured REST API and an MVC web app sharing a business layer over a SQL Server database.

## Screenshots

| Swagger (API) | Activity list (MVC) | Details + reviews |
| ------------- | ------------------- | ----------------- |
| ![Swagger](screenshots/swagger.png) | ![List](screenshots/activity-list.png) | ![Details](screenshots/activity-details.png) |

## What This Project Demonstrates

- **RESTful API design** — CRUD, dedicated search endpoint, and server-side paging for the primary entity, with meaningful HTTP status codes (400 / 404 / 500) and error handling.
- **Authentication & authorization** — JWT issuing and validation (`register` / `login` / `change password`), role-based access separating admin and user capabilities.
- **Request logging** — every CRUD action is logged with structured entries, exposed through dedicated log endpoints and consumed by static, token-authenticated pages (JWT held in `localStorage`).
- **Relational data modelling** — a 1-to-N relationship (activity type) and an M-to-N relationship (accessibility features, via a bridge table), plus a user-action bridge for reviews/ratings.
- **Server-side validation** — required fields, format checks (URLs, emails), and duplicate-name prevention via model annotations.
- **Multi-tier architecture** — separate model sets per tier with AutoMapper handling the mapping, so database models never leak into views.
- **Database-first with SQL Server** — schema and seed data live in a single SQL script; connection strings are read from configuration, never hardcoded.

## Tech Stack

.NET 8 · ASP.NET Core (Web API + MVC) · C# · SQL Server (database-first) · JWT · Swagger / OpenAPI · AutoMapper · Razor + Bootstrap · vanilla JS / AJAX

## Project Structure

```
AccessibleFinder/
├── Database/
│   └── Database.sql              # database-first schema + seed data (single script)
└── AccessibleManager/
    ├── AccessibleManager.sln
    ├── WebAPI/                   # RESTful service — JWT-secured, Swagger UI
    ├── WebApp/                   # MVC web application
    └── ...                       # shared business + data-access layers (multi-tier)
```

## Getting Started`

### Installation

## Prerequisites

- .NET SDK 8.0
- SQL Server 2019+ (or SQL Server Express / LocalDB)
- Visual Studio 2022 (or the dotnet CLI)

## Setup

**1. Clone the repository**

```bash
git clone https://github.com/bkoren/AccessibilityFinder.git
cd AccessibilityFinder
```

**2. Create the database**

```bash
sqlcmd -S localhost -E -b -i "Database/database.sql"
```

If your instance is named, replace `localhost` with `.\SQLEXPRESS`.

**3. Trust the local HTTPS certificate** (first time only)

```bash
dotnet dev-certs https --trust
```

**4. Run both projects**

Each command blocks its terminal, so **open two terminal windows**.

Terminal 1 — API:

```bash
cd AccessibilityManager
dotnet run --project WebAPI
```

Terminal 2 — web app:

```bash
cd AccessibilityManager
dotnet run --project WebApp
```

## Access

| App           | URL                            |
|---------------|--------------------------------|
| API & Swagger | https://localhost:7263/swagger |
| MVC web app   | https://localhost:7208         |

> **Note:** both projects must be running. The web app calls the API — if only one is up, pages will load but data will not.

## License

Distributed under the MIT License. See `LICENSE` for details.

