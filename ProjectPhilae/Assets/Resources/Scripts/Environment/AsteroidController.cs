using UnityEngine;

public class AsteroidController : MonoBehaviour
{    
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 0.01f));
    }
}
