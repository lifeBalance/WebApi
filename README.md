# WebAPI

In order to [create a Web API with ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-8.0&tabs=visual-studio-code) in **VS Code**, we just have to launch the **command palette**, select `.NET: New Project` and select **ASP.NET Core API**. I chose the following settings:

- Authentication type: **none**
- Configure for **HTTPS**
- ~~Use **controllers**~~
- Enable [Open API](https://www.openapis.org/) support (aka swagger)

## Launching the API

Once the project was generated, after running `dotnet run` we got the [swagger](https://swagger.io/) UI, available at:
```
http://localhost:5028/swagger/index.html
```

We could also see the raw JSON output of the api by visiting:
```
http://localhost:5028/weatherforecast
```

> [!NOTE]
> You can get the last segment of the URL, by checking in the `controllers` folder; there it must be a file named `WeatherForecastController`. The **endpoint** for hitting this controller, it's the controller's name (case insensitive) without the word `Controller`.

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

## Modifying the API routes

Since we generated our API with controllers, each **URL endpoint** can be found inside each controller. On top of each **controller class**, we can find an [attribute](https://learn.microsoft.com/en-us/dotnet/csharp/advanced-topics/reflection-and-attributes/) that determines the **base URL route**. There we can modify the base route:

- From `[Route("[controller]")]` to `[Route("api/[controller]")]`, so that from now on the endpoint would be at `api/weatherforecast`. 

## Need to install

### Database

We'll use [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) as our database, the problem is that is only for **Windows**! To address that problem, we'll use Docker:

1. Pull the image:
```
docker pull mcr.microsoft.com/azure-sql-edge
```

2. Initiate the SQL Server Container with the following command:
```
docker run -e "ACCEPT_EULA=1" -e "MSSQL_SA_PASSWORD=1234" -e "MSSQL_PID=Developer" -e "MSSQL_USER=SA" -p 1433:1433 -d --name=sql mcr.microsoft.com/azure-sql-edge
```

If you prefer to use **Docker Compose**, use the following `docker-compose.yml` file:

```yaml
services:
  sql:
    image: mcr.microsoft.com/azure-sql-edge
    container_name: sql
    environment:
      ACCEPT_EULA: "1"
      MSSQL_SA_PASSWORD: "reallyStrongPwd123"
      MSSQL_PID: "Developer"
      MSSQL_USER: "SA"
    ports:
      - "1433:1433"
    restart: always
```

To run it, just execute `docker-compose up -d` in your terminal.

### Azure Data Studio

As a GUI to SQL server we'll use [Azure Data Studio](https://azure.microsoft.com/en-us/products/data-studio). Just download it, and connect to the SQL runnning on Docker. For the settings used in the `docker-compose.yml` above, we'll use the following in **Azure Data Studio**:

![Azure Data Studio](./README/azure-data-studio.png)

### VS Code Extensions

- [C# Extensions - JosKreativ](https://marketplace.visualstudio.com/items?itemName=kreativ-software.csharpextensions)
- [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=kreativ-software.csharpextensions)