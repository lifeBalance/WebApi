# Stocks Portfolio - Many-To-Many

In our app, users must be able to add/remove different stocks to their portfolios. That's what is called a [many-to-many relationship](https://en.wikipedia.org/wiki/Many-to-many_(data_model)) (a user can have many different stocks, and the same stock can be owned by many users).

In order to implement the **stocks portfolio** feature, we'll be dealing mostly with:

- Models, the `Portfolio`, `AppUser`, and `Stock` classes.
- Database Context


## Models

We gotta do some work here:

1. We'll create a new class(model) named `Portfolio`, from where we control a **portfolio table**, where the join between the **users** and **stocks** tables will be done.
2. Then in our existing `AppUser` model, we have to add a property to link a user with her portfolio. Actually a user can have **multiple portfolios**.
3. The same property should be added to the `Stock` model.

> [!TIP]
> When implementing many-to-many relationships, it's a good idea to use [table attributes](https://learn.microsoft.com/en-us/dotnet/api/system.data.linq.mapping.tableattribute.name?view=netframework-4.8.1) to our models, where we can specify exactly the names we want for our tables.


## Adding the Model to the DB Context

Then we have to add the new `Portfolio` class to the [database context](https://learn.microsoft.com/en-us/aspnet/core/data/ef-mvc/intro?view=aspnetcore-8.0#create-the-database-context), easy peasy:

```cs
public DbSet<Portfolio> Portfolios { get; set; } = null!;
```

There we also have to set the foreign key in `OnModelCreating`:

```cs
modelBuilder.Entity<Portfolio>().HasKey(p => new { p.AppUserId, p.StockId });

modelBuilder.Entity<Portfolio>()
    .HasOne(p => p.AppUser)
    .WithMany(u => u.Portfolios)
    .HasForeignKey(p => p.AppUserId);

modelBuilder.Entity<Portfolio>()
    .HasOne(p => p.Stock)
    .WithMany(u => u.Portfolios)
    .HasForeignKey(p => p.StockId);
```


> [!TIP]
> Read more about [primary and foreign keys](https://learn.microsoft.com/en-us/sql/relational-databases/tables/primary-and-foreign-key-constraints?view=sql-server-ver16).

## Migration

Our current database is not designed to support the code above (we changed the db tables with the attributes), so we have to get rid of the existing migrations (delete the whole `Migrations` folder).


Then also, in **Azure Data Studio** we can go to **Databases**, and nuke the `finshark` db by running the following query:

```sql
USE master;
GO

ALTER DATABASE finshark SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
GO
DROP DATABASE finshark;
GO
```

Then we can create a new migration to recreate our db:
```
dotnet ef migrations add PortfolioManyToMany
dotnet ef database update
```

If we check Azure Data Studio now, we should have a new db, with all our tables. The `portfolio` table should have the `AppUserId` and `StockId` columns.

## Portfolio: CRUD

As to now, we've created a **model** for our **portfolio** as way to implement a **many-to-many** relationship between **users** and **stocks**. A row in our **porfolio table** is basically a way to link users and stocks:

| UserId    | StockId |
| --------- | ------- |
| 23jlk23kj | 1       |
| 23jlk23kj | 2       |
| 6zask12as | 1       |
| 6zask12as | 2       |

So whenever we want to pull from our database a user's portfolio, we'll find all the rows with her `UserId`, each of them linked to a `StockId`. Then, using this `StockId` list, we'll pull the information for each stock.

Here we'll be working on performing [CRUD](https://en.wikipedia.org/wiki/Create,_read,_update_and_delete) operations on our portfolios. In order to do that, we have to:

- Create a `PortfolioController`
- Create a `PortfolioRepository`
- Create a `IPortfolioRepository`
- Create a `ClaimsExtension`

### The Portfolio Controller

In the `PortfolioController` we're injecting:

- The `UserManager` class
- The `IPortfolioRepository` class


### The `GetUserPortfolio` endpoint

In this endpoint we're extracting the user from the **request**, thanks to the following line:

```cs
var username = User.GetUsername();
```

[User](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.controllerbase.user?view=aspnetcore-8.0) is a **property** of `ControllerBase` , which allows us to read the current user from the [HttpContext](https://learn.microsoft.com/en-us/dotnet/api/system.web.httpcontext?view=net-8.0) as a [ClaimsPrincipal](https://learn.microsoft.com/en-us/dotnet/api/system.security.claims.claimsprincipal?view=net-8.0).

> [!NOTE]
> [Claims](https://en.wikipedia.org/wiki/Claims-based_identity) is a common way for applications to acquire the identity information they need about users inside their organization.

Note that we're using a method named `GetUserName()` in our `User` object. That method was written as an [extension method](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods), in the `ClaimsExtensions.cs` file.

> [!TODO]
> Find out why is he using that link in the `GetUserName` extension method 🤔

### A Portfolio Repository and its Interface

Aside from the **controller**, we also need a repo, where we'll define the logic that interact with the db. And of course, we need to add them both to the `Program.cs` as services:

```cs
builder.Services.AddScoped<IPortfolioRepository, PortfolioRepository>();
```

In the repo we'll have access to a property of the database context named `Portfolios`, which was defined in the `ApplicationDBContext.cs` file:
```cs
public DbSet<Portfolio> Portfolios { get; set; } = null!;
```

After doing all this, we can go to Swagger, log in, paste the token in the **Authorize** box, and make a `GET` request to `/api/portfolio` to see our portfolio.

> [!NOTE]
> You may have to create some stock prior to that, and link it to your portfolio in the portfolios table.


---
[:arrow_backward:][back] ║ [:house:][home] ║ [:arrow_forward:][next]

<!-- navigation -->
[home]: /README.md
[back]: ./users.md
[next]: #