# Intus Code Task - Ervin Tuzlic

This repository contains a Blazor WebAssembly (WASM) project organized into a multi-project solution. The solution is divided into three main projects, each serving a specific purpose in building a robust and maintainable web application.

## Project Structure
### 1. Client
The Client project is responsible for handling the client-side logic and user interface. This includes Blazor components, pages, and any client-specific code. This project interacts with the server/API project to request and display data.

### 2. Shared
The Shared project serves as a centralized location for sharing files between the client and server with Data Transfer Objects (DTOs) and Domain Models. This project helps maintain consistency and avoids duplication of code related to data structures.

### 3. Server/API
The Server project functions as the backend/API of the application. It handles incoming requests from the client, contains the server-side logic, and interacts with the database. The project also includes SQL Server configurations for database connections and services that handle business logic. It utilizes the DTOs defined in the Shared project for data communication with the client.
