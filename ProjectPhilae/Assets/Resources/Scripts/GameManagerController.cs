using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private List<GameObject> waypoints;
    private bool allWaypointsCollected;
    private GameObject landingPad;
    private bool levelHasPad;
    private int currentLevel;

    void Start()
    {
        currentLevel = 1;
        Load();
    }

    private void Load()
    {
        State = GameStates.EnterInstructions;
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
        if (State == GameStates.Executing)
        {
            CheckForWaypoints();
            CheckForLandingPad();
        }
        else if (State == GameStates.Win)
        {
            LoadNextLevel();
        }
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

    private void LoadNextLevel()
    {
        currentLevel++;
        SceneManager.LoadScene($"Level_{currentLevel}", LoadSceneMode.Single);
        Load();
    }
}
