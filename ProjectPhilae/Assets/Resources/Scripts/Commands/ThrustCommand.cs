using UnityEngine;

public class ThrustCommand : Command
{
    public ThrustCommand() : this(0.5f, 3f)
    { }

    public ThrustCommand(float duration, float power) : base(duration)
    {
        Power = power;
    }

    private const float FuelConsumptionMultiplier = 10f;

    public float Power;


    private void StartParticles(ShipController shipController)
    {
        if (!shipController.thrustSystem.isPlaying)
        {
            var main = shipController.thrustSystem.main;
            main.startSpeed = Power / 10;

            var emmision = shipController.thrustSystem.emission;
            emmision.rateOverTime = Power * 5;

            shipController.thrustSystem.Play();
            shipController.thrustSprite.SetActive(true);
        }
    }

    private void StopParticles(ShipController shipController)
    {
        if (shipController.thrustSystem.isPlaying)
        {
            shipController.thrustSystem.Stop();
            shipController.thrustSprite.SetActive(false);
        }            
    }

    public override bool ExecuteTillDone(ShipController shipController)
    {
        if (State == CommandState.Pending)
        {
            AudioManagerController.Instance.PlaySound("Thrust");
            State = CommandState.InProgress;
            startTime = Time.time;
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
                AudioManagerController.Instance.StopSound("Thrust");
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
        rigidbody.AddForce(rigidbody.transform.up * Power);
        shipController.Fuel -= FuelConsumptionMultiplier * Power * Time.deltaTime;
    }

    protected override string GetDisplayText()
    {
        return $"Thrust for {Duration} seconds at power {Power}";
    }
}