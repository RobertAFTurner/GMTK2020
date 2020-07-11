using UnityEngine;

public class StopCommand : Command
{
    public StopCommand() : this(1f)
    { }

    public StopCommand(float duration) : base(duration)
    {
    }

    private const float FuelConsumptionMultiplier = 10f;
    
    private Vector2 startVel;

    public override bool ExecuteTillDone(ShipController shipController)
    {
        if (State == CommandState.Pending)
        {
            State = CommandState.InProgress;
            startTime = Time.time;
            startVel = shipController.GetRigidBody().velocity;
        }

        if (State == CommandState.InProgress)
        {
            if (shipController.fuel <= 0)
            {
                State = CommandState.UnableToComplete;
                return true;
            }

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

        shipController.fuel -= FuelConsumptionMultiplier * Time.deltaTime / Duration;
    }

    protected override string GetDisplayText()
    { 
        return $"Slow down to a stop over {Duration} seconds";
    }
}