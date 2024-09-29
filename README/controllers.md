# Controllers

Since we generated our API with controllers, each **URL endpoint** can be found inside each controller. On top of each **controller class**, we can find an [attribute](https://learn.microsoft.com/en-us/dotnet/csharp/advanced-topics/reflection-and-attributes/) that determines the **base URL route**. There we can modify the base route:

- From `[Route("[controller]")]` to `[Route("api/[controller]")]`, so that from now on the endpoint would be at `api/weatherforecast`. 

> [!NOTE]
> You can go ahead and delete the `WeatherForecast.cs` file, we will be writing our own controllers.

## The `ControllerBase` class

**Web API** controllers should typically derive from [ControllerBase](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.controllerbase) rather from [Controller](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.controller):

>[!NOTE]
> `Controller` derives from `ControllerBase` and adds support for **views**, so it's for handling web pages, not web API requests.

## Injecting a Database Context [⛔️ BAD PRACTICE ⛔️]

> [!TIP]
> Using the **repository pattern** we won't have to inject the **database context** into the **controller constructors**.

If our **controller** is gonna interact with a **database**, we must [inject](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-8.0) an **instance** of the [DbContext](https://learn.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.dbcontext?view=efcore-8.0) class. We do that in the **constructor**:

```c#
private readonly ApplicationDBContext _context;

public StockController(ApplicationDBContext context)
{
    _context = context;
}
```

> [!TIP]
> That can also be done in a [primary constructor](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/instance-constructors#primary-constructors)
>
> ```c#
> public class StockController(ApplicationDBContext context) : ControllerBase
> {
>     // Hold an instance of ApplicationDBContext
>     private readonly ApplicationDBContext _context = context;
>     ...
> }
> ```

### Attribute Routing

[Attribute routing](https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/routing?view=aspnetcore-8.0#attribute-routing-for-rest-apis) consists on using [C# attributes](https://learn.microsoft.com/en-us/dotnet/csharp/advanced-topics/reflection-and-attributes/) to easily map our [controller actions](https://learn.microsoft.com/en-us/aspnet/mvc/overview/older-versions-1/controllers-and-routing/creating-an-action-cs):

- To **endpoints** in our API (URLs), for example, `[Route("api/books")]`.
- To **HTTP verbs** in our API, for example, `[HttpGet]` or `[HttpPost]`.

Often, the **routes** in a **controller** all start with the same **prefix**, such as `api/books` or `api/users`. In that case we can use the `[RoutePrefix]` attribute, at the **class level**.

> [!TIP]
> Use a tilde (`~`) on the method attribute to override the route prefix at a **method level**.

🦊 Read more about [attribute routing](https://learn.microsoft.com/en-us/aspnet/web-api/overview/web-api-routing-and-actions/attribute-routing-in-web-api-2). 🦊 

### ApiController attribute

The [ApiController attribute](https://learn.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-8.0#apicontroller-attribute) can be applied to a **controller class** to enable the following opinionated, **API-specific** behaviors:

- Attribute routing requirement
- Automatic HTTP 400 responses
- Binding source parameter inference
- Multipart/form-data request inference
- Problem details for error status codes

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

> [!WARNING]
> In **ASP.NET Core 8** by **default**, the `System.Text.Json` library is part of the .NET runtime, so you can use it out of the box without needing additional dependencies. `System.Text.Json` is is a high-performance, lightweight library and the default JSON serializer and deserializer for ASP.NET Core applications.
>
> You still may need the `NewtonsoftJson` library if your app require advance functionality or if you're dealing with a **legacy app** that uses it.


## Query String Helpers

A nice little technique to deal with long query strings, is to create a `QueryObject` class (we can store it in a `Helpers` folder), and add properties to each of the **key/values** of our [query string](https://en.wikipedia.org/wiki/Query_string).

---
[:arrow_backward:][back] ║ [:house:][home] ║ [:arrow_forward:][next]

<!-- navigation -->
[home]: /README.md
[back]: ./project.md
[next]: ./models.md