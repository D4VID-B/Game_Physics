using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipFuel : MonoBehaviour
{
    public float totalFuel = 100f;
    public float fuelLoss = 0.5f;


    void Start()
    {
        //Time.fixedDeltaTime = 1f;
    }

    void FixedUpdate()
    {
        totalFuel -= fuelLoss;
    }
}
