using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManagerController : Singleton<GameManagerController>
{
    public static GameManagerController Instance => (GameManagerController)instance;

    public enum GameStates
    {
        EnterInstructions,
        Executing,
        Win,
        Lose
    }

    public GameStates State = GameStates.EnterInstructions;

    private List<Command> prevCommands;

    private List<GameObject> waypoints;
    private bool allWaypointsCollected;
    private GameObject landingPad;
    private bool levelHasPad;
    private int currentLevel;
    private bool reload = true;

    void Start()
    {
        currentLevel = 1;
        Load();        
    }

    private void Load()
    {
        State = GameStates.EnterInstructions;
        allWaypointsCollected = false;
        levelHasPad = false;

        waypoints = new List<GameObject>();
        waypoints.AddRange(GameObject.FindGameObjectsWithTag("Waypoint"));

        var pad = GameObject.Find("LandingCollider");
        if (pad != null)
        {
            landingPad = pad;
            levelHasPad = true;
        }
    }

    public void StartExecution() => State = GameStates.Executing;

    void Update()
    {
        if(reload)
        {
            // Because the Game Manager persists between levels we have to wait until the first frame after loading to "Start" the Game Manaager.
            StartCoroutine(LateLoad());
            reload = false;
        }

        if (State == GameStates.Executing)
        {
            CheckForWaypoints();
            CheckForLandingPad();
        }
        else if (State == GameStates.Win)
        {
            ShipController.Instance.Stop();
            GUIManagerController.Instance.ShowWinPanel(true);      
        }
    }

    private IEnumerator LateLoad()
    {
        State = GameStates.EnterInstructions;
        yield return new WaitForSeconds(0.25f);
        Load();
    } 

    private void CheckForWaypoints()
    {
        if (waypoints.Any())
        {
            var hitWaypoint = waypoints.FirstOrDefault(waypoint => waypoint.GetComponent<WaypointGoalController>().IsCollected);

            if (hitWaypoint != null)
            {
                waypoints.Remove(hitWaypoint);
                Destroy(hitWaypoint);
            }
        }
        else
        {
            allWaypointsCollected = true;
        }
    }

    private void CheckForLandingPad()
    {
        if (levelHasPad)
        {
            if (allWaypointsCollected &&
               landingPad.GetComponent<LandingPadController>().PlayerLanded)
            {
                State = GameStates.Win;
            }
        }
        else if (allWaypointsCollected)
        {
            State = GameStates.Win;
        }
    }

    public void LoadLevel(bool keepCommands = false)
    {
        GUIManagerController.Instance.ShowWinPanel(false);      
        Debug.Log($"Load level, keep commands: {keepCommands}");
        if (keepCommands)
        {
            var consoleController = GameObject.Find("Console").GetComponent<ConsoleController>();
            prevCommands = consoleController.Commands;
            prevCommands.ForEach(command => command.State = CommandState.Pending);
        }

        SceneManager.LoadScene($"Level_{currentLevel}", LoadSceneMode.Single);
        
        // Can't just call Load here because it will not re-set the waypoints correctly.
        reload = true;
    }

    public void LoadNextLevel()
    {
        if(prevCommands != null && prevCommands.Any())
        {
            prevCommands.Clear();
            prevCommands = null;
        } 

        currentLevel++;
        LoadLevel();
    }

    public List<Command> GetPreviousCommands() 
    {
        return prevCommands;
    }
}
