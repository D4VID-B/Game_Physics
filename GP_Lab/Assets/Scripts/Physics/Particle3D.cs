using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle3D : MonoBehaviour
{
#region Variables
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

    [Header("Demo: Sin Spin")]
    public Vector3 spinAngularAcceleration;
    public Vector3 spinAngularVelocity;
    public bool useAngularVelocityInsteadOfAcceleration;

    #endregion


#region Enums
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

    #endregion


#region Mass
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
#endregion

#region Lab06

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
        //rotation += angularVelocity * dt;

        //this is the angularVelocity * dt part
        //Quaternion temp = multiplyQuatNum(Quaternion.Euler(angularVelocity.x, angularVelocity.y, angularVelocity.z), dt);
        //rotation = rotation * temp;

        //to add the effects of two quaternions together, you multiply them
        //      though this seems to be more like:  rotation = (rotation + angularVelocity) * dt


        // 1/2 * w * q.nrm
        //Quaternion temp = multiplyScalarByQuaternion(dt * 0.5f, multiplyVectorByQuaternion(angularVelocity, rotation.normalized));
        Quaternion temp = multiplyScalarByQuaternion(dt, multiplyScalarByQuaternion(0.5f, multiplyVectorByQuaternion(angularVelocity, rotation.normalized)));


        Debug.Log("multVectByQuat: " + multiplyVectorByQuaternion(angularVelocity, rotation.normalized));
        Debug.Log("Temp: " + temp);

        //componant wise addition
        rotation = new Quaternion((rotation.normalized.x + temp.x), (rotation.normalized.y + temp.y), (rotation.normalized.z + temp.z), (rotation.normalized.w + temp.w));

        //rotation = rotation.normalized;

        //integrate
        //normalize
        //integrate
        angularVelocity += angularAcceleration * dt;
    }

    public Quaternion multiplyScalarByQuaternion(float scalar, Quaternion quat)
    {
        //this is quat v quat
        /*
         [q1.x * q2.w + q1.y * q2.z - q1.z * q2.y + q1.w * q2.x,
         -q1.x * q2.z + q1.y * q2.w + q1.z * q2.x + q1.w * q2.y,
          q1.x * q2.y - q1.y * q2.x + q1.z * q2.w + q1.w * q2.z,
         -q1.x * q2.x - q1.y * q2.y - q1.z * q2.z + q1.w * q2.w]
        */

        //this below does not work, because it implies that there are i j and k associated with the scalar, but there isnt
        /*
         [q1.x * # + q1.y * # - q1.z * # + q1.w * #,
         -q1.x * # + q1.y * # + q1.z * # + q1.w * #,
          q1.x * # - q1.y * # + q1.z * # + q1.w * #,
         -q1.x * # - q1.y * # - q1.z * # + q1.w * #]
        */

        //therefore the scalar should just be a straight mult of x y z and w

        //this is  what the DirectX implementation states
        //https://www.winehq.org/pipermail/wine-cvs/2007-November/037925.html


        Quaternion result = new Quaternion((quat.x * scalar), (quat.y * scalar), (quat.z * scalar), (quat.w * scalar));

        //a quaternion is just a combo of vector and a scalar, so should the scalar we mult by only affect the w value of a quaternion?

        return result;
    }

    /// <summary>
    /// Muliply a 3D Vector by a Quaternion. Returns the resulting quaternion.
    /// </summary>
    public Quaternion multiplyVectorByQuaternion(Vector3 vect, Quaternion quat)
    {
        //not the same as rotating, so dont do those steps

        //(w = 0) quaternion = a 3D vector:
        //      v = (v[1] * i) + (v[2] * j) + (v[3] * k) + 0
        //
        //essentially it should be value mult value but where do the unreals come in?
        //if the w on a 3D vector is 0 in quaternion land, doesn that mean we multiply by 0 on the passed in quaternions w?

        // take the first value of both multiplied by eachother:
        //   vect.x = v[1] * i;
        //   quat.x = Q[1] * i;

        //multiplyy together (let # just represent real numbers in the equation)
        //    (#i) * (#i) = # * # * i * i
        //    # * # * -1    (because i^2 = -1)
        //    -(# * #)

        //the w value
        // a vector 3D is a "pure quaternion", w = 0. So 0 * quat.w is 0

        //take that knowledge and apply it to the normal quat v quat
        /*
         [q1.x * q2.w + q1.y * q2.z - q1.z * q2.y + q1.w * q2.x,
         -q1.x * q2.z + q1.y * q2.w + q1.z * q2.x + q1.w * q2.y,
          q1.x * q2.y - q1.y * q2.x + q1.z * q2.w + q1.w * q2.z,
         -q1.x * q2.x - q1.y * q2.y - q1.z * q2.z + q1.w * q2.w]
        */

        //treating vect as a pure quaternion, that way we can keep the i j and k values that make the above equation possible. The requirement is that we add a w, but its just 0 so its easy
        /*
        Quaternion result = new Quaternion(quat.x * 0 + quat.y * vect.z - quat.z * vect.y + quat.w * vect.z,
                                            -quat.x * vect.z + quat.y * 0 + quat.z * vect.x + quat.w * vect.y,
                                            quat.x * vect.y - quat.y * vect.x + quat.z * 0 + quat.w * vect.z,
                                            -quat.x * vect.x - quat.y * vect.y - quat.z * vect.z + quat.w * 0);
        */

        //Approach 01
        //Quaternion result = new Quaternion(0 + quat.y * vect.z - quat.z * vect.y + quat.w * vect.z,
        //                                    -quat.x * vect.z + 0 + quat.z * vect.x + quat.w * vect.y,
        //                                    quat.x * vect.y - quat.y * vect.x + 0 + quat.w * vect.z,
        //                                    -quat.x * vect.x - quat.y * vect.y - quat.z * vect.z + 0);


        Quaternion result = new Quaternion(0 + vect.x * quat.w + vect.y * quat.z - vect.z * quat.y,
                                            0 - vect.x * quat.z + vect.y * quat.w + vect.z * quat.x,
                                             0 + vect.x * quat.y - vect.y * quat.x + vect.z * quat.w,
                                                0 - vect.x * quat.x - vect.y * quat.y - vect.z * quat.z);

        //Approach 02
        //Quaternion result = new Quaternion(-(quaternion.x * vector.x), -(quaternion.y * vector.y), -(quaternion.z * vector.z), 0);

        //Approach 03
        //Vector3 quatVec = new Vector3(quat.x, quat.y, quat.z);
        //float real =  -Vector3.Dot(vect, quatVec);
        //Vector3 tempVec = Vector3.Cross( (quat.w * vect) + quatVec, vect);
        //Quaternion result = new Quaternion(tempVec.x, tempVec.y, tempVec.z, real);

        return result;
    }
#endregion



#region Runtime

    void Start()
    {
        position = this.transform.position;
        setMass(startingMass);
    }

    void DemoChoice()
    {

    }

    void FixedUpdate()
    {

        float sinTime = Mathf.Sin(Time.time);

        if(!useAngularVelocityInsteadOfAcceleration)
        {
            angularAcceleration = new Vector3(sinTime * spinAngularAcceleration.x, sinTime * spinAngularAcceleration.y, sinTime * spinAngularAcceleration.z);
        }
        else
        {
            angularVelocity = new Vector3(sinTime * spinAngularVelocity.x, sinTime * spinAngularVelocity.y, sinTime * spinAngularVelocity.z);
        }

        //Lab 01 & Lab 02 - Step 3
        if (IntegrationMethod == PositionFunction.PositionEuler)
        {
            updatePosEulerExplicit(Time.fixedDeltaTime);

            transform.position = position;

            //Rotations
            if (RotationUpdateMethod == RotationFunction.RotationEuler)
            {
                updateRotEulerExplicit(Time.fixedDeltaTime);

                transform.rotation = rotation;

            }

        }
        else if (IntegrationMethod == PositionFunction.PositionKinematic)
        {
            updatePosKinematic(Time.fixedDeltaTime);

            transform.position = position;

            //Rotations
            if (RotationUpdateMethod == RotationFunction.RotationEuler)
            {
                updateRotEulerExplicit(Time.fixedDeltaTime);

                transform.rotation = rotation;

            }

        }

    }
#endregion

}
