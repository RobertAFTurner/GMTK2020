using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ShipState
{
    WaitingToLaunch,
    Executing,
    OutOfControl,
    Stopped
}

[RequireComponent(typeof(Rigidbody2D))]
public class ShipController : SingletonDestructible<ShipController>
{
    public static ShipController Instance => (ShipController)instance;

    public float Fuel;
    public float StartingFuel;
    public float LaunchTime;
    public float StopTime;
    public Command CurrentlyExecutingCommand;

    private List<Command> myCommands;

    [SerializeField]
    private Rigidbody2D shipRigidbody;

    [SerializeField] public GameObject thrustSprite;
    [SerializeField] public ParticleSystem thrustSystem;
    [SerializeField] public ParticleSystem rotateClockwiseSystem;
    [SerializeField] public ParticleSystem rotateAnticlockwiseSystem;
    [SerializeField] public ParticleSystem stopLeftSystem;
    [SerializeField] public ParticleSystem stopRightSystem;

    public ShipState State { get; private set; }

    void Start()
    {
        Load();
    }

    private void Load()
    {
        LaunchTime = 0f;
        StartingFuel = FindObjectOfType<LevelData>().StartingFuel;
        Fuel = StartingFuel;
        State = ShipState.WaitingToLaunch;
        shipRigidbody = GetComponent<Rigidbody2D>();
    }

    public void Stop()
    {
        if (State != ShipState.Stopped)
        {
            StopTime = Time.time;
            State = ShipState.Stopped;
            shipRigidbody.velocity = Vector2.zero;
        }
    }

    void FixedUpdate()
    {
        if (State == ShipState.Executing)
        {
            if (Fuel < 0f)
                Fuel = 0f;

            if (LaunchTime < 0.5f)
                LaunchTime = Time.time;

            if (CurrentlyExecutingCommand.ExecuteTillDone(this))
            {
                PopNextCommand();
                if (CurrentlyExecutingCommand == null)
                    State = ShipState.OutOfControl;
            }
        }
    }

    public Rigidbody2D GetRigidBody()
    {
        return shipRigidbody;
    }

    public void ExecuteCommands(List<Command> commands)
    {
        myCommands = new List<Command>(commands);
        State = ShipState.Executing;
        PopNextCommand();
    }

    private void PopNextCommand()
    {
        CurrentlyExecutingCommand = myCommands.FirstOrDefault();

        if (CurrentlyExecutingCommand != null)
            myCommands.RemoveAt(0);
    }

    public void ReturnToSpawnPoint()
    {
        Load();
        var spawnPoint = GameObject.Find("PlayerSpawnPoint");
        transform.position = spawnPoint.transform.position;
        Stop();
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Environment")
        {
            Debug.Log("Boom - you died");        
            Stop();
            AudioManagerController.Instance.StopSound("Thrust");
            GameManagerController.Instance.LoadLevel(true);
        }
    }
}
