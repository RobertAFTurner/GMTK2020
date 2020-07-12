using System;
using TMPro;
using UnityEngine;

public class ShipStatusDisplay : MonoBehaviour
{
    private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = this.GetComponent<TMPro.TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        string status = GameManagerController.Instance.State.ToString();

        if (GameManagerController.Instance.State == GameManagerController.GameStates.Executing &&
            ShipController.Instance.currentlyExecutingCommand == null)
        {
            if (Mathf.FloorToInt(Time.time) % 2 != 0)
            {
                status = "<color=red>OutOfControl!</color>";
            }
            else
            {
                status = "";
            }
        }

        var fuelText = ShipController.Instance.fuel <= 30f
            ? $"<color=red>{ShipController.Instance.fuel:###}</color>"
            : $"{ShipController.Instance.fuel:###}";

        text.SetText($"PROBE STATUS:\r\n{status}\r\n\r\nFUEL:  {fuelText}/100");
    }
}
