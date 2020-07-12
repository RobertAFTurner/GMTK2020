using UnityEngine;

public class WaypointGoalController : MonoBehaviour
{  
    public bool IsCollected { get; set; } 

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Ship")
        {
            AudioManagerController.Instance.PlaySound("PickUp");
            IsCollected = true;
        }
    }
}
