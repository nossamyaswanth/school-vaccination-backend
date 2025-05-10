# School Vaccination Portal API

The **School Vaccination Portal API** is a backend application designed to manage vaccination drives, student vaccination records, and generate reports. It provides endpoints for authentication, managing students, vaccination drives, and generating vaccination reports.

---

## Table of Contents

- [Features](#features)
- [Technologies Used](#technologies-used)
- [Prerequisites](#prerequisites)
- [Setup Instructions](#setup-instructions)
- [Environment Variables](#environment-variables)
- [Running the Application](#running-the-application)
- [API Endpoints](#api-endpoints)
- [Database Schema](#database-schema)

---

## Features

- **Authentication**: JWT-based authentication for secure access.
- **Student Management**: CRUD operations for student records.
- **Vaccination Drives**: Schedule and manage vaccination drives.
- **Reports**: Generate vaccination summaries and export details to Excel.
- **File Uploads**: Upload and download vaccination certificates.
- **Excel Import**: Bulk import student data from Excel files.

---

## Technologies Used

- **.NET 9.0**: Backend framework.
- **Entity Framework Core**: ORM for database operations.
- **SQL Server**: Database.
- **JWT Authentication**: Secure API access.
- **Swagger/OpenAPI**: API documentation.
- **EPPlus**: Excel file generation.
- **ExcelDataReader**: Excel file parsing.

---

## Prerequisites

Ensure you have the following installed on your system:

- [.NET SDK 9.0](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Visual Studio Code](https://code.visualstudio.com/) or any IDE of your choice
- [Postman](https://www.postman.com/) or any API testing tool (optional)

---

## Setup Instructions

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/nossamyaswanth/school-vaccination-backend.git
   cd school-vaccination-portal-api/SchoolVaccination.API
   ```
2. **Restore Dependencies**:
   ```bash
   dotnet restore
   ```
3. **Set Up the Database**:
   - Update the connection string in appsettings.json:
   ```bash
   "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=schoolvaccination.db;User Id=sa;Password=YourPassword;TrustServerCertificate=True;"
    }
   ```
   - Apply migrations:
   ```bash
   dotnet ef database update
   ```
4.**Seed the Database**: 
  - The database will be seeded automatically when the application starts.
 
---
   
### Environment Variables

The application uses the following environment variables (configured in `appsettings.json`):

- `Jwt:Key`: Secret key for signing tokens.  
- `Jwt:Issuer`: Token issuer.  
- `Jwt:Audience`: Token audience.
   ```bash
   "Jwt": {
  "Key": "supersecretkey1234567890123123@JWT!secureKey",
  "Issuer": "SchoolVaccinationPortal",
  "Audience": "SchoolVaccinationUsers"
  }
   ```

---

### Running the Application

1. **Start the Application**:
   ```bash
   dotnet run
   ```
2. **Access the API**:
    - *Swagger UI*: [http://localhost:5020/swagger](http://localhost:5020/swagger)  
    - *API Base URL*: [http://localhost:5020](http://localhost:5020)

---

### API Endpoints
#### Authentication
- **POST** `/auth/login`: Authenticate and get a JWT token.
#### Students
- **GET** `/api/students`: Get all students  
- **POST** `/api/students`: Add a new student  
- **PUT** `/api/students/{id}`: Update a student  
- **DELETE** `/api/students/{id}`: Delete a student  
- **POST** `/api/students/{id}/upload`: Upload a vaccination certificate  
- **GET** `/api/students/{id}/download`: Download a vaccination certificate
#### Vaccination Drives
- **GET** `/api/vaccinationdrive`: Get all vaccination drives  
- **POST** `/api/vaccinationdrive`: Schedule a new drive  
- **PUT** `/api/vaccinationdrive/{id}`: Update a drive
#### Vaccination Records
- **POST** `/api/vaccinationrecord`: Add or update a vaccination record
#### Reports
- **GET** `/api/reports/summary`: Get vaccination summary  
- **GET** `/api/reports/download-vaccination-details`: Download vaccination details as an Excel file
  
---
### Database Schema

#### Tables
- **Students**: Stores student details  
- **VaccinationDrives**: Stores vaccination drive details  
- **VaccinationRecords**: Stores vaccination records  
- **Users**: Stores user credentials for authentication

---

This `README.md` provides a comprehensive overview of your project, including setup instructions, API endpoints, and features. You can customize it further as needed.
