# AuthLearningApi

A layered ASP.NET Core Web API project built to practice authentication, authorization, validation, logging, and pagination.

## Features

- JWT Authentication
- Role-based Authorization
- Claims-based identity
- Register / Login endpoints
- BCrypt password hashing
- FluentValidation
- Global Exception Middleware
- Serilog logging
- Pagination
- MSSQL with EF Core
- Service layer structure

## Tech Stack

- ASP.NET Core Web API
- Entity Framework Core
- SQL Server / LocalDB
- JWT Bearer Authentication
- FluentValidation
- Serilog
- BCrypt.Net

## Project Structure

- `AuthLearningApi.Api` - controllers, middleware, configuration
- `AuthLearningApi.Application` - DTOs, validators, interfaces, shared models
- `AuthLearningApi.Domain` - entities
- `AuthLearningApi.Persistence` - DbContext and migrations
- `AuthLearningApi.Infrastructure` - JWT, services, and infrastructure logic

## Endpoints

- `POST /api/auth/register`
- `POST /api/auth/login`
- `GET /api/auth/me`
- `GET /api/auth/admin-only`
- `GET /api/auth/users?pageNumber=1&pageSize=10`

## Notes

This project was created as a learning project to practice backend fundamentals such as authentication, authorization, claims, validation, password hashing, exception handling, logging, and pagination.

