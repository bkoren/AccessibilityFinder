<div align="center">

# AccessibilityFinder

**A directory of activities filtered by real-world accessibility, with community reviews — for people with disabilities.**

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4)
![C#](https://img.shields.io/badge/C%23-11-239120)
![SQL Server](https://img.shields.io/badge/SQL%20Server-2019%2B-CC2927)
![License](https://img.shields.io/badge/license-MIT-blue)

</div>

> **Academic project.** Built as the capstone for the *Razvoj web aplikacija* (Web Application Development) course at Algebra University. Not deployed — clone and run locally (see [Getting Started](#getting-started)).

---

## About

AccessibleActivities helps people with disabilities find activities — accommodation, restaurants, landmarks, and more — that fit their access needs. Administrators catalog activities by type and tag each one with concrete accessibility features (transport options, specialized guides, and so on); users add reviews and accessibility ratings. Accessibility is treated as a first-class, searchable dimension rather than an afterthought.

The point of the build was to implement a full multi-tier ASP.NET Core solution end to end: a JWT-secured REST API and an MVC web app sharing a business layer over a SQL Server database.

## Screenshots

<!-- Replace these with real captures — a Swagger screenshot and one MVC page go a long way here. -->
| Swagger (API) | Activity list (MVC) | Details + reviews |
| ------------- | ------------------- | ----------------- |
| ![Swagger](screenshots\swagger.png) | ![List](screenshots\activity-list.png) | ![Details](screenshots\activity-details.png) |

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

## Getting Started

### Prerequisites

```bash
.NET SDK 8.0
SQL Server 2019+ (or SQL Server Express / LocalDB)
Visual Studio 2022 (or the dotnet CLI)
```

### Installation

```bash
# 1. Clone the repo
git clone https://github.com/{{USER}}/{{REPO}}.git
cd {{REPO}}

# 2. Create the database
#    Run Database/Database.sql against your SQL Server instance
#    (database-first — there are no EF migrations)

# 3. Set your connection string and JWT settings in appsettings.json (see Configuration)

# 4. Restore and build
dotnet restore
dotnet build
```

## Usage

Run the two projects from the solution.

```bash
# RESTful service (Web API) — serves data + Swagger UI
dotnet run --project WebAPI

# Web application (MVC) — landing, browse, details, admin
dotnet run --project WebApp
```

- **API + Swagger:** `https://localhost:<port>/swagger` — CRUD, search, paging, and the JWT auth flow.
- **Web app:** `https://localhost:<port>` — public landing page, user registration/login, activity browsing and details, and the role-gated admin area.

## Configuration

Settings are read from `appsettings.json` (connection strings are **not** hardcoded). Provide:

| Key                                    | Description                        |
| -------------------------------------- | ---------------------------------- |
| `ConnectionStrings:DefaultConnection`  | SQL Server connection string       |
| `Jwt:Key`                              | Secret key used to sign JWT tokens |
| `Jwt:Issuer`                           | Token issuer                       |
| `Jwt:Audience`                         | Token audience                     |


## License

Distributed under the MIT License. See `LICENSE` for details.

