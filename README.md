# WebAPI

In order to [create a Web API with ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-8.0&tabs=visual-studio-code) in **VS Code**, we just have to launch the **command palette**, select `.NET: New Project` and select **ASP.NET Core API**. I chose the following settings:

- Authentication type: **none**
- Configure for **HTTPS**
- Use **controllers**
- Enable [Open API](https://www.openapis.org/) support (aka swagger)

## Launching the API

Once the project was generated, after running `dotnet run` we got the [swagger](https://swagger.io/) UI, available at:
```
http://localhost:5028/swagger/index.html
```

## Enabling HTTPS

Even though we created our project with **HTTPS support**, according to the [docs](https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-8.0&tabs=visual-studio-code), to **enable** serving the content in the HTTPS URL, we have to:

1. Trust the HTTPS development certificate by running the following command:

```
dotnet dev-certs https --trust
```

2. Run the following command to start the app on the https profile:

```
dotnet run --launch-profile https
```

Assuming the app is running at **port 7188**, we can access the endpoint at:

```
https://localhost:7188/weatherforecast
```

And the **swagger** interface at:

```
https://localhost:7188/swagger/index.html
```