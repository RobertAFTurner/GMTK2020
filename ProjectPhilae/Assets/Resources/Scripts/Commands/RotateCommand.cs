using UnityEngine;

public class RotateCommand : ICommand
{
    public enum AngularDirection
    {
        Clockwise,
        Anticlockwise
    }

    public float Duration;
    public float Degrees;
    public AngularDirection Direction;

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
            targetAngle = startAngle + Degrees * (Direction == AngularDirection.Clockwise ? -1f : 1f);
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

        var angle = Mathf.SmoothStep(startAngle, targetAngle, (Time.time - startTime)/Duration);
        //var angle = Mathf.SmoothDampAngle(rigidbody.rotation, targetAngle, ref currentVelocity, Duration/2, float.MaxValue, Time.deltaTime);
        rigidbody.MoveRotation(angle);
    }

    public override string ToString()
    {
        if (State == CommandState.InProgress)
            return $">Rotate by {Degrees} degrees {Direction} over {Duration} seconds ({(Time.time - startTime):.0#}/{Duration:.0#})";

        return $"Rotate by {Degrees} degrees {Direction} over {Duration} seconds";
    }
}