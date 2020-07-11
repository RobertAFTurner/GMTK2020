using System;
using TMPro;
using UnityEngine;

public class ShipStatusDisplay : MonoBehaviour
{
    private const string template =
        @"Probe Status:

STATE: {0}
FUEL:  {1}/100
";

    private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = this.GetComponent<TMPro.TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        var output = string.Format(template, GameManagerController.Instance.State, ShipController.Instance.fuel);

        text.SetText(output);
    }
}
