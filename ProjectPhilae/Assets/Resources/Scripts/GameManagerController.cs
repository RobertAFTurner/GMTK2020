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

    private int currentLevel;

    void Start()
    {
        currentLevel = 1;
        Load();
    }

    private void Load()
    {
        State = GameStates.EnterInstructions;
        waypoints = new List<GameObject>();
        waypoints.AddRange(GameObject.FindGameObjectsWithTag("Waypoint"));
    }

    public void StartExecution() => State = GameStates.Executing;

    void Update()
    {
        if (State == GameStates.Executing)
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
                State = GameStates.Win; 
            }
        }

        if(State == GameStates.Win)
        {
            currentLevel++;
            SceneManager.LoadScene($"Level_{currentLevel}", LoadSceneMode.Single);
            Load();
        }
    }
}
