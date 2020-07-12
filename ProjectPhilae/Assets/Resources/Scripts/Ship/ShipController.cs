using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ShipController : SingletonDestructible<ShipController>
{
    public static ShipController Instance => (ShipController)instance;

    public enum ShipState
    {
        WaitingToLaunch,
        Executing,
        OutOfControl,
        Stopped
    }

    public float Fuel;
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

    private ShipState state;

    void Start()
    {
        Load();
    }

    private void Load()
    {
        LaunchTime = 0f;
        Fuel = 100f;
        state = ShipState.WaitingToLaunch;
        shipRigidbody = GetComponent<Rigidbody2D>();
    }

    public void Stop()
    {
        if (state != ShipState.Stopped)
        {
            StopTime = Time.time;
            state = ShipState.Stopped;
            shipRigidbody.velocity = Vector2.zero;
        }
    }

    void FixedUpdate()
    {
        if (state == ShipState.Executing)
        {
            if (LaunchTime < 0.5f)
                LaunchTime = Time.time;

            if (CurrentlyExecutingCommand.ExecuteTillDone(this))
            {
                PopNextCommand();
                if (CurrentlyExecutingCommand == null)
                    state = ShipState.OutOfControl;
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
        state = ShipState.Executing;
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
        if(other.gameObject.tag == "Environment")
        {
            Debug.Log("Boom - you died");
            Stop();
            GameManagerController.Instance.LoadLevel(true);
        }
    }
}
