using OpenMediator;
using WebApplicationAPI.Domain;

namespace WebApplicationAPI.UseCases.GetUser;

public record GetUserCommand(int Id) : ICommand<User>;
