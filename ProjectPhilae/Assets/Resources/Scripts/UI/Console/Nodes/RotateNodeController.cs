using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static RotateCommand;

public class RotateNodeController : NodeBase<RotateCommand>
{
    private TMP_Dropdown directionInput;
    private Slider degreesSlider;
    private Slider durationSlider;

    public void OnEnable()
    {
        durationSlider = this.GetComponentsInChildren<Slider>().Single(c => c.name.Contains("Duration"));
        durationSlider.onValueChanged.AddListener(SetDuration);

        degreesSlider = this.GetComponentsInChildren<Slider>().Single(c => c.name.Contains("Degree"));
        degreesSlider.onValueChanged.AddListener(SetDegrees);

        directionInput = this.GetComponentsInChildren<TMP_Dropdown>().Single(c => c.name.Contains("Direction"));
        directionInput.onValueChanged.AddListener(SetDirection);
    }

    public void OnDisable()
    {
        durationSlider.onValueChanged.RemoveAllListeners();
        degreesSlider.onValueChanged.RemoveAllListeners();
        directionInput.onValueChanged.RemoveAllListeners();
    }

    public void SetDirection(int index)
    {
        command.Direction = index == 0 ? AngularDirection.Clockwise : AngularDirection.Anticlockwise;
    }

    public void SetDuration(float value)
    {
        command.Duration = Mathf.Round(value*10)/10;
    }

    public void SetDegrees(float value)
    {
        command.Degrees = Mathf.Round(value);
    }

    protected override void ApplyCommandToUI()
    {
        degreesSlider.value = command.Degrees;
        durationSlider.value = command.Duration;
        directionInput.value = directionInput.options.IndexOf(directionInput.options.Single(o => o.text == command.Direction.ToString()));
    }
}
