using UnityEngine;

public class LandingPadController : MonoBehaviour
{
    public bool PlayerLanded { get; set; }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "ShipLander")
        {
            PlayerLanded = true;
        }
    }
}
