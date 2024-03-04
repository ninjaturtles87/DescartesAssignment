Hello,

For app running, you just need to run DescartesAssignment project. You will find more details in text below.

Best regards, Nikola

App details:

Solution consists of four .NET CORE framework projects:

- DescartesAssignment - ASP.NET Core Web API project This is API project responsible for handling third party requests. It validates received data, processes it and gives back appropriate responses. 

- DataAccess - Class Library. This project is a layer between API and database. For simplification purpose, instead of real database, it consists of a class called Database in which values will be added, updated od get while the app is running.

- DescartesAssignment.UnitTests - xUnit Test Project. This is the project for Unit tests. For mocking Mocq library is used, the version is one before incriminated 4.20 version.

- DescartesAssignment.IntegrationTests - MS Test Project, which is used for integration tests for the app.

Also, Swagger is added for documentation and testing purposes, and it is run on https://localhost:7111/swagger/index.html

For logging purposes I have used Serilog. Logs are stored in a folder named 'Logs' which you can find in project.
