using UnityEngine;

public class WaitCommand : Command
{
    public WaitCommand() : this(2f)
    { }

    public WaitCommand(float duration) : base(duration)
    {
    }

    public override bool ExecuteTillDone(ShipController shipController)
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

    protected override string GetDisplayText()
    {
        return $"Wait for {Duration} seconds";
    }
}