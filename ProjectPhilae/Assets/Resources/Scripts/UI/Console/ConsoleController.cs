using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ConsoleController : MonoBehaviour
{
    public List<Command> Commands;
    public Command DraftCommand;

    [SerializeField] private GameObject thrustNodePrefab;
    [SerializeField] private GameObject waitNodePrefab;
    [SerializeField] private GameObject reverseThrustNodePrefab;
    [SerializeField] private GameObject stopNodePrefab;
    [SerializeField] private GameObject rotateNodePrefab;
    [SerializeField] private Canvas consoleCanvas;
    [SerializeField] private GameObject configPanelPlaceholder;
    private GameObject currentNodeInstance;
    private int? selectedCommandIndex;
    private Dictionary<Type, GameObject> nodePrefabLookup;


    // Triggered by Button clicks ------------
    public void AddThrust() => SetNewDraftCommand(new ThrustCommand(), thrustNodePrefab);
    public void AddWait() => SetNewDraftCommand(new WaitCommand(), waitNodePrefab);
    public void AddReverseThrust() => SetNewDraftCommand(new ReverseThrustCommand(), reverseThrustNodePrefab);
    public void AddStop() => SetNewDraftCommand(new StopCommand(), stopNodePrefab);
    public void AddRotate() => SetNewDraftCommand(new RotateCommand(), rotateNodePrefab);

    public void Start()
    {
        nodePrefabLookup = new Dictionary<Type, GameObject>
        {
            [typeof(ThrustCommand)] = thrustNodePrefab,
            [typeof(WaitCommand)] = waitNodePrefab,
            [typeof(ReverseThrustCommand)] = reverseThrustNodePrefab,
            [typeof(StopCommand)] = stopNodePrefab,
            [typeof(RotateCommand)] = rotateNodePrefab,
        };

        var prevCommands = GameManagerController.Instance.GetPreviousCommands();
        Commands = prevCommands != null ? prevCommands : new List<Command>();
        currentNodeInstance = null;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
            SelectDown();

        if (Input.GetKeyDown(KeyCode.UpArrow))
            SelectUp();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (GameManagerController.Instance.State == GameManagerController.GameStates.EnterInstructions)
            {
                Execute();
            }
            else
            {
                Abort();
            }
        }

        if (Input.GetKeyDown(KeyCode.Delete)) //Backspace would interfere with typing || Input.GetKeyDown(KeyCode.Backspace) 
        {
            RemoveCommandFromList();
        }

        if (Input.GetKeyDown(KeyCode.Return))
            CloseDraftCommandNode();

        if (Input.GetKeyDown(KeyCode.T))
            AddThrust();

        if (Input.GetKeyDown(KeyCode.W))
            AddWait();

        if (Input.GetKeyDown(KeyCode.R))
            AddRotate();

        if (Input.GetKeyDown(KeyCode.B))
            AddReverseThrust();

        if (Input.GetKeyDown(KeyCode.S))
            AddStop();
    }

    public void CommitCommand()
    {
        selectedCommandIndex = null;
        DraftCommand.State = CommandState.Pending;
        CloseDraftCommandNode();
    }

    public void RemoveCommandFromList()
    {
        Debug.Log("Removing command");
        if (selectedCommandIndex.HasValue)
        {
            CloseDraftCommandNode(false);
            Commands.RemoveAt(selectedCommandIndex.Value);
            SelectUp();
        }
        else if (Commands.Count > 0)
        {
            Commands.RemoveAt(Commands.Count - 1);
        }
    }

    public void Execute()
    {
        Debug.Log("Executing commands");
        GameManagerController.Instance.StartExecution();
        ShipController.Instance.ExecuteCommands(Commands);
    }

    public void SelectUp()
    {
        MoveSelection(-1);
    }

    public void SelectDown()
    {
        MoveSelection(1);
    }

    public void Abort()
    {
        ShipController.Instance.Stop();
        GameManagerController.Instance.LoadLevel(true);
    }

    private void MoveSelection(int offset)
    {
        selectedCommandIndex = selectedCommandIndex.HasValue ? selectedCommandIndex + offset : Commands.Count - 1;

        if (selectedCommandIndex >= Commands.Count)
            CloseDraftCommandNode();

        if (selectedCommandIndex < 0)
            selectedCommandIndex = 0;

        if (selectedCommandIndex.HasValue)
        {
            var command = Commands[selectedCommandIndex.Value];
            MethodInfo method = typeof(ConsoleController).GetMethod(nameof(SetDraftCommand));
            MethodInfo generic = method.MakeGenericMethod(command.GetType());
            object[] parameters = { command, nodePrefabLookup[command.GetType()] };
            generic.Invoke(this, parameters);
        }
    }

    public void SetNewDraftCommand<T>(T draftCommand, GameObject configPrefab) where T : Command
    {
        Commands.Add(draftCommand);
        selectedCommandIndex = Commands.Count - 1;
        SetDraftCommand(draftCommand, configPrefab);
    }

    public void SetDraftCommand<T>(T draftCommand, GameObject configPrefab) where T : Command
    {
        if (DraftCommand != null)
        {
            CloseDraftCommandNode(false);
        }

        Debug.Log($"Adding command: {draftCommand.GetType().Name}");
        DraftCommand = draftCommand;
        DraftCommand.State = CommandState.Editing;
        DisplayConfigPanelUi(draftCommand, configPrefab);
    }

    public void CloseDraftCommandNode(bool clearSelectedCommandIndex = true)
    {
        if (clearSelectedCommandIndex)
            selectedCommandIndex = null;

        DraftCommand.State = CommandState.Pending;
        currentNodeInstance?.GetComponent<NodeBase>().DestroySelf();
        currentNodeInstance = null;
    }

    private void DisplayConfigPanelUi<T>(T command, GameObject prefab) where T : Command
    {
        var instance = Instantiate(prefab, new Vector3(0, 0, -10), Quaternion.identity);
        instance.transform.SetParent(configPanelPlaceholder.transform, false);
        instance.GetComponent<NodeBase<T>>().SetCommand(command);
        currentNodeInstance = instance;
    }

    // Triggered by Button clicks ------------
}