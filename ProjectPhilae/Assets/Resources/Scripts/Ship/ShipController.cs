﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ShipController : MonoBehaviour
{
    public enum ShipState
    {
        WaitingToLaunch,
        Executing,
        Stopped
    }

    List<ICommand> myCommands;
    ICommand currentlyExecutingCommand;

    [SerializeField]
    private Rigidbody2D shipRigidbody;

    private ShipState state;

    void Start()
    {
        state = ShipState.WaitingToLaunch;
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

    public void ExecuteCommands(List<ICommand> commands)
    {
        myCommands = new List<ICommand>(commands);
        state = ShipState.Executing;
        PopNextCommand();
    }

    private void PopNextCommand()
    {
        currentlyExecutingCommand = myCommands.FirstOrDefault();

        if (currentlyExecutingCommand != null)
            myCommands.RemoveAt(0);
    }
}
