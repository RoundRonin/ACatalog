# ACatalog 
## Overview
ACatalog is an API designed to manage stores, products and stores' inventories. Developed using ASP.NETCore and Entity Framework. Layered architecture pattern is used, separation of concerns is provided which leads to maintainability, and scalability.

## Project Structure
The project is organized into several layers:

1. API Layer: Contains the controllers and is responsible for handling HTTP requests and responses. Validation happens on this level. ViewModels -- are the models here.
2. Business Logic Layer (BLL): Contains the services responsible for the business logic. DTOs -- are the models here.
3. Data Access Layer (DAL): Contains the repositories responsible for data access. Entities -- are the models here. They Contain the data models representing the database entities.

## Key Patterns
1. Repository Pattern: Abstracts the data layer, providing a clear separation between data access and business logic.
2. Dependency Injection: Ensures loose coupling and enhances testability. Provided by Microsoft.
3. Data Mapper is used for the database, provided by Microsoft with Entity Framework.
4. Anemic domain model is used to manage business logic. DTOs represent the real-life structure and services provide the functionality.
5. Async/Await: Utilized throughout to ensure non-blocking operations.


## Usage
Prerequisites
.NET SDK installed
Docker for PostgreSQL database (if using the database repository)

### Clone the repository:
```sh
git clone https://github.com/RoundRonin/ArtcleCatalog.git
cd ArticleCatalog
```

### Install Dependencies:
```sh
dotnet restore
```


### Set Up the Database
If you are using a database, you need to follow these steps. Otherwise -- skip.

Run PostgreSQL in Docker unisng docker compose.

For safety concerns, credentials for the DB are stored in .env
Though for testing purpose it is included in the repo.

To start:
```sh
docker-compose up -d
```

To stop:
```sh
docker-compose down
```

Run Migrations:
```sh
cd DAL
dotnet ef migrations add InitialCreate --startup-project ..\ArtcleCatalog\
dotnet ef database update --startup-project ..\ArtcleCatalog\
```
### Additional configuration
Configure File Paths: Update appsettings.json:

Use "File" or "Database" to indicate your preferred way of storing data.
```json
{
    "RepositoryType": "File",
    "StoreFilePath": "./files/store.csv",
    "ProductFilePath": "./files/product.csv",
    "InventoryFilePath": "./files/inventory.csv"
}
```

### Run the Application
```sh
dotnet run
```

The API documentation is auto-generated using Swagger and can be accessed at the following endpoint:
```
http://localhost:5000/swagger
```
## NuGet Packages Used
1. Microsoft.EntityFrameworkCore: Core EF package.
2. Microsoft.EntityFrameworkCore.Design: Design-time tools for EF.
3. Microsoft.EntityFrameworkCore.Tools: Command-line tools for EF.
4. Swashbuckle.AspNetCore: Swagger for API documentation.
5. DotNetEnv

And some other packages for testing
1. xUnit 
2. dotnet add package Microsoft.EntityFrameworkCore.InMemory


## Development Environment
c
This project was developed using Visual Studio, git.

## Contribution
Feel free to not contirbute at all!
