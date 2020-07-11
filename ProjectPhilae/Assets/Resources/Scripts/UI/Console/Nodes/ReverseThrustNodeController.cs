using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ReverseThrustNodeController : NodeBase<ReverseThrustCommand>
{
    private Slider powerSlider;
    private Slider durationSlider;

    public void OnEnable()
    {
        durationSlider = this.GetComponentsInChildren<Slider>().Single(c => c.name.Contains("Duration"));
        durationSlider.onValueChanged.AddListener(SetDuration);

        powerSlider = this.GetComponentsInChildren<Slider>().Single(c => c.name.Contains("Power"));
        powerSlider.onValueChanged.AddListener(SetPower);
    }

    public void OnDisable()
    {
        durationSlider.onValueChanged.RemoveAllListeners();
        powerSlider.onValueChanged.RemoveAllListeners();
    }

    private void SetPower(float power)
    {
        command.Power = Mathf.Round(power);
    }

    public void SetDuration(float value)
    {
        command.Duration = Mathf.Round(value * 10) / 10;
    }

    protected override void ApplyCommandToUI()
    {
        powerSlider.value = command.Power;
        durationSlider.value = command.Duration;
    }
}
