using UnityEngine;

public class RotateCommand : Command
{
    public RotateCommand() : this(2f, 90, AngularDirection.Clockwise)
    { }

    public RotateCommand(float duration, float degrees, AngularDirection direction) : base(duration)
    {
        Degrees = degrees;
        Direction = direction;
    }

    private const float FuelConsumptionMultiplier = 5f;

    public enum AngularDirection
    {
        Clockwise,
        Anticlockwise
    }

    public float Degrees;
    public AngularDirection Direction;

    private float startAngle;
    private float targetAngle;
    //private float currentVelocity;

    private void StartParticles(ShipController shipController)
    {
        if (Direction == AngularDirection.Clockwise && !shipController.rotateClockwiseSystem.isPlaying)
            shipController.rotateClockwiseSystem.Play();

        if (Direction == AngularDirection.Anticlockwise &&!shipController.rotateAnticlockwiseSystem.isPlaying)
            shipController.rotateAnticlockwiseSystem.Play();
    }

    private void StopParticles(ShipController shipController)
    {
        if (shipController.rotateClockwiseSystem.isPlaying)
            shipController.rotateClockwiseSystem.Stop();

        if (shipController.rotateAnticlockwiseSystem.isPlaying)
            shipController.rotateAnticlockwiseSystem.Stop();
    }

    public override bool ExecuteTillDone(ShipController shipController)
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
            StartParticles(shipController);
            
            if (shipController.fuel <= 0)
            {
                StopParticles(shipController);
                State = CommandState.UnableToComplete;
                return true;
            }

            Execute(shipController);

            if (Time.time > startTime + Duration)
            {
                StopParticles(shipController);
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

        shipController.fuel -= FuelConsumptionMultiplier * Time.deltaTime / Duration;
    }

    protected override string GetDisplayText()
    {
        return $"Rotate by {Degrees} degrees {Direction} over {Duration} seconds";
    }
}