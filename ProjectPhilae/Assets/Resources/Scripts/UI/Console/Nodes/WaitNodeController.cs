using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaitNodeController : NodeBase<WaitCommand>
{
    private Slider durationSlider;

    public void OnEnable()
    {
        durationSlider = this.GetComponentsInChildren<Slider>().Single(c => c.name.Contains("Duration"));
        durationSlider.onValueChanged.AddListener(SetDuration);
    }

    public void OnDisable()
    {
        durationSlider.onValueChanged.RemoveAllListeners();
    }

    public void SetDuration(float value)
    {
        command.Duration = Mathf.Round(value * 10) / 10;
    }

    protected override void ApplyCommandToUI()
    {
        durationSlider.value = command.Duration;
    }
}
