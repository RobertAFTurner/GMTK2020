using System.Linq;
using TMPro;
public class RotateNodeController : NodeBase<RotateCommand>
{
    private TMP_InputField degreeInput;
    private TMP_InputField durationInput;
    private TMP_InputField directionInput;

    public void OnEnable()
    {
        degreeInput = this.GetComponentsInChildren<TMP_InputField>().Single(c => c.name.Contains("Degrees"));
        degreeInput.onValueChanged.AddListener(SetDegrees);

        durationInput = this.GetComponentsInChildren<TMP_InputField>().Single(c => c.name.Contains("Duration"));
        durationInput.onValueChanged.AddListener(SetDuration);

        directionInput = this.GetComponentsInChildren<TMP_InputField>().Single(c => c.name.Contains("Direction"));
        directionInput.onValueChanged.AddListener(SetDirection);
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

    public void SetDirection(string value)
    {
        command.Direction = float.TryParse(value, out var parsedValue) ? parsedValue : 0f;
    }
}
