
using System.Linq;
using TMPro;
using UnityEngine;

public class WinPanelController : MonoBehaviour // Singleton<WinPanelController>
{
    //public static WinPanelController Instance => (WinPanelController)instance;

    public void SetScore(int score, float multiplier, float fuel)
    {
        var texts = GetComponentsInChildren<TMP_Text>();

        texts.Single(t => t.gameObject.name.Contains("ScoreDisplay")).text = $"You scored {score} points!";
        texts.Single(t => t.gameObject.name.Contains("ScoreCalc")).text = $"Fuel multiplier: {fuel:###}        Speed multiplier: {multiplier:###}";
    }

    void Start()
    {
        //gameObject.SetActive(false);
    }
}
