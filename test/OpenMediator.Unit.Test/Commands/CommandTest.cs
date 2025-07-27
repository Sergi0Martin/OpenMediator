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

    [Fact]
    public async Task Send_Command_Throw_If_Not_Registered()
    {
        // Arrange
        var command = new CreateCarCommand(1);

        // Act
        Func<Task> action = async () => await _fixture.Mediator.SendAsync(command);

        // Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(action);
        Assert.Equal("Handler not found for command CreateCarCommand", exception.Message);
    }

    public record CreateCarCommand(int Id) : ICommand;
    
    
    [Fact]
    public async Task HandleAsync_ShouldThrow_WhenCancellationIsRequested()
    {
        // Arrange
        var handler = new LongRunningCommandHandler();
        var command = new LongRunningCommand();

        using var cts = new CancellationTokenSource();
        await cts.CancelAsync(); // Já está cancelado antes de executar

        // Act & Assert
        await Assert.ThrowsAsync<OperationCanceledException>(() =>
            handler.HandleAsync(command, cts.Token));
    }

}