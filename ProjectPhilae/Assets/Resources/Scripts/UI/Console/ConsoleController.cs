using System.Collections.Generic;
using UnityEngine;

public class ConsoleController : MonoBehaviour
{
    [SerializeField]
    Canvas consoleCanvas;

    public List<ICommand> committedCommands;
    public ICommand draftCommand;

    void Start()
    {
        committedCommands = new List<ICommand>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Triggered by Button clicks ------------
    public void AddThrust()
    {
        Debug.Log("Adding draft thrust");
        var draftThrustCommand = new ThrustCommand();
        draftCommand = new ThrustCommand();
        // TODO: Instantiate ThrustCommand config pannel and pass the ThrustCommand to it to work on.
    }

    public void AddWait()
    {
        Debug.Log("Adding draft wait");
        var draftWaitCommand = new WaitCommand();
        draftCommand = new WaitCommand();
        // TODO: Instantiate Command config pannel and pass the Command to it to work on.
    }


    public void AddCommandToList()
    {
        Debug.Log("Adding current command");
        committedCommands.Add(draftCommand);
        draftCommand = null;
        // TODO: Destroy draft command config panel 
    }

        public void RemoveCommandFromList()
    {
        Debug.Log("Adding current command");
        committedCommands.RemoveAt(committedCommands.Count -1);
    }

    public void Execute()
    {
        Debug.Log("Executing commands");
    }
    // Triggered by Button clicks ------------
}
