using UnityEngine;

public class StopCommand : Command
{
    public StopCommand() : this(1f)
    { }

    public StopCommand(float duration) : base(duration)
    {
    }

    private const float FuelConsumptionMultiplier = 20f;
    
    private Vector2 startVel;

    private void StartParticles(ShipController shipController)
    {
        if (!shipController.stopLeftSystem.isPlaying)
            shipController.stopLeftSystem.Play();

        if (!shipController.stopRightSystem.isPlaying)
            shipController.stopRightSystem.Play(); 
    }

    private void StopParticles(ShipController shipController)
    {
        if (shipController.stopLeftSystem.isPlaying)
            shipController.stopLeftSystem.Stop();

        if (shipController.stopRightSystem.isPlaying)
            shipController.stopRightSystem.Stop(); 
    }

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
            StartParticles(shipController);

            if (shipController.Fuel <= 0)
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
        var timeSinceStarted = Time.time - startTime;
        var percentComplete = timeSinceStarted / Duration;
        rigidbody.velocity = Vector2.Lerp(startVel, Vector2.zero, percentComplete);

        shipController.Fuel -= FuelConsumptionMultiplier * Time.deltaTime / Duration;
    }

    protected override string GetDisplayText()
    { 
        return $"Slow down to a stop over {Duration} seconds";
    }
}