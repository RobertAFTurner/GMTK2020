using System;
using System.Text;
using TMPro;
using UnityEngine;

public class CommandDisplay : MonoBehaviour
{
    public ConsoleController ConsoleController;
    private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = this.GetComponent<TMPro.TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        var output = $"{string.Join(Environment.NewLine, ConsoleController.Commands)}";
        if (!output.Contains(">"))
            output += "\r\n<color=white>>_</color>";
        text.SetText(output);
    }
}
