using System.Linq;
using TMPro;

public class StopNodeController : NodeBase<StopCommand>
{
    private TMP_InputField input;
    
    void OnEnable()
    {
        input = this.GetComponentsInChildren<TMP_InputField>().Single(c => c.name.Contains("Time"));
        input.onValueChanged.AddListener(SetDuration);
        input.text = command.Duration.ToString();
    }

    public void OnDisable()
    {
        input.onValueChanged.RemoveAllListeners();
    }

    public void SetDuration(string value)
    {
        command.Duration = float.TryParse(value, out var parsedValue) ? parsedValue : 0f;
    }

    protected override void ApplyCommandToUI()
    {
        input.text = command.Duration.ToString();
    }
}
