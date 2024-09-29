# Models

Models are what we use to interact with the database. They define the structure of the data we'll be pulling of the database. To create a model, we can use the **C# Extensions** (VS Code extension); just create a `Models` folder, **right-click** on it, and select `New C# > Class`, then give it a filename and boom! profit.

## ORM

We need to install some necessary packages using **NuGet Gallery**, so open the **command palette**, then `NuGet: Add NuGet Package`, and search for `sqlserver`

> [!WARNING]
> Choose a version of the package that matches the version of NET you are using (run `dotnet --version` to find out, or check your `WebApi.csproj` file, the property `TargetFramework`, which in my case was `8.0.8`).

The packages we need are:

- `Microsoft.EntityFrameworkCore.SqlServer`, by Microsoft (just search for `sqlserver`).
- `Microsoft.EntityFrameworkCore.Tools`, by Microsoft (just search for `tools`).
- `Microsoft.EntityFrameworkCore.Design`, by Microsoft (just search for `design`).

With the packages above, we'll be able to create our `ApplicationDBContext` class, which is what we'll inject in our controllers to fetch data from our DB as objects (this class inherits from `DbContext`, which is one of the classes included in `EntityFrameworkCore`)

## Creating a Database

In Azure studio, we can create a database with:

```sql
-- Use master database
USE master;
GO

-- Create TutorialDB database if it does not exist
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'finshark')
BEGIN
    CREATE DATABASE [finshark];
END
GO
```

If you refresh your databases, `finshar` should be there.

## Connection String

We have to add a connection string in our `appsettings.Development.json` file:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost; Database=finshark; User Id=SA; Password=reallyStrongPwd123; TrustServerCertificate=true"
},
```

> [!TIP]
> Check [this site](https://www.connectionstrings.com/sql-server/) for learn about connection strings.

## Migrations

In order to create the **database tables** you need to run 2 commands:

```
dotnet ef migrations add InitialMigration
dotnet ef database update
```

> [!WARNING]
> In order to run the commands above, you need to install the `dotnet-ef` tool, so run `dotnet tool install --global dotnet-ef`.

After the migrations run successfully:

- A new `Migrations` folder will show up in our project.
- Also, if you **refresh** your `finshark` database, you should be able to see the newly created **tables**

## Deleting a Database from Azure Data Studio

When I run the migrations the **1st time** and restarted the app, I received the following warning:

```
warning CS8981: The type name 'init' only contains lower-cased ascii characters. Such names may become reserved for the language.
```

That's because I named my first migration as `dotnet ef migrations add init`. So I decided to **delete** the `Migrations` folder, and the database as well:

```sql
USE master;
GO

ALTER DATABASE finshark SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
GO
DROP DATABASE finshark;
GO
```

Then I ran the migrations again and all was good.


---
[:arrow_backward:][back] ║ [:house:][home] ║ [:arrow_forward:][next]

<!-- navigation -->
[home]: /README.md
[back]: ./controllers.md
[next]: ./dtos.md