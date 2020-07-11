public class WaitCommand : ICommand
{
    public float Duration;

    public bool ExecuteTillDone()
    {
        return true;
    }

    public override string ToString()
    {
        return $"Waiting for {Duration} seconds";
    }
}