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
        var status = GameManagerController.Instance.State.ToString();

        if (GameManagerController.Instance.State == GameManagerController.GameStates.Executing &&
            ShipController.Instance.State == ShipState.OutOfControl)
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

        var fuelText = ShipController.Instance.Fuel <= 30f
            ? $"<color=red>{ShipController.Instance.Fuel:###}</color>"
            : $"{ShipController.Instance.Fuel:###}";

        text.SetText($"PROBE STATUS:\r\n{status}\r\n\r\nFUEL:  {fuelText}/{ShipController.Instance.StartingFuel}");
    }
}
