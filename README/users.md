# Users

Here we'll see how to:

- User authentication
- User creation
- JWT generation

## Authentication - JWT (JSON Web Tokens)

[JWT](https://jwt.io/) is an open standard for securely transmitting information between parties as a JSON object. Basically, when the user successfully **authenticates**, it's given a **JWT** which will be included in all subsequent API calls in order to be able to access to the API resources.

In order to use JWT in our **ASP.NET Core** application we need to install the following packages:

- [Microsoft.Extensions.Identity.Core](https://www.nuget.org/packages/Microsoft.Extensions.Identity.Core)
- [Microsoft.AspNetCore.Identity.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.AspNetCore.Identity.EntityFrameworkCore)
- [Microsoft.AspNetCore.Authentication.JwtBearer](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.JwtBearer)

Which we can install with:
```
dotnet add package Microsoft.Extensions.Identity.Core
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
```

### Adding a User Model

The `Microsoft.AspNetCore.Identity` namespace gives us access to an `IdentityUser` class that comes with some **default properties**:

- Email
- Password
- Password confirmation

We'll create an `AppUser` model which inherits from `IdentityUser`, and expand the properties mentioned above.

### Inherit from `IdentityDbContext`

Now we have to modify our `ApplicationDBContext` so that now it will inherit from `IdentityDbContext` (instead of just `DbContext`), which is a class also included in `EntityFrameworkCore`. We need to pass into it the `AppUser` model:

```cs
public class ApplicationDBContext : IdentityDbContext<AppUser>
```

### Wire Up in Program.cs

Just add the lines:

```cs
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = true;
})
.AddEntityFrameworkStores<ApplicationDBContext>();
```

For the **JWT** `Issuer`, `Audience` and `SigningKey`, we need to add it to our `appsettings.json`:
```json
"AllowedHosts": "*",
"JWT": {
  "Issuer": "http://localhost:5028",
  "Audience": "http://localhost:5028",
  "SigningKey": "supersecretword"
}
```

And then we can use them in our `Program.cs`:
```cs
builder.Services.AddAuthentication(options => {
    options.DefaultScheme = 
    options.DefaultAuthenticateScheme = 
    options.DefaultChallengeScheme = 
    options.DefaultSignInScheme = 
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"], // add values from appsettings.json
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
        ),
    }; 
});
```

Finally, we have to add the lines:
```cs
app.UseAuthentication();
app.UseAuthorization();
```

### Migrations

We need to run:
```
dotnet ef migrations add IdentityMigration
dotnet ef database update
```

Check your DB tables to see the new users stuff.

## Creating Users (Sign up - Register)

Adding logic for creating users is a little bit involved, especially because we'll be adding **user roles**. These are the steps we have to follow:

- **Controller**: We need a controller to receive the request and create a **user** with a **role** (check the `AccountController.cs`).
- **DTO**: We'll define a DTO for the request body used in the controller (check the `RegisterDto.cs`).
- Customize the [model](https://learn.microsoft.com/en-us/ef/core/modeling/) by overriding the `OnModelCreating` method in our database context (check `ApplicationDBContext.cs`).
- We have to run a **migration** to add the **roles** for `User` and `Admin` to the `AspNetRoles` table.

> [!NOTE]
> EF Core uses a **metadata model** to describe how the application's entity types are mapped to the underlying database.

The `Microsoft.AspNetCore.Identity` namespace includes a class named [UserManager](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.usermanager-1?view=aspnetcore-8.0), which provides **methods** for, well, managing users.

### Account Controller

1. Here we start by injecting the [UserManager](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.usermanager-1?view=aspnetcore-8.0) class into our controller.
2. Then create an **action** to handle the `POST` request to the `/api/account/register` endpoint. Here we have to:
  - **Validate** the data incoming in the **request body**, so that it matches the **data type** used in the action parameters (`RegisterDto`). We do that using [model binding](https://learn.microsoft.com/en-us/aspnet/core/mvc/models/model-binding) which does all the validation work for us. We just have to use:
  ```cs
  if (!ModelState.IsValid)
  {
      return BadRequest(ModelState);
  }
  ```
  - Then we just create both the **user** and the **role** by calling methods on `_userManager`.

### Migrations

We need to run:
```
dotnet ef migrations add SeedRolesMigration
dotnet ef database update
```

Check your `AspNetRoles` table to check the roles are there.

## Token Service

The `Identity` packages we installed at the beginning allow us to **validate JWTs** but we need to be able to generate them.

> [!NOTE]
> There are a lot of good 3rd party providers of JWT **authentication**. 

> [!NOTE]
> Regarding **authorization** there are two main approaches: **roles** and **claims**. The first ones need **DB** access, while the second ones are like labels included in the **payload** of our JWTs.

We need to do several things here:

- Create an **interface** for our service (check `ITokenService.cs`). This interface contains the methods our service will have to implement.
- Create the **service** where we'll define:

  - A list of claims
  - The algorithm used for signing the token
  - The token **payload** (which contains the claims)

Then we can refactor our `AccountController` so that when a user is successfully created, we return our JWT (actually we return a `NewUserDto` that contains the token, username, and email)
