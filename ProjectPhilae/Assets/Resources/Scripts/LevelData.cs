using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    public enum Commands
    {
        Thrust,
        Wait,
        Rotate,
        Reverse,
        Stop
    }

    public float FiveStarScore;
    public float OneStarScore;
    public float StartingFuel = 100f;

    public List<Commands> ExcludedCommands;
}
