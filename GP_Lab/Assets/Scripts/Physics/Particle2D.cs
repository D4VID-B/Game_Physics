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

    public float fluidDensity = 1.225f;
    public Vector2 fluidVelocity = Vector2.zero;
    public float spring_stiffness, spring_resting;

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


    //Lab 3 - Step 1

    float m_Inertia;

    public float radius, i_Radius, o_Radius, height, width, length;

    public float f_Torque;

    Vector2 localCM, globalCM, foreAppPoint;

    public enum Shape_2D
    {
        Disk,   // I = 1/2*m*(r*r)
        Ring,   // I = 1/2*m*(outer_r^2 * inner_r^2)
        Rectangle,  // I = 1/12 * m * (height^2 * width^2)
        Rod     // I = 1/12 * m * (length*length)
    }

    public Shape_2D Shape;

    float calculateMomentOfInertia(Shape_2D shape)
    {
        float MoI = 0;
        switch (shape)
        {
            case Shape_2D.Disk:
                return MoI = 0.5f * mass * (radius * radius);
            case Shape_2D.Ring:
                return MoI = 0.5f * mass * ((o_Radius * o_Radius) * (i_Radius * i_Radius));
            case Shape_2D.Rectangle:
                return MoI = (float)1 / 12 * mass * ((height * height) * (width * width));
            case Shape_2D.Rod:
                return MoI = (float)1 / 12 * mass * (length * length);
        }

        return MoI;
    }

    //Lab 03 - Step 02
    float convertToAcceleration(float inertia) //Update angular Acceleration
    {
        float conversion = 1 / inertia * f_Torque;

        f_Torque = 0f;

        return conversion;
    }

    void addTorque(float torque) //Apply torque
    {
        f_Torque += torque;

        //float momentArm = ; //moment arm = distance between object center of mass & point of force application 
                              // for regular objects of scale 1 moment arm = 0.5
        float appliedForce;
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
    

    //Lab 02 Demo

    public enum FrictionCoef_Static
    {
        AlumSteelStatic, // 0.61
        RubberConcreteDryStatic, // 1.0
        SteelSteelStatic, // 0.74
    }

    public enum FrictionCoef_Kinetic
    {
        AlumSteelKinetic, // 0.47
        RubberConcreteDryKinetic, // 0.8
        SteelSteelKinetic, // 0.57
    }

    public FrictionCoef_Static MaterialType_Static;
    public FrictionCoef_Kinetic MaterialType_Kinetic;
    //Source: https://physics.ucf.edu/~saul/Common/06-Forces/friction-coeffs.gif

    public float getCoeff_Static (FrictionCoef_Static selection)
    {
        float coeff = 1.0f;

        switch(selection)
        {
            case FrictionCoef_Static.AlumSteelStatic:
                return coeff = 0.61f;
            
            case FrictionCoef_Static.RubberConcreteDryStatic:
                return coeff = 1.0f;

            case FrictionCoef_Static.SteelSteelStatic:
                return coeff = 0.74f;

        }

            return coeff;
    }

    public float getCoeff_Kinetic(FrictionCoef_Kinetic selection)
    {
        float coeff = 1.0f;

        switch(selection)
        {
            case FrictionCoef_Kinetic.AlumSteelKinetic:
                return coeff = 0.47f;
            case FrictionCoef_Kinetic.RubberConcreteDryKinetic:
                return coeff = 0.8f;
            case FrictionCoef_Kinetic.SteelSteelKinetic:
                return coeff = 0.57f;
        }

        return coeff;

    }

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
        //Vector2 gravity = mass * new Vector2(0.0f, -9.871f);
        //addForce(f_gravity);

        Vector2 gravity = ForceGenerator.generateForce_Gravity(mass, -9.871f, Vector2.up);
        Vector2 surfaceNormalUnit = new Vector2(Mathf.Sin(surfaceTransform.eulerAngles.z), Mathf.Cos(surfaceTransform.eulerAngles.z));
        Vector2 normal = ForceGenerator.GenerateForce_normal(gravity, surfaceNormalUnit);

                
                                        //******** Block on a slanted surface ********//
        if(gameObject.name == "SlideCube") // Demostrating Gravity, Normal (As Sliding) and Friction forces
        {
            //addForce(gravity);
            //addForce(normal);

            addForce(ForceGenerator.GenerateForce_sliding(gravity, normal));

            //Using Friction (some more help from brother)
            //x = Mass * g * sin()cos() y = mass * g * sin()sin()
            Vector2 fOpposing = new Vector2((mass * -9.871f * Mathf.Sin(surfaceTransform.eulerAngles.z) * Mathf.Cos(surfaceTransform.eulerAngles.z)), (mass * -9.871f * Mathf.Sin(surfaceTransform.eulerAngles.z) * Mathf.Sin(surfaceTransform.eulerAngles.z)));
            addForce(ForceGenerator.GenerateForce_friction(normal, fOpposing, velocity, getCoeff_Static(MaterialType_Static), getCoeff_Kinetic(MaterialType_Kinetic)));

        }




        //********  Cube on a Spring ********//
        if (gameObject.name == "HangCube") // Demonstrating Spring and Drag forces
        {
            addForce(ForceGenerator.GenerateForce_spring(transform.position, surfaceTransform.position, spring_resting, spring_stiffness));
            addForce(ForceGenerator.GenerateForce_drag(velocity, fluidVelocity, fluidDensity, 1, 1.05f)); //Drag coef & object x-section are pre-calculated for a 
            //Velocity is taken fronm the particle properties and is integrated by the script, 
            //while fluid density & velocity are public variables that default to Earth air with no wind
        }
        
    }
}
