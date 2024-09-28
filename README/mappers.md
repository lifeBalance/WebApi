# Mappers

Mappers facilitate the conversion of data between different representations, such as between **DTOs** and **domain entities**.

1. **Entity to DTO**: When we fetch an entity from the database, the mapper extracts the relevant information and converts it into a simpler DTO before sending it to the **client**.
2. **DTO to Entity**: When we receive a DTO (e.g., from a user input form), the mapper takes that DTO and converts it back into a full entity, before storing it in the **database**.

> [!NOTE]
> In many applications, usually there are 2 levels of **models**: 
> 
> 1. **Domain internal models**, which we defined in the `Models` folder.
> 2. **Public models**, which are often called DTOs (data transfer objects).
> 
> DTOs are the models consumed by a **client**, and usually they are tinier than their domain counterparts. Also, a DTO model may contain properties from few domain models.

---
[:arrow_backward:][back] ║ [:house:][home] ║ [:arrow_forward:][next]

<!-- navigation -->
[home]: /README.md
[back]: ./dtos.md
[next]: #