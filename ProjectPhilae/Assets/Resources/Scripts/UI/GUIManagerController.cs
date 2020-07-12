
using UnityEngine;

public class GUIManagerController : Singleton<GUIManagerController>
{
    public static GUIManagerController Instance => (GUIManagerController)instance;

    [SerializeField] GameObject winPanel;

    public void ShowWinPanel(bool active)
    {
        winPanel.SetActive(active);
    }

}
