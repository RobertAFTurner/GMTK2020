using UnityEngine;

public class ReverseThrustCommand : ICommand
{
    public float Duration;
    public float Power;

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
        rigidbody.AddForce(rigidbody.transform.up * Power * -1f);
    }

    public override string ToString()
    {
        if (State == CommandState.InProgress)
            return $">Reverse thrust for {Duration} seconds at power {Power} ({(Time.time - startTime):.0#}/{Duration:.0#})";

        return $"Reverse thrust for {Duration} seconds at power {Power}";
    }
}