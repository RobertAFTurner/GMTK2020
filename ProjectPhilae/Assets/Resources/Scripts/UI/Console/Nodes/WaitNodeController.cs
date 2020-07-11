using System.Linq;
using TMPro;
public class WaitNodeController : NodeBase<WaitCommand>
{
    private TMP_InputField input;

    public void OnEnable()
    {
        input = this.GetComponentsInChildren<TMP_InputField>().Single(c => c.name.Contains("Duration"));
        input.onValueChanged.AddListener(SetDuration);
    }

    public void OnDisable()
    {
        input.onValueChanged.RemoveAllListeners();
    }

    public void SetDuration(string value)
    {
        command.Duration = float.TryParse(value, out var parsedValue) ? parsedValue : 0f;
    }
}
