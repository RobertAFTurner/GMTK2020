using UnityEngine;
public abstract class Command
{
    public abstract bool ExecuteTillDone(ShipController shipController);
    public CommandState State { get; protected set; }
    
    protected float startTime;
    public float Duration;

    protected abstract string GetDisplayText();

    public override string ToString()
    {
        var displayText = GetDisplayText();

        if (State == CommandState.InProgress)
            return $">{displayText} ({(Time.time - startTime):.0}/{Duration:.0})";

        if (State == CommandState.UnableToComplete)
            return $"{displayText} FAILED - NOT ENOUGH FUEL";

        return displayText;
    }
}