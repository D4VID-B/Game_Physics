using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//With some assitance from brother

public class Particle2D : MonoBehaviour
{
    //Step 1
    //Hi
    public Vector2 position;
    public Vector2 velocity;
    public Vector2 acceleration;
    public float rotation;
    public float angularVelocity;
    public float angularAcceleration;

    public float accelerationValue;

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
        ConstantAcceleration
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
        position += velocity * dt + (acceleration*.5f)*dt*dt;
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



    // Update is called once per frame
    void FixedUpdate()
    {
        //Step 3
        if(IntegrationMethod == PositionFunction.PositionEuler)
        {
            updatePosEulerExplicit(Time.fixedDeltaTime);
            transform.position = position;

            if(RotationUpdateMethod == RotationFunction.RotationEuler)
            {
                updateRotEulerExplicit(Time.fixedDeltaTime);

                transform.eulerAngles = new Vector3 (0f, 0f, rotation);
            }
            else if(RotationUpdateMethod == RotationFunction.RotationKinematic)
            {
                updateRotKinematic(Time.fixedDeltaTime);

                transform.eulerAngles = new Vector3(0f, 0f, rotation);
            }
        }
        else if(IntegrationMethod == PositionFunction.PositionKinematic)
        {
            updatePosKinematic(Time.fixedDeltaTime);
            transform.position = position;

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
        if(MovementType == UpdateFormula.Ocilate)
        {
            acceleration.x = -3f * Mathf.Sin(Time.fixedTime);
        }
        else if(MovementType == UpdateFormula.ConstantVelocity)
        {
            acceleration.x = 0;
        }
        else if(MovementType == UpdateFormula.ConstantAcceleration)
        {
            acceleration.x = accelerationValue;
        }


    }
}
