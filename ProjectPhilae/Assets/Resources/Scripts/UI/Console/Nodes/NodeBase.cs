using UnityEngine;

public abstract class NodeBase : MonoBehaviour
{
    private bool selfDestructing = false;
    private float originalY;

    public void DestroySelf()
    {
        originalY = this.transform.position.y;
        scale = this.transform.localScale;
        selfDestructing = true;
    }

    public void Update()
    {
        if (selfDestructing)
        {
            this.gameObject.transform.Translate(0, Time.deltaTime*200, 0);
            this.gameObject.transform.localScale = scale;
            if (this.transform.position.y > originalY + 500)
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
