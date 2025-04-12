using Microsoft.AspNetCore.Mvc;
using OpenMediator.Buses;
using WebApplicationAPI.Domain;
using WebApplicationAPI.UseCases.CreateUser;
using WebApplicationAPI.UseCases.GetUser;
using WebApplicationAPI.ViewModels;

namespace WebApplicationAPI.Controllers;

[ApiController]
[Route("api/user")]
public class UserController(IMediatorBus _mediator) : ControllerBase
{
    [HttpPost()]
    public async Task<IActionResult> CreateUser()
    {
        var command = new CreateUserCommand(1, "UserTest");
        await _mediator.SendAsync(command);
        return Ok();
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var command = new GetUserCommand(id);
        var user = await _mediator.SendAsync<GetUserCommand, User>(command);
        return Ok(UserViewModel.FromUser(user));
    }
}
