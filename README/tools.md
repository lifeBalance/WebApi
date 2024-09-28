# Tools

Since I'm on macOS, I won't be using  **Visual Studio** as my IDE probably the most recommended option for developing in .NET. Instead, I'll be working with **VS Code**

## VS Code Extensions

To make things smoother, we'll install a couple of **VS Code extensions**:

- [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=kreativ-software.csharpextensions)
- [C# Extensions - JosKreativ](https://marketplace.visualstudio.com/items?itemName=kreativ-software.csharpextensions)

## Database

To get our API going, we're gonna need to install a few things:

- A **Relational Database Management System** (RDBMs), we'll go with SQL Server.
### Database
- A **GUI** to interact with SQL server, in this case [Azure Data Studio](https://azure.microsoft.com/en-us/products/data-studio) ([DataGrip]() is cooler but costs money)

### SQL Server

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

---
[:arrow_backward:][back] ║ [:house:][home] ║ [:arrow_forward:][next]

<!-- navigation -->
[home]: /README.md
[back]: /README.md
[next]: ./project.md