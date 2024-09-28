# WebAPI

## POST: Writing to the Database

Whenever we have to interact with the **database**, we'll be using [Entity Framework](https://learn.microsoft.com/en-us/ef/), by far the most popular **ORM** for [.NET](https://dotnet.microsoft.com/en-us/).

> [!NOTE]
> **Entity Framework** is a modern **object-relation mapper** that lets you build a high-level **data access layer** across a variety of databases, including **SQL Databases** (SQLite, MySQL, PostgreSQL), and **Azure Cosmos DB**. It supports [LINQ](https://learn.microsoft.com/en-us/dotnet/csharp/linq/) queries, change tracking, updates, and schema migrations.

When writing to the database, we need to use a couple of **EF** methods:

- One for tracking the data we want to write: `_context.Movies.Add()`
- Another one for actually saving the **tracked data** to the database: `_context.Movies.SaveChanges()`

## Repository Pattern

So far, our **controller methods** include logic that interacts directly with the **database**, via the **Entity Framework**. But what if in the future we decided to replace **EF** for another **ORM** or even fetch the domain objects from a different **API**.

> [!NOTE]
> The four layers in typical **DDD** implementations are:
> 
> - Domain Layer
> - Application Layer
> - Infrastructure Layer
> - User Interface (Presentation) Layer

The idea behind this pattern is to create some classes to encapsulate the logic that deals with data persistence concerns. That way, our controllers interact with the **domain objects** through objects (**repositories**) of this new **abstraction layer**.

> [!NOTE]
> Using this pattern we won't have to inject the **database context** into the **controller constructors**.

To implement our **repositories** we'll use:

- [C# interfaces](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/interfaces), which we'll store under a folder named `Interfaces`. Each **interface repository name** will follow the convention:

```
I + repositoryName + Repository
```

For example, our repository for stocks will be called `IStockRepository`, and the one for comments `ICommentRepository`.

- C# classes, which we'll keep under the `Repository` folder.

Once all is done, we need to wire them up in `Program.cs`:

```c#
builder.Services.AddScoped<IStockRepository, StockRepository>();
```

> [!WARNING]
> Remember to **restart** your app after the changes; the `watch` can't handle the heat!

## Handling JSON

For handling JSON in our app, we need to install 2 packages:

- Newtonsoft.Json
- Microsoft.AspNetCore.Mvc.NewtonsoftJson

> We can do that from **NuGet Gallery**.

Then we have to add the following lines to our `Program.cs` file:
```cs
builder.Services.AddControllers()
.AddNewtonJson(options => {
  options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});
```

## Data Validation

Add your data validation:

- In the **route attributes**, `[Route("{id:int}")]`. That's enough in `GET` requests for an item with some id.
- For validating data incoming in the *request body**, we'll use the [DataAnotations](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations?view=net-8.0) namespace.

Data anotations look like:
```cs
[Required]
[MinLength(5, ErrorMessage = "Title must be at least 5 characters long")]
[MaxLength(250, ErrorMessage = "Title must be at most 250 characters long")]
public string Title { get; set; } = string.Empty;
```

You should put them on top of the methods of your DTOs, not the models.

Then we have to add the following lines in our **controller actions**:
```cs
if(!ModelState.IsValid)
  return BadRequest(ModelState);
```

This `ModelState` comes included in the `ControllerBase`.

## Filtering Data

We're writing all our **database access logic** in the `repositories` folder; there we files which contain classes for dealing with our database. In these classes we're injecting our **database context**, and calling methods through it.

> [!NOTE]
> The **database context** has **getter/setter** methods for each of the **agregate roots** or **entities** of our **domain model**.

So far we've been using [LINQ](https://learn.microsoft.com/en-us/dotnet/csharp/linq/) queries to interact with the **Entity Framework** in an **object oriented** way (`ToList`, `FindAsync`, `FirstOrDefaultAsync`, etc). These methods usually return [IEnumerable](https://learn.microsoft.com/en-us/dotnet/api/system.collections.ienumerable?view=net-8.0), which are collections of data.

> [!NOTE]
> These methods generate **SQL**, which is what SQL databases understand.

Sometimes we need to write more elaborated queries to **filter** our data collection, according to some criteria. For these scenarios, we have to use [AsQueryable](https://learn.microsoft.com/en-us/dotnet/api/system.linq.queryable.asqueryable?view=net-8.0) to convert the `IEnumerable` to [IQueryable](https://learn.microsoft.com/en-us/dotnet/api/system.linq.iqueryable?view=net-8.0):

```cs
var stocks = _context.Stocks.AsQueryable();
```

Once we have an `IQueryable`, we can do [lots of stuff](https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/ef/language-reference/supported-and-unsupported-linq-methods-linq-to-entities) with it:

- Filter according to some predicate using [WHERE](https://learn.microsoft.com/en-us/dotnet/api/system.linq.queryable.where?view=net-8.0#system-linq-queryable-where-1(system-linq-iqueryable((-0))-system-linq-expressions-expression((system-func((-0-system-boolean)))))):

```cs
stocks.Where(s => s.Symbol == symbol);
```

- Limit:

```cs
stocks.Limit(3);
```

## Passing the Filter Criteria to the Repository Method

The user-supplied values to filter queries must be passed in the controller, to whatever **repository** method we want to filter, for example:

```cs
var stocks = await _stockRepo.GetAllAsync(query);
```

## Editing our Repository and Repository Interface

Then, we need to modify our repository and its interface, so that the method now accepts the query.

## Sorting

For sorting, we need to have a new conditional statement to check if the user added some **sorting** criteria:

```cs
if (!string.IsNullOrWhiteSpace(queryObject.SortBy))
{
    if (queryObject.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
    {
        stocks = queryObject.IsSortDescending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
    }
}
```

## Pagination

[Pagination](https://learn.microsoft.com/en-us/ef/core/querying/pagination) is done by combining the `Skip` and `Take` methods:

- `Skip` is for skipping records.
- `Take` is for, you guessed it, taking records.

```cs
// Pagination
// If page number is 1, we skip 0 records(because 1 - 1 = 0).
// If page number is 2, we skip 1 * pageSize records.
var skipNumber = (queryObject.PageNumber - 1) * queryObject.PageSize;

return await stocks.Skip(skipNumber).Take(queryObject.PageSize).ToListAsync();
```

## Query String Helpers

A nice little technique to deal with long query strings, is to create a `QueryObject` class (we can store it in a `Helpers` folder), and add properties to each of the **key/values** of our [query string](https://en.wikipedia.org/wiki/Query_string).

---
[:arrow_backward:][back] ║ [:house:][home] ║ [:arrow_forward:][next]

<!-- navigation -->
[home]: #
[back]: #
[next]: ./README/tools.md