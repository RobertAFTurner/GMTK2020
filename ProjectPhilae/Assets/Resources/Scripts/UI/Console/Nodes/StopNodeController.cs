﻿using System.Linq;
using TMPro;

public class StopNodeController : NodeBase<StopCommand>
{
    private TMP_InputField input;
    
    void OnEnable()
    {
        input = this.GetComponentsInChildren<TMP_InputField>().Single(c => c.name.Contains("Time"));
        input.onValueChanged.AddListener(SetTime);
    }

    public void OnDisable()
    {
        input.onValueChanged.RemoveAllListeners();
    }

    public void SetTime(string value)
    {
        command.Duration = float.TryParse(value, out var parsedValue) ? parsedValue : 0f;
    }
}
