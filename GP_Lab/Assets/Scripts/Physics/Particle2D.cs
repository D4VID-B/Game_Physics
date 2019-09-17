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
    //Lab 02 Demo

    public enum FrictionCoef
    {
        AlumAlumStatic,
        AlumAlumSliding,
        LeatherIronStatic,
        LeatherIronSliding,
        SteelSteelStatic,
        SteelSteelSliding

    }

    public PositionFunction IntegrationMethod;
    public RotationFunction RotationUpdateMethod;
    public UpdateFormula MovementType;
    public FrictionCoef MaterialType;

    public float getCoeff (FrictionCoef selection)
    {
        float coeff = 0;

        switch(selection)
        {
            case FrictionCoef.AlumAlumStatic:
                return coeff = 1.20f;
            case FrictionCoef.AlumAlumSliding:
                return coeff = 1.4f;
            case FrictionCoef.LeatherIronStatic:
                return coeff = 0.6f;

            case FrictionCoef.LeatherIronSliding:
                return coeff = 0.56f;

            case FrictionCoef.SteelSteelStatic:
                return coeff = 0.65f;

            case FrictionCoef.SteelSteelSliding:
                return coeff = 0.42f;

        }

            return coeff;
    }

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

        Vector2 gravity = ForceGenerator.generateForce_Gravity(mass, -9.871f, Vector2.up);
        Vector2 normal = ForceGenerator.GenerateForce_normal(ForceGenerator.generateForce_Gravity(mass, -9.871f, Vector2.up), new Vector2(Mathf.Cos(surfaceTransform.rotation.z), Mathf.Sin(surfaceTransform.rotation.z)).normalized);
        
        //Always there
        addForce(ForceGenerator.generateForce_Gravity(mass, -9.871f, Vector2.up));

                                        //******** Block on a slanted surface ********//
        if(gameObject.name == "SlideCube")
        {
            //addForce(ForceGenerator.GenerateForce_normal(ForceGenerator.generateForce_Gravity(mass, -9.871f, Vector2.up), new Vector2(Mathf.Cos(surfaceTransform.rotation.z), Mathf.Sin(surfaceTransform.rotation.z)).normalized));
            //addForce(ForceGenerator.GenerateForce_drag(velocity, velocity/10, 1, 1, 10));

            //Using Friction (some more help from brother)
            addForce(ForceGenerator.GenerateForce_friction(normal, new Vector2(mass * -9.871f * .5f, mass * -9.871f * .5f), Vector2.zero, getCoeff(FrictionCoef.AlumAlumStatic), getCoeff(MaterialType)));

            //Using Sliding force
            addForce(ForceGenerator.GenerateForce_sliding(ForceGenerator.generateForce_Gravity(mass, -9.871f, Vector2.up), normal));
        }
        



                                        //********  Cube on a Spring ********//
        if(gameObject.name == "HangCube")
        {
            addForce(ForceGenerator.GenerateForce_spring(transform.position, surfaceTransform.position, 1f, 0.5f));
        }
        
    }
}
