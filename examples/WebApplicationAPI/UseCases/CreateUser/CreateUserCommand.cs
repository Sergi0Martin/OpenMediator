using OpenMediator;

namespace WebApplicationAPI.UseCases.CreateUser;

public record CreateUserCommand(int Id, string Name) : ICommand;
