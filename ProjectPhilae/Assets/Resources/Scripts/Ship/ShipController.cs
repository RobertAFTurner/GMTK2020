﻿using System.Collections;
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
        Stopped
    }

    public float fuel;
    public List<Command> myCommands;
    public Command currentlyExecutingCommand;

    [SerializeField]
    private Rigidbody2D shipRigidbody;

    private ShipState state;

    void Start()
    {
        Load();
    }

    private void Load()
    {
        fuel = 100f;
        state = ShipState.WaitingToLaunch;
        shipRigidbody = GetComponent<Rigidbody2D>();
    }

    public void Stop()
    {
        state = ShipState.Stopped;
        shipRigidbody.velocity = Vector2.zero;
    }

    void FixedUpdate()
    {
        if (state == ShipState.Executing)
        {
            if (currentlyExecutingCommand.ExecuteTillDone(this))
            {
                PopNextCommand();
                if (currentlyExecutingCommand == null)
                    state = ShipState.Stopped;
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
        currentlyExecutingCommand = myCommands.FirstOrDefault();

        if (currentlyExecutingCommand != null)
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
