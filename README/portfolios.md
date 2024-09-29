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

---
[:arrow_backward:][back] ║ [:house:][home] ║ [:arrow_forward:][next]

<!-- navigation -->
[home]: /README.md
[back]: ./users.md
[next]: #