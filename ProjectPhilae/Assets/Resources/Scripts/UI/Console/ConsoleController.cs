using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

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
    private CommandDisplay commandDisplay;
    private GameObject currentNodeInstance;
    private int? selectedCommandIndex;
    private Dictionary<Type, GameObject> nodePrefabLookup;

    public void AddThrust() => SetNewDraftCommand(new ThrustCommand(), thrustNodePrefab);
    public void AddWait() => SetNewDraftCommand(new WaitCommand(), waitNodePrefab);
    public void AddReverseThrust() => SetNewDraftCommand(new ReverseThrustCommand(), reverseThrustNodePrefab);
    public void AddStop() => SetNewDraftCommand(new StopCommand(), stopNodePrefab);
    public void AddRotate() => SetNewDraftCommand(new RotateCommand(), rotateNodePrefab);

    public void Start()
    {
        var buttons = FindObjectsOfType<Button>();

        buttons.Single(b => b.gameObject.name == "ThrustButton").onClick.AddListener(AddThrust);
        buttons.Single(b => b.gameObject.name.Contains("Wait")).onClick.AddListener(AddWait);
        buttons.Single(b => b.gameObject.name.Contains("ReverseThrust")).onClick.AddListener(AddReverseThrust);
        buttons.Single(b => b.gameObject.name.Contains("Stop")).onClick.AddListener(AddStop);
        buttons.Single(b => b.gameObject.name.Contains("Rotate")).onClick.AddListener(AddRotate);

        buttons.Single(b => b.gameObject.name.Contains("Done")).onClick.AddListener(Done);
        buttons.Single(b => b.gameObject.name.Contains("Execute")).onClick.AddListener(Execute);
        buttons.Single(b => b.gameObject.name.Contains("Remove")).onClick.AddListener(Remove);
        buttons.Single(b => b.gameObject.name.Contains("Up")).onClick.AddListener(Up);
        buttons.Single(b => b.gameObject.name.Contains("Down")).onClick.AddListener(Down);
        buttons.Single(b => b.gameObject.name.Contains("Abort")).onClick.AddListener(Abort);

        nodePrefabLookup = new Dictionary<Type, GameObject>
        {
            [typeof(ThrustCommand)] = thrustNodePrefab,
            [typeof(WaitCommand)] = waitNodePrefab,
            [typeof(ReverseThrustCommand)] = reverseThrustNodePrefab,
            [typeof(StopCommand)] = stopNodePrefab,
            [typeof(RotateCommand)] = rotateNodePrefab,
        };

        commandDisplay = FindObjectOfType<CommandDisplay>();
        var prevCommands = GameManagerController.Instance.GetPreviousCommands();
        Commands = prevCommands ?? new List<Command>();
        currentNodeInstance = null;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
            Down();

        if (Input.GetKeyDown(KeyCode.UpArrow))
            Up();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (GameManagerController.Instance.State == GameManagerController.GameStates.EnterInstructions)
                Execute();
            else
                Abort();
        }

        if (Input.GetKeyDown(KeyCode.Delete)) //Backspace would interfere with typing || Input.GetKeyDown(KeyCode.Backspace) 
            Remove();

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (GameManagerController.Instance.State == GameManagerController.GameStates.EnterInstructions)
                Done();

            if (GameManagerController.Instance.State == GameManagerController.GameStates.Win)
                GameManagerController.Instance.LoadNextLevel();
        }

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

        if (Input.GetKeyDown(KeyCode.P))
            GameManagerController.Instance.LoadNextLevel();
    }

    public void Done()
    {
        if (GameManagerController.Instance.State != GameManagerController.GameStates.EnterInstructions)
            return;

        AudioManagerController.Instance.PlaySound("KeyPress");
        if (DraftCommand == null)
        {
            if (Commands.Count == 0)
                commandDisplay.SetUserPrompt("The return key closes any commands which are open for editing.\r\n\r\nNo commands entered! Insert disks to add commands");

            return;
        }

        selectedCommandIndex = null;
        DraftCommand.State = CommandState.Pending;
        CloseDraftCommandNode();
    }

    public void Remove()
    {
        if (GameManagerController.Instance.State != GameManagerController.GameStates.EnterInstructions)
            return;

        AudioManagerController.Instance.PlaySound("KeyPress");
        if (Commands.Count == 0)
        {
            if (Commands.Count == 0)
                commandDisplay.SetUserPrompt("The DELETE key removes the selected command, or most recently entered command.\r\n\r\nNo commands entered! Insert disks to add commands");

            return;
        }

        Debug.Log("Removing command");
        if (selectedCommandIndex.HasValue)
        {
            Commands.RemoveAt(selectedCommandIndex.Value);
            CloseDraftCommandNode(true, true);
            //Up();
        }
        else if (Commands.Count > 0)
        {
            Commands.RemoveAt(Commands.Count - 1);
        }
    }

    public void Execute()
    {
        if (GameManagerController.Instance.State != GameManagerController.GameStates.EnterInstructions)
            return;

        if (Commands.Count == 0)
        {
            commandDisplay.SetUserPrompt("Press Execute after entering commands to upload them to the probe and launch.\r\n\r\nNo commands entered! Insert disks to add commands");
            return;
        }

        if (DraftCommand != null)
            Done();

        Debug.Log("Executing commands");
        GameManagerController.Instance.StartExecution();
        AudioManagerController.Instance.PlaySound("Execute");
        ShipController.Instance.ExecuteCommands(Commands);
    }

    public void Up()
    {
        MoveSelection(-1);
    }

    public void Down()
    {
        MoveSelection(1);
    }

    public void Abort()
    {
        if (GameManagerController.Instance.State != GameManagerController.GameStates.Executing)
            return;

        AudioManagerController.Instance.PlaySound("Abort");
        ShipController.Instance.Stop();
        GameManagerController.Instance.LoadLevel(true);
    }

    private void MoveSelection(int offset)
    {
        if (GameManagerController.Instance.State != GameManagerController.GameStates.EnterInstructions)
            return;

        if (Commands.Count == 0)
        {
            commandDisplay.SetUserPrompt("The UP/DOWN keys can be used so select and modify commands which have already been entered.\r\n\r\nNo commands entered! Insert disks to add commands");
        }

        selectedCommandIndex = selectedCommandIndex.HasValue ? selectedCommandIndex + offset : Commands.Count - 1;

        if (selectedCommandIndex >= Commands.Count)
        {
            if (DraftCommand != null)
                CloseDraftCommandNode();

            return;
        }

        if (selectedCommandIndex < 0)
        {
            selectedCommandIndex = 0;
            return;
        }

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
        if (GameManagerController.Instance.State != GameManagerController.GameStates.EnterInstructions)
            return;

        if (selectedCommandIndex.HasValue)
        {
            Commands.Insert(selectedCommandIndex.Value+1, draftCommand);
            selectedCommandIndex++;
        }
        else
        {
            Commands.Add(draftCommand);
            selectedCommandIndex = Commands.Count - 1;
        }

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

    private void CloseDraftCommandNode(bool clearSelectedCommandIndex = true, bool discarding = false)
    {
        if (clearSelectedCommandIndex)
            selectedCommandIndex = null;

        DraftCommand.State = CommandState.Pending;
        currentNodeInstance?.GetComponent<NodeBase>().DestroySelf(discarding);
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