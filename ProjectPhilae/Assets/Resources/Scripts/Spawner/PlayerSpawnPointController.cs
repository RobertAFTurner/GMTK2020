using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPointController : MonoBehaviour
{

    [SerializeField]
    GameObject playerShip;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(playerShip, transform.position, Quaternion.identity);
    }
}
