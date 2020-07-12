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
        var fuelText = ShipController.Instance.Fuel <= 30f
            ? $"<color=red>{ShipController.Instance.Fuel:###}</color>"
            : $"{ShipController.Instance.Fuel:###}";

        text.SetText($"PROBE STATUS:\r\n{GameManagerController.Instance.State}\r\n\r\nFUEL:  {fuelText}/100");
    }
}
