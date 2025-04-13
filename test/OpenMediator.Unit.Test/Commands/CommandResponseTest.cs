using Microsoft.Extensions.DependencyInjection;
using OpenMediator.Shared.Test;
using OpenMediator.Shared.Test.Commands;

namespace OpenMediator.Unit.Test.Commands;

[Collection("CommandTests")]
public sealed class CommandResponseTest : IClassFixture<UnitTestFixture>
{
    private readonly UnitTestFixture _fixture;
    private TestDependency _testDependency;

    public CommandResponseTest(UnitTestFixture fixture)
    {
        _fixture = fixture;
        _testDependency = _fixture.ServiceProvider.GetService<TestDependency>()!;
    }

    [Fact]
    public async Task Send_Command_With_Response_Works()
    {
        // Arrange
        var command = new GetUserCommand(1);

        // Act
        var result = await _fixture.Mediator.SendAsync<GetUserCommand, User>(command);

        // Assert
        var expectedResult = new User(
            Id: 1,
            Name: "Ralph",
            Email: "ralp@company.com");
        Assert.Equal(result, expectedResult);
        Assert.True(_testDependency.Called);
    }

    [Fact]
    public async Task Send_Command_With_Response_Throw_If_Not_Registered()
    {
        // Arrange
        var command = new GetCarModelCommand(1);

        // Act
        Func<Task> action = async () => await _fixture.Mediator.SendAsync<GetCarModelCommand, string>(command);

        // Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(action);
        Assert.Equal("Handler not found for command GetCarModelCommand", exception.Message);
    }

    public record GetCarModelCommand(int Id) : ICommand<string>;

}
