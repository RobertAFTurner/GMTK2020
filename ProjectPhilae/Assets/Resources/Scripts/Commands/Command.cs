using UnityEngine;
public abstract class Command
{
    public Command(float duration)
    {
        Duration = duration;
    }

    public abstract bool ExecuteTillDone(ShipController shipController);
    public CommandState State { get; set; }
    
    protected float startTime;
    public float Duration;

    protected abstract string GetDisplayText();

    public override string ToString()
    {
        var displayText = GetDisplayText();

        if (State == CommandState.Editing)
            return $"<color=white>>{displayText}</color>";

        if (State == CommandState.InProgress)
            return $"><color=#B7FF92>{displayText} ({(Time.time - startTime):.0}/{Duration:.0})</color>";

        if (State == CommandState.UnableToComplete)
            return $"{displayText} <color=red>FAILED - NOT ENOUGH FUEL</color>";

        return displayText;
    }
}