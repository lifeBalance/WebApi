# Controllers

Since we generated our API with controllers, each **URL endpoint** can be found inside each controller. On top of each **controller class**, we can find an [attribute](https://learn.microsoft.com/en-us/dotnet/csharp/advanced-topics/reflection-and-attributes/) that determines the **base URL route**. There we can modify the base route:

- From `[Route("[controller]")]` to `[Route("api/[controller]")]`, so that from now on the endpoint would be at `api/weatherforecast`. 

> [!NOTE]
> You can go ahead and delete the `WeatherForecast.cs` file, we will be writing our own controllers.

## The `ControllerBase` class

**Web API** controllers should typically derive from [ControllerBase](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.controllerbase) rather from [Controller](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.controller):

>[!NOTE]
> `Controller` derives from `ControllerBase` and adds support for **views**, so it's for handling web pages, not web API requests.

## Injecting a Database Context


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

---
[:arrow_backward:][back] ║ [:house:][home] ║ [:arrow_forward:][next]

<!-- navigation -->
[home]: /README.md
[back]: ./models.md
[next]: ./dtos.md