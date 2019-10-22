using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle3D : MonoBehaviour
{
    [Header("Position")]
    public Vector3 position;
    public Vector3 velocity;
    public Vector3 acceleration;
    Vector3 force;

    [Header("Rotation")]
    public Quaternion rotation;
    public Vector3 angularVelocity;
    public Vector3 angularAcceleration;
    public Vector3 torque;

    //Lab02 - Step 1
    [Header("Mass")]
    public float startingMass;
    float mass, massInv;


    

    public void setMass(float newMass)
    {
        mass = newMass >= 0 ? newMass : 0.0f; //Option 01
        mass = Mathf.Max(0.0f, newMass);      //Option 02
        massInv = mass > 0.0f ? 1.0f / mass : 0.0f;
    }

    public float getMass()
    {
        return mass;
    }

    public enum PositionFunction
    {
        PositionEuler,
        PositionKinematic
    }

    public enum RotationFunction
    {
        RotationEuler
    }

    public enum UpdateFormula
    {
        Ocilate,
        ConstantVelocity,
        ConstantAcceleration,
        ZERO_MOVE
    }

    public PositionFunction IntegrationMethod;
    public RotationFunction RotationUpdateMethod;
    public UpdateFormula MovementType;
    

    //Lab 01 - Step 2

    void updatePosEulerExplicit(float dt)
    {
        //x(t+dt) = x(t) + v(t)dt
        //Euler:
        //F(t+dt) = F(t) + f(t)dt
        //               + (df/dt)dt

        position += velocity * dt;

        //v(t+dt) = v(t) + a(t)dt
        velocity += acceleration * dt;
    }

    void updatePosKinematic(float dt)
    {
        position += velocity * dt + (acceleration * .5f) * dt * dt;
        velocity += acceleration * dt;
    }

    void updateRotEulerExplicit(float dt)
    {
        
    }

    public Quaternion multiplyQuatNum(Quaternion quaternion, float scalar)
    {
        return quaternion;
    }

    public Quaternion multiplyQuatVector(Quaternion quaternion, Vector3 vector)
    {

        return quaternion;
    }

    void Start()
    {
        position = this.transform.position;
        setMass(startingMass);
    }

    void FixedUpdate()
    {
        //Lab 01 & Lab 02 - Step 3
        if (IntegrationMethod == PositionFunction.PositionEuler)
        {
            updatePosEulerExplicit(Time.fixedDeltaTime);

            transform.position = position;

            //Rotations
            //if (RotationUpdateMethod == RotationFunction.RotationEuler)
            //{
            //    updateRotEulerExplicit(Time.fixedDeltaTime);

            //        transform.eulerAngles = new Vector3(0f, 0f, rotation);
                
            //}
            
        }
        else if (IntegrationMethod == PositionFunction.PositionKinematic)
        {
            updatePosKinematic(Time.fixedDeltaTime);

            transform.position = position;

            //Rotations
            //if (RotationUpdateMethod == RotationFunction.RotationEuler)
            //{
            //    updateRotEulerExplicit(Time.fixedDeltaTime);

            //    transform.eulerAngles = new Vector3(0f, 0f, rotation);
                
            //}
            
        }

    }
}
