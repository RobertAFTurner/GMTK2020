using UnityEngine;

public abstract class NodeBase : MonoBehaviour
{
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}

public abstract class NodeBase<T> : NodeBase where T : ICommand
{
    protected T command;

    public void SetCommand(T command)
    {
        this.command = command;
    }
}
