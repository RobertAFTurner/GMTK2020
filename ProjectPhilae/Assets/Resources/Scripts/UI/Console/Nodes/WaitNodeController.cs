using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WaitNodeController : NodeBase<WaitCommand>
{
    [SerializeField]
    private TMP_InputField input;
    
    public void SetDuration()
    {
        command.Duration = string.IsNullOrWhiteSpace(input.text) ? 0f : float.Parse(input.text);
    }
}
