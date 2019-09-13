using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//With some assitance from brother

public class Particle2D : MonoBehaviour
{
    //Step 1
    public Vector2 position;
    public Vector2 velocity;
    public Vector2 acceleration;
    public float rotation;
    public float angularVelocity;
    public float angularAcceleration;
    public float accelerationValue;

    //Lab02 - Step 1
    public float startingMass;
    float mass, massInv;

    public Transform surfaceTransform;

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

    //Lab02 - Step02

    Vector2 force;

    public void addForce(Vector2 newForce)
    {
        //D'Alembert
        force += newForce;
    }

    void updateAcceleration()
    {
        //Newton 2
        acceleration = force * massInv;

        force.Set(0.0f, 0.0f);
    }

    //Lab 01 - Step 1 cont
    public enum PositionFunction
    {
        PositionEuler,
        PositionKinematic
    }

    public enum RotationFunction
    {
        RotationEuler,
        RotationKinematic
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

    //Step 2

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
        rotation += angularVelocity * dt;

        angularVelocity += angularAcceleration * dt;
    }

    void updateRotKinematic(float dt)
    {
        rotation += angularVelocity * dt + (angularAcceleration * .5f * dt * dt);
        angularVelocity += angularAcceleration * dt;
    }

    void Start()
    {
        setMass(startingMass);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Lab 01 & Lab 02 - Step 3
        if (IntegrationMethod == PositionFunction.PositionEuler)
        {
            updatePosEulerExplicit(Time.fixedDeltaTime);
            updateAcceleration();

            transform.position = position;

            //Rotations
            if (RotationUpdateMethod == RotationFunction.RotationEuler)
            {
                updateRotEulerExplicit(Time.fixedDeltaTime);

                transform.eulerAngles = new Vector3(0f, 0f, rotation);
            }
            else if (RotationUpdateMethod == RotationFunction.RotationKinematic)
            {
                updateRotKinematic(Time.fixedDeltaTime);

                transform.eulerAngles = new Vector3(0f, 0f, rotation);
            }
        }
        else if (IntegrationMethod == PositionFunction.PositionKinematic)
        {
            updatePosKinematic(Time.fixedDeltaTime);
            updateAcceleration();

            transform.position = position;

            //Rotations
            if (RotationUpdateMethod == RotationFunction.RotationEuler)
            {
                updateRotEulerExplicit(Time.fixedDeltaTime);

                transform.eulerAngles = new Vector3(0f, 0f, rotation);

            }
            else if (RotationUpdateMethod == RotationFunction.RotationKinematic)
            {
                updateRotKinematic(Time.fixedDeltaTime);

                transform.eulerAngles = new Vector3(0f, 0f, rotation);

            }
        }



        //Step 4
        //if(MovementType == UpdateFormula.Ocilate)
        //{
        //    acceleration.x = -3f * Mathf.Sin(Time.fixedTime);
        //}
        //else if(MovementType == UpdateFormula.ConstantVelocity)
        //{
        //    acceleration.x = 0;
        //}
        //else if(MovementType == UpdateFormula.ConstantAcceleration)
        //{
        //    acceleration.x = accelerationValue;
        //}

        //Lab 02 - Step 4
        //F_gravity: f = mg
        //Vector2 f_gravity = mass * new Vector2(0.0f, -9.871f);
        //addForce(f_gravity);

        addForce(ForceGenerator.generateForce_Gravity(mass, -9.871f, Vector2.up));
        addForce(ForceGenerator.GenerateForce_normal(ForceGenerator.generateForce_Gravity(mass, -9.871f, Vector2.up), new Vector2(Mathf.Cos(surfaceTransform.rotation.z), Mathf.Sin(surfaceTransform.rotation.z)).normalized));
        addForce(ForceGenerator.GenerateForce_drag(velocity, velocity/10, 1, 1, 10));
        //addForce(ForceGenerator.GenerateForce_sliding(ForceGenerator.generateForce_Gravity(mass, -9.871f, Vector2.up), ForceGenerator.GenerateForce_normal(ForceGenerator.generateForce_Gravity(mass, -9.871f, Vector2.up), new Vector2(Mathf.Cos(surfaceTransform.rotation.z), Mathf.Sin(surfaceTransform.rotation.z)).normalized)));
    }
}
