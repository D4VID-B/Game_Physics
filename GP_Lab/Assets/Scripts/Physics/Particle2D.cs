using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//With some assitance from brother

public class Particle2D : MonoBehaviour
{
    //SHIP SHIT
    [Header("Ship Properties")]
    public float elevationThrust;
    public float lateralThrust;
    public bool SHIP_MODE;          // COLTON ADDED THIS FOR SHIP SCRIPT

    //Step 1
    [Header("Position")]
    public Vector2 position;
    public Vector2 velocity;
    public Vector2 acceleration;
    Vector2 force;

    [Header("Rotation")]
    public float rotation;
    public float angularVelocity;
    public float angularAcceleration;
    public float accelerationValue;

    //Lab02 - Step 1
    [Header("Mass")]
    public float startingMass;
    float mass, massInv;

    [Header("Torque")]
    public float f_Torque;
    public Vector2 t_Position, t_Force;


    [Header("Shape Properties")]
    public float radius;
    public float i_Radius;
    public float o_Radius;
    public float height;
    public float width;
    public float length;

    [Header("Coefficients, Density, and Inertia")]
    public float fluidDensity = 1.225f;
    public Vector2 fluidVelocity = Vector2.zero;
    public float dragCoefficient, objectAreaXSection;
    public float spring_stiffness, spring_resting;
    private float momentOfInertia;
    private float invMomentOfInertia;
    float m_Inertia;

    [Header("Outside Force Suppliers")]
    public Transform surfaceTransform;
    Vector2 localCM, globalCM, foreAppPoint;




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
                MoI = 0.5f * mass * (radius * radius);
                momentOfInertia = MoI;
                invMomentOfInertia = 1.0f / MoI;
                return MoI;
            case Shape_2D.Ring:
                MoI = 0.5f * mass * ((o_Radius * o_Radius) * (i_Radius * i_Radius));
                momentOfInertia = MoI;
                invMomentOfInertia = 1.0f / MoI;
                return MoI;
            case Shape_2D.Rectangle:
                MoI = (float)1 / 12 * mass * ((height * height) * (width * width));
                momentOfInertia = MoI;
                invMomentOfInertia = 1.0f / MoI;
                return MoI;
            case Shape_2D.Rod:
                MoI = (float)1 / 12 * mass * (length * length);
                momentOfInertia = MoI;
                invMomentOfInertia = 1.0f / MoI;
                return MoI;
        }

        return MoI;
    }

    //Lab 03 - Step 02
    float convertToAcceleration(float inertia) //Update angular Acceleration
    {
        float conversion = 1 / inertia * f_Torque;

        //f_Torque = 0f;

        return conversion;
    }

    void addTorque(Vector2 thePosition, Vector2 theForce) //Apply torque
    {
        //torqueAmount could also be torque, which would mean the function doesn't take the float
        //f_Torque += torque;

        //its a psuedovector, the cross product between force vector and position vector
        //f_Torque = Vector3.Cross(position, force);

        Debug.Log("thePosition = " + thePosition + "    theForce = " + theForce);

        //Vector2 tempTorque = Vector3.Cross(thePosition, theForce);
        float tempTorque = (thePosition.x * theForce.y) - (thePosition.y * theForce.x);

        //Debug.Log("tempTorque = " + tempTorque + "    tempTorque.mag  = " + tempTorque.magnitude);
        Debug.Log("tempTorque = " + tempTorque);

        //f_Torque += tempTorque.magnitude;
        f_Torque += tempTorque;

    }


    //Lab02 - Step02


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

    void updateAngularAcceleration()
    {
        Debug.Log("f_torque = " + f_Torque + "   inv = " + invMomentOfInertia);
        angularAcceleration = f_Torque * invMomentOfInertia;
        f_Torque = 0.0f;
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
        position = this.transform.position;
        setMass(startingMass);
        calculateMomentOfInertia(Shape);
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
                updateAngularAcceleration();

                transform.eulerAngles = new Vector3(0f, 0f, rotation);
            }
            else if (RotationUpdateMethod == RotationFunction.RotationKinematic)
            {
                updateRotKinematic(Time.fixedDeltaTime);
                updateAngularAcceleration();

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
                updateAngularAcceleration();

                transform.eulerAngles = new Vector3(0f, 0f, rotation);

            }
            else if (RotationUpdateMethod == RotationFunction.RotationKinematic)
            {
                updateRotKinematic(Time.fixedDeltaTime);
                updateAngularAcceleration();


                transform.eulerAngles = new Vector3(0f, 0f, rotation);

            }
        }


        //we should already know what the MoI specific to the game model is based on what the enum input was
        if (Input.GetKey(KeyCode.F))
        {
            addTorque(t_Position, t_Force);
        }
        
        if(SHIP_MODE)
        {
            //F_gravity: f = mg
            Vector2 gravity = ForceGenerator.generateForce_Gravity(mass, -1.625f, Vector2.up);  //I USED A DIFFERENT GRAVITY, THE MOONS GRAVITY
            Vector2 surfaceNormalUnit = new Vector2(Mathf.Sin(surfaceTransform.eulerAngles.z), Mathf.Cos(surfaceTransform.eulerAngles.z));
            Vector2 normal = ForceGenerator.GenerateForce_normal(gravity, surfaceNormalUnit);

            //addForce(gravity);

            //addForce(normal);     //find a way to add normal force only when colliding with ground

            //this is shit and is temporary
            
            

            float RotZOBB = this.transform.rotation.z;
            Vector2 xNormOBB = new Vector2(Mathf.Cos(RotZOBB), Mathf.Sin(RotZOBB));
            Vector2 yNormOBB = new Vector2(-Mathf.Sin(RotZOBB), Mathf.Cos(RotZOBB));


            Vector2 elevationForce = yNormOBB * elevationThrust;//new Vector2(0.0f, 20.0f);// * yNormOBB;
            Vector2 lateralForce = xNormOBB * lateralThrust;//new Vector2(4.0f, 0.0f);// * xNormOBB;

            //Vector2 elevationForce = new Vector2(0.0f, elevationThrust);// * yNormOBB;
            //Vector2 lateralForce = new Vector2(lateralThrust, 0.0f);// * xNormOBB;

            //Yaw control (isnt it pitch though? yaw would be on the Y axis which just pointlessly spins it)
            if (Input.GetKey(KeyCode.Q))
            {
                addTorque(t_Position, -t_Force);
            }
            else if (Input.GetKey(KeyCode.E))
            {
                addTorque(t_Position, t_Force);
            }
            else
            {
                //we need to slow torque a little maybe, or not who gives a shit
            }

            //rotation is greater than pi
            //if()
            {

            }

            //rotation is less than negative pi
            //if()
            {

            }

            //Elevation control
            if (Input.GetKey(KeyCode.W))
            {
                addForce(elevationForce);
            }
            if(Input.GetKey(KeyCode.S))
            {
                addForce(-elevationForce);
            }

            // Lateral control
            if (Input.GetKey(KeyCode.A))
            {
                addForce(-lateralForce);
            }
            if (Input.GetKey(KeyCode.D))
            {
                addForce(lateralForce);
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

        //Vector2 gravity = ForceGenerator.generateForce_Gravity(mass, -9.871f, Vector2.up);
        //Vector2 surfaceNormalUnit = new Vector2(Mathf.Sin(surfaceTransform.eulerAngles.z), Mathf.Cos(surfaceTransform.eulerAngles.z));
        //Vector2 normal = ForceGenerator.GenerateForce_normal(gravity, surfaceNormalUnit);

        //Vector2 drag = ForceGenerator.GenerateForce_drag(velocity, fluidVelocity, fluidDensity, objectAreaXSection, dragCoefficient);


        //addForce(gravity);
        //addForce(drag);

        //******** Block on a slanted surface ********
        //if(gameObject.name == "SlideCube") // Demostrating Gravity, Normal (As Sliding) and Friction forces
        //{
        //    //addForce(gravity);
        //    //addForce(normal);
        //    Vector2 sliding = ForceGenerator.GenerateForce_sliding(gravity, normal);
        //    addForce(sliding);

        //    //Using Friction (some more help from brother)
        //    //x = Mass * g * sin()cos() y = mass * g * sin()sin()
        //    //Vector2 fOpposing = new Vector2((mass * -9.871f * Mathf.Sin(surfaceTransform.eulerAngles.z) * Mathf.Cos(surfaceTransform.eulerAngles.z)), (mass * -9.871f * Mathf.Sin(surfaceTransform.eulerAngles.z) * Mathf.Sin(surfaceTransform.eulerAngles.z)));
        //    addForce(ForceGenerator.GenerateForce_friction(normal, sliding, velocity, getCoeff_Static(MaterialType_Static), getCoeff_Kinetic(MaterialType_Kinetic)));

        //}




        ////********  Cube on a Spring ********
        //if (gameObject.name == "HangCube") // Demonstrating Spring and Drag forces
        //{
        //    addForce(ForceGenerator.GenerateForce_spring(transform.position, surfaceTransform.position, spring_resting, spring_stiffness));
        //    addForce(ForceGenerator.GenerateForce_drag(velocity, fluidVelocity, fluidDensity, 1, 1.05f)); //Drag coef & object x-section are pre-calculated for a 
        //    //Velocity is taken fronm the particle properties and is integrated by the script, 
        //    //while fluid density & velocity are public variables that default to Earth air with no wind
        //}

    }
}
