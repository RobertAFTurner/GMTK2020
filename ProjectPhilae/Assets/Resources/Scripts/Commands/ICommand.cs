using UnityEngine;
public interface ICommand
{
  bool ExecuteTillDone(ShipController shipController);
  CommandState State { get; }
}