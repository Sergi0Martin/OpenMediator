using Microsoft.Extensions.DependencyInjection;
using OpenMediator.Shared.Test;
using OpenMediator.Shared.Test.Commands;

namespace OpenMediator.Unit.Test.Commands;

[Collection("CommandTests")]
public sealed class CommandTest : IClassFixture<UnitTestFixture>
{
    private readonly UnitTestFixture _fixture;
    private TestDependency _testDependency;

    public CommandTest(UnitTestFixture fixture)
    {
        _fixture = fixture;
        _testDependency = _fixture.ServiceProvider.GetService<TestDependency>()!;
    }

    [Fact]
    public async Task Send_Command_Works()
    {
        // Arrange
        var command = new CreateUserCommand(1, "UserTest");

        // Act
        await _fixture.Mediator.SendAsync(command);

        // Assert
        Assert.True(_testDependency.Called);
    }
}