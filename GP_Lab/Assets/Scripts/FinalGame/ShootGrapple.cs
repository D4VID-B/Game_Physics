using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootGrapple : MonoBehaviour
{
    public float fluidDensity = 1.225f;
    public Vector3 fluidVelocity = Vector3.zero;
    public float dragCoefficient, objectAreaXSection;
    public float spring_stiffness, spring_resting;
    public float shotStrength = 10f;
    public Transform shipTransform;

    void Update()
    {
        checkInput();
    }

    void checkInput()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Particle3D>().addForce( ForceGenerator3D.genereateImpulse(new Vector3(0f, 0f, 10f), shotStrength));
        }

        GetComponent<Particle3D>().addForce(ForceGenerator3D.GenerateForce_spring(transform.position, shipTransform.position, spring_resting, spring_stiffness));
        GetComponent<Particle3D>().addForce(ForceGenerator3D.GenerateForce_drag(GetComponent<Particle3D>().velocity, fluidVelocity, fluidDensity, 1, 1.05f));
    }


}
