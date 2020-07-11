using UnityEngine;

public class WaitCommand : ICommand
{
    public float Duration;

    public CommandState State { get; private set; } = CommandState.Pending;

    private float startTime;

    public bool ExecuteTillDone(ShipController shipController)
    {
        if (State == CommandState.Pending)
        {
            State = CommandState.InProgress;
            startTime = Time.time;
        }

        if (State == CommandState.InProgress)
        {
            if (Time.time > startTime + Duration)
            {
                State = CommandState.Done;
                return true;
            }
        }

        return false;
    }

    public override string ToString()
    {
        if (State == CommandState.InProgress)
            return $">Wait for {Duration} seconds ({(Time.time - startTime):.0#}/{Duration:.0#})";

        return $"Wait for {Duration} seconds";
    }
}