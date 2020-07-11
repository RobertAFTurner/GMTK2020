using UnityEngine;
using UnityEngine.UI;
using static GameManagerController;

public class AbortButtonLogic : MonoBehaviour
{
    public GameObject Button;

    void OnEnable()
    {
        Button.GetComponent<Button>().onClick.AddListener(() =>
        {
            ShipController.Instance.Stop();
            GameManagerController.Instance.LoadLevel(true);
        });
    }

    void OnDisable()
    {
        Button.GetComponent<Button>().onClick.RemoveAllListeners();
    }

    void Update()
    {
        Button.SetActive(Instance.State != GameStates.EnterInstructions);
    }
}
