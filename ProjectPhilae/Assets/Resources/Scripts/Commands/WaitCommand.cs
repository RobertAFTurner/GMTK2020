public class WaitCommand : ICommand
{
  public float Duration;

  public CommandState State { get; private set; } = CommandState.Pending;

  public bool ExecuteTillDone(ShipController shipController)
  {
    State = CommandState.Done;
    return true;
  }

  public override string ToString()
  {
    return $"Waiting for {Duration} seconds";
  }
}