# Todo List Web Application using ASP.NET Core MVC

## Getting Started

### 1. Clone the Repository
   - Clone the project repository from GitHub to your local machine.

### 2. Setup Local Database
   - Ensure you have SQL Server installed locally.
   - Open `appsettings.json` file and ensure the `DefaultConnection` connection string points to your local SQL Server instance.
   - Update the connection string with your SQL Server instance details if necessary.

### 3. Run Migrations
   - Open Package Manager Console in Visual Studio.
   - Ensure that the default project selected in Package Manager Console is the one containing your DbContext (typically the project containing `ApplicationDbContext.cs`).
   - Run the following command to apply migrations and create/update the database schema:
     ```
     Add-Migration MigrationName
     Update-Database
     ```

### 4. Build and Run the Application
   - Build the solution in Visual Studio to ensure all dependencies are resolved.
   - Set the startup project to the web application project (typically named `ToDo`).
   - Press F5 or click on the Run button in Visual Studio to start the application.
   - Alternatively, you can run `dotnet watch run` in the Terminal.
   - Now, the application should be running locally on your machine.

## Project Structure and Code Organization

- **Areas:** Contains the Identity area with pages for user authentication and account management.
- **Data:** Contains the DbContext (`ApplicationDbContext.cs`) for Entity Framework Core and the `ContextSeed.cs` file for seeding initial data into the database. It's used to create default user roles and default admin.
- **Enums:** Contains the `Roles.cs` enum for defining user roles.
- **Models:** Contains the application models such as `ToDos.cs` and `ErrorViewModel.cs`.
- **Controllers:** Contains the MVC controllers responsible for handling requests and generating responses.
- **Views:** Contains the CRUD Razor views for rendering HTML output.
- **wwwroot:** Contains static files such as CSS, JavaScript, and images.

