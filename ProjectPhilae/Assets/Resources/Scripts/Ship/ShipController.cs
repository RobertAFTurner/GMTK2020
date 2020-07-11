using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    // CurrentControl
    // ControlLisy

    [SerializeField]
    private Rigidbody2D rigidbody;

    [SerializeField]
    private float speed;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
            speed += 0.1f;
        else if(Input.GetKeyDown(KeyCode.DownArrow))
            speed -= 0.1f;
    }

    void FixedUpdate()
    {
        rigidbody.AddForce(rigidbody.transform.up * speed);
    }
}
