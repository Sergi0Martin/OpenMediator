using Microsoft.AspNetCore.Mvc;
using OpenMediator.Buses;
using WebApplicationAPI.UseCases;

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
}
