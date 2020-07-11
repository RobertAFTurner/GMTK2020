using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeBase : MonoBehaviour
{
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
