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

        Commands = new List<Command>();
        currentNodeInstance = null;
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
            CloseDraftCommandNode();
            Commands.RemoveAt(selectedCommandIndex.Value);
            SelectUp();
        }
        else if (Commands.Count > 0)
        {
            Commands.RemoveAt(Commands.Count -1);
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
            object[] parameters = {command, nodePrefabLookup[command.GetType()]};
            generic.Invoke(this, parameters);
        }
    }
    
    // Update is called once per frame
    private void Update()
    {
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
            CloseDraftCommandNode();
        }

        Debug.Log($"Adding command: {draftCommand.GetType().Name}");
        DraftCommand = draftCommand;
        DraftCommand.State = CommandState.Editing;
        DisplayConfigPanelUi(draftCommand, configPrefab);
    }

    public void CloseDraftCommandNode()
    {
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