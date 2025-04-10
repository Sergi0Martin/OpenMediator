using OpenMediator;

namespace WebApplicationAPI.UseCases;

public record CreateUserCommand(int Id, string Name) : ICommand;
