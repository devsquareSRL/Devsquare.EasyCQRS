# Devsquare.EasyCQRS

A lightweight CQRS (Command Query Responsibility Segregation) implementation for .NET 9 applications, built on Clean Architecture principles and using MediatR for command and query dispatching.

## Features

- Clean separation of commands and queries following CQRS pattern
- Comprehensive dependency injection support
- Seamless integration with ASP.NET Core
- Minimal external dependencies
- Type-safe command and query dispatching

## Installation

```bash
dotnet add package Devsquare.EasyCQRS
```

## Usage

### Setting Up Dependency Injection

```csharp
// In Program.cs or Startup.cs
using Devsquare.EasyCQRS.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add EasyCQRS services with automatic handler registration
// Pass the assemblies containing your command and query handlers
builder.Services.AddEasyCQRS(typeof(Program).Assembly);
```

### Defining Commands and Queries

```csharp
// Command example
public class CreateUserCommand : ICommand<Guid>
{
    public string Username { get; set; }
    public string Email { get; set; }
}

// Command handler
public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Guid>
{
    public async Task<Guid> HandleAsync(CreateUserCommand command, CancellationToken cancellationToken)
    {
        // Implementation logic
        var userId = Guid.NewGuid();
        return userId;
    }
}

// Query example
public class GetUserByIdQuery : IQuery<UserDto>
{
    public Guid UserId { get; set; }
}

// Query handler
public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserDto>
{
    public async Task<UserDto> HandleAsync(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        // Implementation logic
        return new UserDto { /* ... */ };
    }
}
```

### Using the Command and Query Dispatchers

```csharp
// In a controller or service
public class UserController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public UserController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserCommand command)
    {
        var userId = await _commandDispatcher.SendAsync(command);
        return Ok(userId);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        var query = new GetUserByIdQuery { UserId = id };
        var user = await _queryDispatcher.SendAsync(query);
        return Ok(user);
    }
}
```