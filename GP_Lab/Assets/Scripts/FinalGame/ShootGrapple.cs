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


    Particle3D grapple;
    Vector3 attachmentPoint;

    private void Start()
    {
        grapple = GetComponent<Particle3D>();

        attachmentPoint = new Vector3(shipTransform.position.x, shipTransform.position.y, shipTransform.position.z);
        grapple.position = attachmentPoint;
        grapple.velocity = Vector3.zero;
        grapple.acceleration = Vector3.zero;
        
    }

    void Update()
    {
        attachmentPoint = new Vector3(shipTransform.position.x, shipTransform.position.y, shipTransform.position.z);
        checkInput();

        checkAndReset();
    }

    void checkInput()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {

            grapple.velocity = ForceGenerator3D.genereateImpulse(shipTransform.gameObject.GetComponentInParent<Particle3D>().velocity, shotStrength); 
        }

        grapple.addForce(ForceGenerator3D.GenerateForce_spring(transform.position, attachmentPoint, spring_resting, spring_stiffness));
        grapple.addForce(ForceGenerator3D.GenerateForce_drag(grapple.velocity, fluidVelocity, fluidDensity, 1, 10.05f));
    }

    void checkAndReset()
    {
        Vector3 diff = transform.position - attachmentPoint;

        if (diff.magnitude > 30f || Input.GetKeyDown(KeyCode.Tab))
        {
            transform.position = attachmentPoint;
            grapple.velocity = Vector3.zero;
            grapple.acceleration = Vector3.zero;
            grapple.position = transform.position;
        }
    }
}
