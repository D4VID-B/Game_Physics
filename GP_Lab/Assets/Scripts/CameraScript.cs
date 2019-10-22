using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform playerShip;
    private Vector3 playerShipStartPosition;
    void Start()
    {
        playerShipStartPosition = playerShip.position;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(this.transform.position.y >= playerShipStartPosition.y)
        {
            this.transform.position = new Vector3(playerShip.position.x, playerShip.position.y, this.transform.position.z);
        }
        else
        {
            this.transform.position = new Vector3(playerShip.position.x, playerShipStartPosition.y, this.transform.position.z);
        }
        */
        this.transform.position = new Vector3(playerShip.position.x, this.transform.position.y, this.transform.position.z);


    }
}
