public class ThrustCommand : ICommand
{
  public float Duration;
  public float Power;

  public bool ExecuteTillDone()
  {
    return true;
  }

  public override string ToString()
  {
    return $"Thrust for {Duration} seconds at power {Power}";
  }
}