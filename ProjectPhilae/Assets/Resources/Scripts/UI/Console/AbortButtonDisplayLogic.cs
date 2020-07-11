using UnityEngine;
using UnityEngine.UI;
using static GameManagerController;

public class AbortButtonDisplayLogic : MonoBehaviour
{
    public GameObject Button;

    void Update()
    {
        Button.SetActive(Instance.State != GameStates.EnterInstructions);
    }
}
