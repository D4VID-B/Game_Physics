using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInput();
    }


    void PlayerInput()
    {
        //Yaw control (isnt it pitch though? yaw would be on the Y axis which just pointlessly spins it)
        if(Input.GetKeyDown(KeyCode.A))
        {

        }
        if(Input.GetKeyDown(KeyCode.D))
        {

        }

        //Elevation control
        if(Input.GetKeyDown(KeyCode.S))
        {

        }
        if(Input.GetKeyDown(KeyCode.W))
        {

        }

    }
}
