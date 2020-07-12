
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

        var levelData = FindObjectOfType<LevelData>();

        int starCount;

        if (score >= levelData.FiveStarScore)
            starCount = 5;
        else if (score <= levelData.OneStarScore)
            starCount = 1;
        else
        {
            var scoreAboveOneStar = score - levelData.OneStarScore;
            var starThreshold = levelData.FiveStarScore - levelData.OneStarScore / 4f;
            starCount = 1 + Mathf.FloorToInt(scoreAboveOneStar / starThreshold);
        }

        GetComponentInChildren<StarDisplay>().DisplayStars(starCount);
    }

    void Start()
    {
        //gameObject.SetActive(false);
    }
}
