using System;
using System.Text;
using TMPro;
using UnityEngine;

public class CommandDisplay : MonoBehaviour
{
    public ConsoleController ConsoleController;
    private TextMeshProUGUI text;
    private string prompt;

    // Start is called before the first frame update
    void Start()
    {
        text = this.GetComponent<TMPro.TextMeshProUGUI>();
    }

    public void SetUserPrompt(string promptText)
    {
        prompt = promptText;
    }

    // Update is called once per frame
    void Update()
    {
        if (ConsoleController.Commands.Count > 0)
        {
            prompt = null;
        }

        var output = $"{string.Join(Environment.NewLine, ConsoleController.Commands)}";

        if (!string.IsNullOrWhiteSpace(prompt))
            output += $"\r\n<color=yellow>{prompt}</color>\r\n<color=white>>_</color>";

        else if (string.IsNullOrWhiteSpace(output))
            output += "\r\n<color=yellow>Insert disks to add commands</color>\r\n<color=white>>_</color>";

        else if (!output.Contains(">"))
            output += "\r\n<color=white>>_</color>";

        text.SetText(output);
    }
}
