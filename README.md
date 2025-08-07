# LifeyLife

A web application for managing life challenges and dares.

## Setup Instructions

1. Clone the repository
2. Copy `appsettings.Example.json` to `appsettings.Development.json` and update the values:
   - Set your database connection string
   - Generate a secure JWT secret key (minimum 16 characters)
   - Update other configuration values as needed

3. Install dependencies:
   ```bash
   # Backend
   dotnet restore
   
   # Frontend
   cd LifeyLife.App/ClientApp
   npm install
   ```

4. Set up the database:
   ```bash
   # Run migrations
   dotnet ef database update
   ```

5. Run the application:
   ```bash
   # Development mode
   ./dev.sh
   ```

## Development

- Backend API runs on https://localhost:5555
- Frontend runs on http://localhost:4200

## Security Notes

- Never commit sensitive configuration files
- Keep your JWT secret key secure
- Use environment variables for production deployments
- Regularly rotate secrets and keys

## Contributing

1. Create a new branch for your feature
2. Make your changes
3. Submit a pull request

## License

[Your License Here]

### Idea

### Idea
ToDo:
 - add authentication
 - add localization

### Tech Overview
This service is a dedicated BFF (backend for frontent) service for the LifeyLife.

This service is built with "Onion Architecture" approach even though it does NOT use DDD.

![Image of Onion Architecture](http://4.bp.blogspot.com/-b4L9u8dyxgU/UDJIxbJt89I/AAAAAAAAAPU/JED0ustIIuM/s320/Overview.png)

This is justified by necessity to switch between data sources as this service evolves. Data sources would be Payroll DB, own database (PostgresSql) and downstream services.

If you are hesitant where to put your code use this cheat sheet:

1. **LifeyLife.App** - hosting and configuration stuff only.
2. **LifeyLife.Api** - This is basically service layer. Put here GRPC/HTTP service implementation, request handlers, mapping to proto, authentication, authorization. _Also put here classes that do data retrieval from other GRC services, but **NOT** their contracts. Contracts should go to Core._
3. **LifeyLife.Core** - business logic, common models, contracts. This project should NOT have any references to **LifeyLife.App**, **LifeyLife.Api**, **LifeyLife.Data**.
4. **LifeyLife.Data** - everything related to data access to PostgresSQL database that belongs to this service. _Do NOT put contracts of the repositories to this project. they should go to Core._ 
