using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipFuel : MonoBehaviour
{
    public float totalFuel = 100f;
    float shipFuel;
    public float fuelLoss = 0.5f;


    void Start()
    {
        shipFuel = totalFuel;
    }

    void FixedUpdate()
    {
        totalFuel -= fuelLoss;
        if(totalFuel < 0)
        {
            totalFuel = 0;
            //GetComponent<Particle3D>().disableThrust();
        }
    }

    public void addFuel(float amount)
    {
        totalFuel += amount;

        if(totalFuel > shipFuel)
        {
            totalFuel = shipFuel;
        }
    }
}
