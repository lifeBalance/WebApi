# Data Transfer Objects (DTOs)

Whenever we retrieve some **entity** from a database, very frequently we want to trim down the information we send down to the user. In order to do that, we can define a [DTO](https://learn.microsoft.com/en-us/aspnet/web-api/overview/data/using-web-api-with-entity-framework/part-5), which is an [object](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/object-oriented/objects) that defines how the data will be sent over the network.

> [!NOTE]
> **DTOs**, are **simple objects** that carry data between different layers of an application. Typically contain only **properties** and no behavior (no methods).

For organization, we can create a `DTOs` folder, where we'll place a subfolder for each of our **models**. Inside each subfolder, we can create the **DTO class**.

> [!TIP]
> In [Domain Driven Design](https://en.wikipedia.org/wiki/Domain-driven_design) we have two layers:
>
> - **Domain Layer** (Domain Entities): Contains all the core business logic.
> - **Application Layer** (DTOs): Manages data flow and communication.
>
> DTOs are used for trimming down the **entities** from a data store before sending them to the client.

---
[:arrow_backward:][back] ║ [:house:][home] ║ [:arrow_forward:][next]

<!-- navigation -->
[home]: /README.md
[back]: ./controllers.md
[next]: ./mappers.md