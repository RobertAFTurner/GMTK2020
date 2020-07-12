
using UnityEngine;

public class PlayerSpawnPointController : MonoBehaviour
{

    [SerializeField] GameObject playerShipPrefab;
    [SerializeField] GameObject leftCap;
    [SerializeField] GameObject rightCap;
    [SerializeField] GameObject rocket;


    // Start is called before the first frame update
    void Start()
    {
        var ship = Instantiate(playerShipPrefab, transform.position, transform.rotation);
    }

    void Update()
    {
        if (ShipController.Instance.State == ShipState.Executing)
        {
            var rightRigidbody = rightCap.GetComponent<Rigidbody2D>();
            rightRigidbody.AddForce(rightRigidbody.transform.right * 4);

            var leftRigidbody = leftCap.GetComponent<Rigidbody2D>();
            leftRigidbody.AddForce(-leftRigidbody.transform.right * 4);

            var rocketRigifBody = rocket.GetComponent<Rigidbody2D>();
            rocketRigifBody.AddForce(-rocketRigifBody.transform.up * 1);
        }
    }

    public void Reset()
    {
            var rightRigidbody = rightCap.GetComponent<Rigidbody2D>();
            rightRigidbody.velocity = Vector2.zero;

            var leftRigidbody = leftCap.GetComponent<Rigidbody2D>();
            leftRigidbody.velocity= Vector2.zero;

            var rocketRigifBody = rocket.GetComponent<Rigidbody2D>();
            rocketRigifBody.AddForce(-rocketRigifBody.transform.up * 1);
    }
}
