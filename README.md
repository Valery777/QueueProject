 Queue Project

A .NET 8 Web API for managing queues using MongoDB, CQRS, and MediatR. 

Features

- Create, update, delete, and retrieve queues
- MongoDB integration using `MongoDB.Driver`
- CQRS pattern with MediatR
- Clean Architecture & SOLID principles
- JWT authentication with login endpoint
- Swagger UI with JWT “Authorize” button UI for API documentation
- Unit tests for handlers (Moq)
- Integration tests using WebApplicationFactory
- In-memory repository for integration tests (no MongoDB required)

 Tech Stack

| Technology             | Description                                                        |
|------------------------|--------------------------------------------------------------------| 
| .NET 8                 | Web API Framework                                                  |
| MongoDB                | NoSQL Database                                                     |
| MediatR                | CQRS and Mediator Pattern                                          |
| Swagger / Swashbuckle  | API Documentation                                                  |
| ILogger / Serilog      | Built-in Logging                                                   |
| JWT Authentication     | Secure token‑based authentication for protected endpoints          |                                           
| xUnit			         | Unit testing framework                                             |
| Moq                    | Mocking library for handler tests                                  |
| WebApplicationFactory  | Integration testing infrastructure                                 |
| Clean Architecture     | Separation of API, Application, Domain, and Infrastructure layers  |                                  |
 



