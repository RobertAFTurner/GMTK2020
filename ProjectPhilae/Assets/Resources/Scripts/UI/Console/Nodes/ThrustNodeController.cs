﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ThrustNodeController : NodeBase<ThrustCommand>
{
    [SerializeField]
    private Slider slider;

    [SerializeField]
    private TMP_InputField input;
    
    public void SetPower()
    {
        command.Power = slider.GetComponent<Slider>().value;
    }

    public void SetDuration()
    {
        command.Duration = string.IsNullOrWhiteSpace(input.text) ? 0f : float.Parse(input.text);
    }
}
