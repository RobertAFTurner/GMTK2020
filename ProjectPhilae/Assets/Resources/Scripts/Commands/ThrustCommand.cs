using UnityEngine;

public class ThrustCommand : Command
{
    private const float FuelConsumptionMultiplier = 10f;

    public float Power;

    public override bool ExecuteTillDone(ShipController shipController)
    {
        if (State == CommandState.Pending)
        {
            State = CommandState.InProgress;
            startTime = Time.time;
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
        rigidbody.AddForce(rigidbody.transform.up * Power);
        shipController.fuel -= FuelConsumptionMultiplier * Power * Time.deltaTime;
    }

    protected override string GetDisplayText()
    {
        return $"Thrust for {Duration} seconds at power {Power}";
    }
}