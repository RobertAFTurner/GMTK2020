using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ThrustNodeController : NodeBase
{
    private ThrustCommand command;
    
    [SerializeField]
    private Slider slider;

    [SerializeField]
    private TMP_InputField input;

    public void SetThrustCommand(ThrustCommand thrustCommand)
    {
        command = thrustCommand;
    }

    public void SetPower()
    {
        command.Power = slider.GetComponent<Slider>().value;
    }

    public void SetDuration()
    {
        command.Duration = float.Parse(input.text);
    }
}
