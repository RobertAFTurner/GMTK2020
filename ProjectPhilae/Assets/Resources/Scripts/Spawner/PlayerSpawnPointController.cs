﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPointController : MonoBehaviour
{

    [SerializeField]
    GameObject playerShipPrefab;

    [SerializeField]
    ConsoleController consoleController;

    // Start is called before the first frame update
    void Start()
    {
        var ship = Instantiate(playerShipPrefab, transform.position, Quaternion.identity);
        consoleController.SetShip(ship.GetComponent<ShipController>());
    }
}
