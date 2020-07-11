using System.Collections.Generic;
using UnityEngine;

public class ConsoleController : MonoBehaviour
{ 
  public List<ICommand> CommittedCommands;
  public ICommand DraftCommand;

  [SerializeField] private ShipController ship;
  [SerializeField] private GameObject thrustNode;
  [SerializeField] private Canvas consoleCanvas;
  [SerializeField] private GameObject configPanelPlaceholder;

  [SerializeField] private GameManagerController gameManager;
  private GameObject currentNodeInstance;

  public void SetShip(ShipController ship)
  {
    this.ship = ship;
  }

  // Triggered by Button clicks ------------
  public void AddThrust()
  {
    Debug.Log("Adding draft thrust");
    var draftThrustCommand = new ThrustCommand();
    DraftCommand = new ThrustCommand();
    // TODO: Instantiate ThrustCommand config pannel and pass the ThrustCommand to it to work on.
    DisplayThrustUI((ThrustCommand)DraftCommand);
  }

  public void AddWait()
  {
    Debug.Log("Adding draft wait");
    var draftWaitCommand = new WaitCommand();
    DraftCommand = new WaitCommand();
    // TODO: Instantiate Command config pannel and pass the Command to it to work on.
  }


  public void AddCommandToList()
  {
    Debug.Log("Adding current command");
    CommittedCommands.Add(DraftCommand);

    currentNodeInstance.GetComponent<NodeBase>().DestroySelf();
    DraftCommand = null;
    currentNodeInstance = null;
    // TODO: Destroy draft command config panel 
  }

  public void RemoveCommandFromList()
  {
    Debug.Log("Adding current command");
    CommittedCommands.RemoveAt(CommittedCommands.Count - 1);
  }

  public void Execute()
  {
    Debug.Log("Executing commands");
    gameManager.StartExecution();
    ship.ExecuteCommands(CommittedCommands);
  }

  private void Start()
  {
    CommittedCommands = new List<ICommand>();
    currentNodeInstance = null;
  }

  // Update is called once per frame
  private void Update()
  {
  }


  private void DisplayThrustUI(ThrustCommand command)
  {
    var instance = Instantiate(thrustNode, new Vector3(0, 0, -10), Quaternion.identity);
    instance.transform.SetParent(configPanelPlaceholder.transform, false);
    instance.GetComponent<ThrustNodeController>().SetThrustCommand(command);
    currentNodeInstance = instance;
  }

  // Triggered by Button clicks ------------
}