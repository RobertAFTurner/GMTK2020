using System;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using Random = System.Random;

public class IntroText : MonoBehaviour
{
    private ConsoleController consoleController;
    private TextMeshProUGUI text;
    private bool firstCommandEntered;
    private int cursor;
    private float tick;
    private string introText;
    private string hint;
    private bool hintShown;

    // Start is called before the first frame update
    void Start()
    {
        cursor = 0;
        tick = Time.time;
        hintShown = false;
        firstCommandEntered = consoleController?.Commands?.Count==0;

        var hints = new string[]
        {
            "Fuel is limited, watch the dial at the top of this console",
            "Speed and fuel consumption determine your score",
            "Coming to an exact stop burns a lot of fuel",
            "You can remove, edit or insert commands by using the arrow keys to select a command to edit",
            "Keyboard controls are available. Try the : T, W, R, or DELETE, ENTER, UP and DOWN keys",
        };

        hint = $"<color=yellow>HINT: {hints[new Random().Next(0, hints.Length - 1)]}";

        consoleController = FindObjectOfType<ConsoleController>();
        text = this.GetComponent<TMPro.TextMeshProUGUI>();

        introText =
            $"Connection to probe established...\n\nHi human! Where shall we go today?\n\n";
    }

    void OnEnable()
    {
        Start();
    }

// Update is called once per frame
    void Update()
    {
        if (tick < 0.5)
        {
            tick = Time.time;
        }

        if (consoleController.Commands.Count > 0)
        {
            firstCommandEntered = true;
            text.text = string.Empty;
        }

        if (!firstCommandEntered)
        {
            if (Time.time > tick + 0.02f)
            {
                this.cursor++;
                tick = Time.time;
                if (cursor <= introText.Length)
                {
                    var scrollingText = introText.Substring(0, Math.Min(cursor, introText.Length));
                    text.text = scrollingText;
                    var lastChar = scrollingText.LastOrDefault();
                    if (lastChar != default(char) && char.IsPunctuation(lastChar))
                        tick += 0.1f;
                }
                else if (!hintShown)
                {
                    // Pause
                    tick += 1f;
                    hintShown = true;
                    cursor += 13;
                    introText += hint;
                }
            }
        }
    }
}
