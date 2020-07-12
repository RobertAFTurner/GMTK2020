using UnityEngine;

public abstract class NodeBase : MonoBehaviour
{
    private bool selfDestructing = false;
    private Vector3 originalPosition;
    private Vector3 targetPosition;

    public void DestroySelf(bool discard = false)
    {
        originalPosition = this.transform.position;
        targetPosition = this.transform.position + (discard ? Vector3.right * 500 : Vector3.up * 500);
        selfDestructing = true;
    }

    public void Update()
    {
        if (selfDestructing)
        {
            this.transform.position =
                Vector3.MoveTowards(this.transform.position, targetPosition, Time.deltaTime * 500);
            if ((targetPosition - this.transform.position).magnitude < 10f)
            {
                selfDestructing = false;
                Destroy(gameObject);
            }
        }
    }
}

public abstract class NodeBase<T> : NodeBase where T : Command
{
    protected T command;

    public void SetCommand(T command)
    {
        this.command = command;
        ApplyCommandToUI();
    }

    protected abstract void ApplyCommandToUI();
}
