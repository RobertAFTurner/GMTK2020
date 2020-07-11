using UnityEngine;

public class StopCommand : ICommand
{
    public float Duration;

    public CommandState State { get; private set; } = CommandState.Pending;

    private float startTime;

    private Vector2 startVel;

    public bool ExecuteTillDone(ShipController shipController)
    {
        if (State == CommandState.Pending)
        {
            State = CommandState.InProgress;
            startTime = Time.time;
            startVel = shipController.GetRigidBody().velocity;
        }

        if (State == CommandState.InProgress)
        {
            Execute(shipController);
            if (Time.time > startTime + Duration)
            {
                State = CommandState.Done;
                return true;
            }
        }

        return false;
    }

    private void Execute(ShipController shipController)
    {
        var rigidbody = shipController.GetRigidBody();
        var timeSinceStarted = Time.time - startTime;
        var percentComplete = timeSinceStarted / Duration;
        rigidbody.velocity = Vector2.Lerp(startVel, Vector2.zero, percentComplete);
    }

    public override string ToString()
    {
        if (State == CommandState.InProgress)
            return $">Slow down to a stop over {Duration} seconds ({(Time.time - startTime):.0#}/{Duration:.0#})";

        return $"Slow down to a stop over {Duration} seconds";
    }
}