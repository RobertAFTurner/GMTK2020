using System.Linq;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ThrustNodeController : NodeBase<ThrustCommand>
{
    private Slider slider;
    private TMP_InputField input;
    
    public void OnEnable()
    {
        input = this.GetComponentsInChildren<TMP_InputField>().Single(c => c.name.Contains("Duration"));
        input.onValueChanged.AddListener(SetDuration);
        
        slider = this.GetComponentsInChildren<Slider>().Single(c => c.name.Contains("Power"));
        slider.onValueChanged.AddListener(SetPower);
    }

    public void OnDisable()
    {
        input.onValueChanged.RemoveAllListeners();
    }

    private void SetPower(float power)
    {
        command.Power = power;
    }

    public void SetDuration(string value)
    {
        command.Duration = float.TryParse(value, out var parsedValue) ? parsedValue : 0f;
    }
}
