using System.Linq;
using TMPro;
using static RotateCommand;

public class RotateNodeController : NodeBase<RotateCommand>
{
    private TMP_InputField degreeInput;
    private TMP_InputField durationInput;
    private TMP_Dropdown directionInput;

    public void OnEnable()
    {
        degreeInput = this.GetComponentsInChildren<TMP_InputField>().Single(c => c.name.Contains("Degrees"));
        degreeInput.onValueChanged.AddListener(SetDegrees);

        durationInput = this.GetComponentsInChildren<TMP_InputField>().Single(c => c.name.Contains("Duration"));
        durationInput.onValueChanged.AddListener(SetDuration);

        directionInput = this.GetComponentsInChildren<TMP_Dropdown>().Single(c => c.name.Contains("Direction"));
        directionInput.onValueChanged.AddListener(SetDirection);
    }

    private void SetDirection(int index)
    {
        command.Direction = index == 0 ? AngularDirection.Clockwise : AngularDirection.Anticlockwise;
    }

    public void OnDisable()
    {
        degreeInput.onValueChanged.RemoveAllListeners();
        durationInput.onValueChanged.RemoveAllListeners();
        directionInput.onValueChanged.RemoveAllListeners();
    }

    public void SetDuration(string value)
    {
        command.Duration = float.TryParse(value, out var parsedValue) ? parsedValue : 0f;
    }

    public void SetDegrees(string value)
    {
        command.Degrees = float.TryParse(value, out var parsedValue) ? parsedValue : 0f;
    }

    protected override void ApplyCommandToUI()
    {
        degreeInput.text = command.Degrees.ToString();
        durationInput.text = command.Duration.ToString();
        directionInput.value = directionInput.options.IndexOf(directionInput.options.Single(o => o.text == command.Direction.ToString()));
    }
}
