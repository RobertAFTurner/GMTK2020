
using UnityEngine;

public class GUIManagerController : Singleton<GUIManagerController>
{
    public static GUIManagerController Instance => (GUIManagerController)instance;

    [SerializeField] WinPanelController winPanel;

    public void HideWinPanel()
    {
        winPanel.gameObject.SetActive(false);
    }

    public void ShowWinPanel(bool active, int score, float multiplier, float fuel)
    {
        winPanel.SetScore(score, multiplier, fuel);
        winPanel.gameObject.SetActive(active);
    }

}
