using UnityEngine;

public class RotateCommand : ICommand
{
    public float Duration;
    public float Degrees;
    public float Direction = 1f;
    private string directionString => Direction > 0 ? "anticlockwise" : "clockwise";

    public CommandState State { get; private set; } = CommandState.Pending;

    private float startTime;
    private float startAngle;
    private float targetAngle;
    //private float currentVelocity;

    public bool ExecuteTillDone(ShipController shipController)
    {
        if (State == CommandState.Pending)
        {
            State = CommandState.InProgress;
            startTime = Time.time;
            startAngle = shipController.GetRigidBody().rotation;
            targetAngle = startAngle + Degrees * Direction;
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

        var angle = Mathf.SmoothStep(startTime, targetAngle, (Time.time - startTime)/Duration);
        //var angle = Mathf.SmoothDampAngle(rigidbody.rotation, targetAngle, ref currentVelocity, Duration/2, float.MaxValue, Time.deltaTime);
        rigidbody.MoveRotation(angle);
    }

    public override string ToString()
    {
        return $"Rotate by {Degrees} degrees {directionString} over {Duration} seconds";
    }
}