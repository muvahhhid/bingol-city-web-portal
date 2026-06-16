# Bingöl City Web Portal

Bingöl City Web Portal is an ASP.NET Core MVC web application created as a portfolio project.
The project presents information about Bingöl, its districts, population data, local culture, and natural attractions.

The application includes a public city portal and a protected admin area for managing district information.

## Live Demo

https://bingol-city-web-portal.onrender.com

Note: The demo is hosted on Render Free, so the first request may take up to a minute if the service was inactive.

## Features

* ASP.NET Core MVC web application
* SQLite database integration
* Admin login with ASP.NET Core Identity
* District management with create, edit, and delete functionality
* Public district overview page
* Population statistics page with chart visualization
* Multi-language support using a language selector
* Responsive Bootstrap-based user interface
* Custom styling for homepage, login page, district cards, and content sections
* Seed data for Bingöl districts
* Local database creation using Entity Framework Core migrations
* Docker support for deployment
* Public deployment on Render

## Technologies Used

* ASP.NET Core MVC
* C#
* Entity Framework Core
* SQLite
* ASP.NET Core Identity
* Bootstrap
* HTML
* CSS
* JavaScript
* Chart.js
* Docker
* Render

## Project Purpose

This project was built to demonstrate practical web development skills with ASP.NET Core MVC.
It focuses on building a structured, database-driven web application with authentication, content management, responsive design, database integration, deployment preparation, and clean project organization.

## Admin Area

The website contains an admin-only area for managing district content.

Admin credentials are not stored directly in the source code. They are configured locally through User Secrets or through environment variables in production.

Required configuration keys:

```text
AdminUser:Email
AdminUser:Password
```

For production hosting, these should be configured as environment variables:

```text
AdminUser__Email
AdminUser__Password
```

## Database

The project uses SQLite for easier local development and portfolio deployment.

The local database file is excluded from GitHub using `.gitignore`.
When the application starts, Entity Framework Core migrations and seed logic can create the required database structure and initial district data.

## Local Setup

Clone the repository:

```bash
git clone https://github.com/muvahhhid/bingol-city-web-portal.git
cd bingol-city-web-portal
```

Set admin credentials using User Secrets:

```bash
dotnet user-secrets init
dotnet user-secrets set "AdminUser:Email" "your-admin-email"
dotnet user-secrets set "AdminUser:Password" "your-strong-password"
```

Restore packages:

```bash
dotnet restore
```

Build the project:

```bash
dotnet build
```

Run the project:

```bash
dotnet run
```

Open the website:

```text
http://localhost:5291
```

## Docker

The project includes Docker support for deployment.

Build the Docker image:

```bash
docker build -t bingol-city-web-portal .
```

Run the container:

```bash
docker run --rm -p 8080:8080 ^
  -e "AdminUser__Email=your-admin-email" ^
  -e "AdminUser__Password=your-strong-password" ^
  bingol-city-web-portal
```

Open the website:

```text
http://localhost:8080
```

## Security Notes

* Admin credentials are not committed to the repository.
* Local SQLite database files are ignored.
* Build output folders are ignored.
* Registration is disabled because the website is designed as a public city portal with admin-only content management.
* Production credentials should be configured through environment variables.

## Author

Created by [muvahhhid](https://github.com/muvahhhid)
