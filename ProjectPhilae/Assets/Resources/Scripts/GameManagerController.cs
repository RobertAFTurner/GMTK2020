using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerController : Singleton<GameManagerController>
{
    public static GameManagerController Instance => (GameManagerController)instance;

    private enum GameStates
    {
        EnterInstructions,
        Executing,
        Win,
        Lose
    }

    GameStates state = GameStates.EnterInstructions;

    private List<GameObject> waypoints;

    void Start()
    {
        waypoints = new List<GameObject>();
        waypoints.AddRange(GameObject.FindGameObjectsWithTag("Waypoint"));
    }

    public void StartExecution() => state = GameStates.Executing;

    void Update()
    {
        if (state == GameStates.Executing)
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
                state = GameStates.Win;
            }
        }

        if(state == GameStates.Win)
        {
            SceneManager.LoadScene("Main", LoadSceneMode.Single);
        }
    }
}
