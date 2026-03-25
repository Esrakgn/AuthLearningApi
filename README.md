# AuthLearningApi

A layered ASP.NET Core Web API project built for learning authentication and authorization.

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

## Project Structure

- `AuthLearningApi.Api` - controllers, middleware, configuration
- `AuthLearningApi.Application` - DTOs, validators, shared models
- `AuthLearningApi.Domain` - entities
- `AuthLearningApi.Persistence` - DbContext and migrations
- `AuthLearningApi.Infrastructure` - JWT and infrastructure services

## Endpoints

- `POST /api/auth/register`
- `POST /api/auth/login`
- `GET /api/auth/me`
- `GET /api/auth/admin-only`
- `GET /api/auth/users?pageNumber=1&pageSize=10`

## Notes

This project was created to practice backend fundamentals such as authentication, authorization, validation, exception handling, logging, and pagination.
