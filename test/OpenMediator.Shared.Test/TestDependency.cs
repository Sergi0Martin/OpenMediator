namespace OpenMediator.Shared.Test;

public sealed class TestDependency
{
    public int Counter { get; set; }
    public bool Called => Counter > 0;

    public void Call()
    {
        Counter++;
    }
}
