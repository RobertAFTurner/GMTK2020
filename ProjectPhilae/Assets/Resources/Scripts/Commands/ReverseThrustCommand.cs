using UnityEngine;

public class ReverseThrustCommand : Command
{
    public ReverseThrustCommand() : this(0.1f, 5f)
    {}

    public ReverseThrustCommand(float duration, float power) : base(duration)
    {
        Power = power;
    }

    private const float FuelConsumptionMultiplier = 10f;

    public float Power;

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
        rigidbody.AddForce(rigidbody.transform.up * Power * -1f);
        shipController.fuel -= FuelConsumptionMultiplier * Power * Time.deltaTime;
    }

    protected override string GetDisplayText()
    {
        return $"Reverse thrust for {Duration} seconds at power {Power}";
    }
}