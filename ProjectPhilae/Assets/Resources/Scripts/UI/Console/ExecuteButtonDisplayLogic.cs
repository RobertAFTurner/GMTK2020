using UnityEngine;
using static GameManagerController;

public class ExecuteButtonDisplayLogic : MonoBehaviour
{
    public GameObject Button;

    void Update()
    {
        Button.SetActive(Instance.State == GameStates.EnterInstructions);
    }
}
