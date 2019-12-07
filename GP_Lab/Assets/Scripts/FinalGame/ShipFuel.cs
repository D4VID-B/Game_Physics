using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipFuel : MonoBehaviour
{
    public float totalFuel = 100f;
    float shipFuel;
    public float fuelLoss = 0.5f;

    public Text fuelText;

    void Start()
    {
        shipFuel = totalFuel;
    }

    void FixedUpdate()
    {
        totalFuel -= fuelLoss;

        fuelText.text = totalFuel.ToString();

        if(totalFuel < 0)
        {
            totalFuel = 0;
            GetComponent<Particle3D>().disableThrust();
            GameObject.Find("GameManager").GetComponent<SwitchScene>().Switch();
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
