using UnityEngine;

public class WaitCommand : Command
{
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
        if (State == CommandState.InProgress)
            return $">Wait for {Duration} seconds ({(Time.time - startTime):.0#}/{Duration:.0#})";

        return $"Wait for {Duration} seconds";
    }
}