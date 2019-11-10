using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle3D : MonoBehaviour
{
    #region Variables
    [Header("Switches")]
    public bool shouldUpdateParticle = true;
    public bool shouldUpdateRotation;
    public bool shouldUpdatePosition;

    [Header("Position")]
    public Vector3 position;
    public Vector3 velocity;
    public Vector3 acceleration;
    Vector3 force;

    [Header("Rotation")]
    public Quaternion rotation;
    public Vector3 angularVelocity;
    

    [Header("Mass")]
    public float startingMass;
    float mass, massInv;

    /// <summary>
    /// Angular Dynamics
    /// </summary>
    Matrix4x4 worldTransform, inverseWorldTransform; 
    Vector3 localCoM, worldCoM;
    Matrix4x4 localTensor = Matrix4x4.identity, worldTensor = Matrix4x4.identity; //world = local * inverseWorldTransform
    Vector3 torqueForce,  angularAcceleration;
    public Vector4 torqueDirection;
    public float torqueMag;
    private Vector4 f_torque;
    Vector3 momentArm;
    

    [Header("Demo: Sin Spin")]
    public Vector3 spinAngularAcceleration;
    public Vector3 spinAngularVelocity;
    public bool useAngularVelocityInsteadOfAcceleration;

    [Header("Demo: Sin Move")]
    public Vector3 moveAcceleration;
    public Vector3 moveVelocity;
    public bool useVelocityInsteadOfAcceleration;

    [Header("Lab 07: AD Shape Parameters")]

    [Range(0.0f, 3.0f)]
    public float radius;
    [Range(0.0f, 3.0f)]
    public float height;
    public float x_extent, y_extent, z_extent; //These are cuboid values

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

    public enum Shape3D
    {
        Solid_Sphere,
        Hollow_Sphere,
        Solid_Cuboid,
        Hollow_Cuboid,
        Solid_Cylinder,
        Solid_Cone

    }
    public Shape3D Shape;

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
        Quaternion temp = multiplyScalarByQuaternion(dt * 0.5f, multiplyVectorByQuaternion(angularVelocity, rotation));


        //Debug.Log("multVectByQuat: " + multiplyVectorByQuaternion(angularVelocity, rotation));
        //Debug.Log("Temp: " + temp);

        //componant wise addition
        rotation = new Quaternion((rotation.x + temp.x), (rotation.y + temp.y), (rotation.z + temp.z), (rotation.w + temp.w));

        rotation = rotation.normalized;

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
    public Quaternion multiplyVectorByQuaternion(Vector3 vector, Quaternion quat)
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


        Quaternion result = new Quaternion(0 + vector.x * quat.w + vector.y * quat.z - vector.z * quat.y,
                                            0 - vector.x * quat.z + vector.y * quat.w + vector.z * quat.x,
                                             0 + vector.x * quat.y - vector.y * quat.x + vector.z * quat.w,
                                                0 - vector.x * quat.x - vector.y * quat.y - vector.z * quat.z);

        //Approach 02
        //Quaternion result = new Quaternion(-(quaternion.x * vector.x), -(quaternion.y * vector.y), -(quaternion.z * vector.z), 0);

       // Debug.Log("Vector 3 is: " + vector);
        //Debug.Log("Quaternion is: " + quat);
        //Approach 03
        //Vector3 quatVec = new Vector3(quat.x, quat.y, quat.z);
        //float real =  -Vector3.Dot(vector, quatVec);
        //Vector3 tempVec = Vector3.Cross((quat.w * vector) + vector, quatVec);
        //Quaternion result = new Quaternion(tempVec.x, tempVec.y, tempVec.z, real);

        return result;
    }
    #endregion

#region Lab07
    Matrix4x4 setTensor(Shape3D theShape)
    {
        switch (theShape) 
        {
            case Shape3D.Solid_Sphere:
                {
                    localTensor = Matrix4x4.identity;
                    localCoM = new Vector3(0.0f, 0.0f, 0.0f);
                    /*
                     * I = [ 2/5 * mass * (radius*radius)   0   0]
                     *     [ 0   2/5 * mass * (radius*radius)   0]
                     *     [ 0   0   2/5 * mass * (radius*radius)]
                     */

                    localTensor.m00 = 0.4f * mass * (radius * radius);
                    localTensor.m11 = 0.4f * mass * (radius * radius);
                    localTensor.m22 = 0.4f * mass * (radius * radius);
                    localTensor.m33 = 1;

                    return localTensor;
                }
            case Shape3D.Hollow_Sphere:
                {
                    localTensor = Matrix4x4.identity;
                    localCoM = new Vector3(0.0f, 0.0f, 0.0f);

                    /*
                     * I = [ 2/3 * mass * (radius*radius)   0   0]
                     *     [ 0   2/3 * mass * (radius*radius)   0]
                     *     [ 0   0   2/3 * mass * (radius*radius)]
                     */

                    localTensor.m22 = 0.67f * mass * (radius * radius);
                    localTensor.m00 = 0.67f * mass * (radius * radius);
                    localTensor.m11 = 0.67f * mass * (radius * radius);
                    localTensor.m33 = 1;

                    return localTensor;
                }
            case Shape3D.Solid_Cuboid:
                {
                    localTensor = Matrix4x4.identity;
                    localCoM = new Vector3(0.0f, 0.0f, 0.0f);

                    /*
                     * I = [ 1/12*mass*((dy*dy) + (dz*dz))  0   0]
                     *     [ 0  1/12*mass*((dx*dx) + (dz*dz))   0]
                     *     [ 0  0   1/12*mass*((dx*dx) + (dy*dy))]
                     *  dx, dy, dz = extents along the axis
                     */

                    localTensor.m00 = 0.083f * mass * ((y_extent * y_extent) + (z_extent * z_extent));
                    localTensor.m11 = 0.083f * mass * ((x_extent * x_extent) + (z_extent * z_extent));
                    localTensor.m22 = 0.083f * mass * ((x_extent * x_extent) + (y_extent * y_extent));
                    localTensor.m33 = 1;

                    return localTensor;
                }
            case Shape3D.Hollow_Cuboid:
                {
                    localTensor = Matrix4x4.identity;
                    localCoM = new Vector3(0.0f, 0.0f, 0.0f);

                    /*
                     * I = [5/3 * mass * ((dy^2) + (dz^2))  0  0]
                     *     [0  5/3 * mass * ((dx^2) + (dz^2))  0]
                     *     [0  0  5/3 * mass * ((dx^2) + (dy^2))]
                     *     width = dx; height = dy; depth = dz;
                     */

                    localTensor.m00 = 1.67f * mass * ((y_extent * y_extent) + (z_extent * z_extent));
                    localTensor.m11 = 1.67f * mass * ((x_extent * x_extent) + (z_extent * z_extent));
                    localTensor.m22 = 1.67f * mass * ((x_extent * x_extent) + (y_extent * y_extent));
                    localTensor.m33 = 1;

                    return localTensor;
                }
            case Shape3D.Solid_Cylinder:
                {

                    /*
                     * I = [ 1/12 * mass * (height * height) + 1/4 * mass * (radius*radius)   0   0]
                     *     [ 0   1/12 * mass * (height * height) + 1/4 * mass * (radius*radius)   0]
                     *     [ 0   0   1/12 * mass * (height * height) + 1/4 * mass * (radius*radius)]
                     */
                    localTensor = Matrix4x4.identity;
                    localCoM = new Vector3(0.0f, 0.5f * height, 0.0f); //From: http://farside.ph.utexas.edu/teaching/301/lectures/node102.html

                    localTensor.m00 = 0.083f * mass * (height * height) + 0.25f * mass * (radius * radius);
                    localTensor.m11 = 0.083f * mass * (height * height) + 0.25f * mass * (radius * radius);
                    localTensor.m22 = 0.083f * mass * (height * height) + 0.25f * mass * (radius * radius);
                    localTensor.m33 = 1;

                    return localTensor;
                }
            case Shape3D.Solid_Cone:
                {
                    /*
                     * I = [ 3/80 * mass * (height * height) + 3/20 * mass * (radius*radius)   0   0]
                     *     [ 0   3/10 * mass * (radius*radius)   0]
                     *     [ 0   0   3/5 * mass * (height * height) + 3/20 * mass * (radius*radius)]
                     */
                    localTensor = Matrix4x4.identity;
                    localCoM = new Vector3(0.0f, 0.75f * height, 0.0f); //CoM is at 3/4 height -> using this: https://www.assignmentexpert.com/homework-answers/physics-answer-36570.pdf

                    localTensor.m00 = 0.0375f * mass * (height * height) + 0.15f * mass * (radius * radius);
                    localTensor.m11 = 0.15f * mass * (radius * radius);
                    localTensor.m22 = 0.6f * mass * (height * height) + 0.15f * mass * (radius * radius);
                    localTensor.m33 = 1;


                    return localTensor;
                }
        }

        //Switch somehow failed - return "zero"
        return Matrix4x4.identity;
    }
    
    //Matrix4x4 transformInvInertiaTensor()

    Matrix4x4 worldTransformMatrix()
    {
        //all of these are local values

        //span (Tr, Ty, Tz, Position)

        Matrix4x4 final;

        final.m00 = this.transform.right.x;
        final.m01 = this.transform.up.x;
        final.m02 = this.transform.forward.x;
        final.m03 = this.transform.position.x;
        final.m10 = this.transform.right.y;
        final.m11 = this.transform.up.y;
        final.m12 = this.transform.forward.y;
        final.m13 = this.transform.position.y;
        final.m20 = this.transform.right.z;
        final.m21 = this.transform.up.z;
        final.m22 = this.transform.forward.z;
        final.m23 = this.transform.position.z;
        final.m30 = 0;
        final.m31 = 0;
        final.m32 = 0;
        final.m33 = 1;  //homogeneous coord

        return final;
    }

    Matrix4x4 localTransformMatrix()
    {
        Matrix4x4 final;
        
        final.m00 = (this.transform.localScale.x) * (rotation.w * rotation.w + rotation.x * rotation.x - rotation.y * rotation.y - rotation.z * rotation.z);
        final.m01 = 2 * (rotation.x * rotation.y - rotation.w * rotation.z);
        final.m02 = 2 * (rotation.x * rotation.z + rotation.w * rotation.y);
        final.m03 = position.x;
        final.m10 = 2 * (rotation.x * rotation.y + rotation.w * rotation.z);
        final.m11 = (this.transform.localScale.y) * (rotation.w * rotation.w - rotation.x * rotation.x + rotation.y * rotation.y - rotation.z * rotation.z);
        final.m12 = 2 * (rotation.y * rotation.z - rotation.w  * rotation.x);
        final.m13 = position.y;
        final.m20 = 2 * (rotation.x * rotation.z - rotation.w * rotation.y);
        final.m21 = 2 * (rotation.y * rotation.z + rotation.w * rotation.x);
        final.m22 = (this.transform.localScale.z) * (rotation.w * rotation.w - rotation.x * rotation.x - rotation.y * rotation.y + rotation.z * rotation.z);
        final.m23 = position.z;
        final.m30 = 0;
        final.m31 = 0;
        final.m32 = 0;
        final.m33 = 1;  //homogeneous coord

        return final;
    }

    public Matrix4x4 worldTransformMatAttemptTwo()
    {
        Matrix4x4 final;

        final.m00 = 1 - 2 * rotation.y * rotation.y - 2 * rotation.z * rotation.z;
        final.m01 = 2 * rotation.x * rotation.y - 2 * rotation.w * rotation.z;
        final.m02 = 2 * rotation.x * rotation.z + 2 * rotation.w * rotation.y;
        final.m03 = position.x;
        final.m10 = 2 * rotation.x * rotation.y + 2 * rotation.w * rotation.z;
        final.m11 = 1 - 2 * rotation.x * rotation.x - 2 * rotation.z * rotation.z;
        final.m12 = 2 * rotation.y * rotation.z - 2 * rotation.w * rotation.x;
        final.m13 = position.y;
        final.m20 = 2 * rotation.x * rotation.z - 2 * rotation.w * rotation.y;
        final.m21 = 2 * rotation.y * rotation.z + 2 * rotation.w * rotation.x;
        final.m22 = 1 - 2 * rotation.x * rotation.x - 2 * rotation.y * rotation.y;
        final.m23 = position.z;
        final.m30 = 0;
        final.m31 = 0;
        final.m32 = 0;
        final.m33 = 1;  //homogeneous coord

        return final;
    }

    public Matrix4x4 inverseMat4(Matrix4x4 mat)
    {
        Matrix4x4 inverse;

        inverse = mat.inverse;

        return inverse;
    }

    Vector3 convertToAngularFromTorque()
    {
        Vector3 angularA;


        //bruh you cant set a vector3 equal to a matrix
        angularA = worldTensor * f_torque;

        Debug.Log("World Tensor: \n" + worldTensor + "f_torque: \n" + f_torque + "angularA after tensor: \n" + angularA);

        //angularA = worldTensor * new Vector4(mTorque.x, mTorque.y, mTorque.z, 1);

        return angularA;
    }

    Vector3 calculateTorque(Vector3 ma, Vector3 nForce)
    {
        return Vector3.Cross(ma, nForce.normalized);
    }

    Vector3 calculateTorque(float magnitude, Vector4 mForce)
    {
        Vector3 final = mForce * magnitude;

        Debug.Log("Input Torque: " + final);
       
        return final;
    }

    void addTorque(Vector4 torqueToAdd)
    {
        f_torque += torqueToAdd;
    }

    void updateAngularAcceleration()
    {
        //Debug.Log("WTM: \n" + worldTransformMatAttemptTwo() + "WTAinv: \n" + inverseMat4(worldTransformMatAttemptTwo()) + "localTensor: \n" + localTensor);
        Matrix4x4 WTMAT = worldTransformMatAttemptTwo();

        Matrix4x4 invLT = localTensor;
        invLT.m00 = 1 / localTensor.m00;
        invLT.m11 = 1 / localTensor.m11;
        invLT.m22 = 1 / localTensor.m22;
        //invLT.m33 = 1 / localTensor.m33;

        Matrix4x4 invWTMAT;
        invWTMAT.m00 = WTMAT.m00;
        invWTMAT.m01 = WTMAT.m10;
        invWTMAT.m02 = WTMAT.m20;
        invWTMAT.m03 = WTMAT.m30;
        invWTMAT.m10 = WTMAT.m01;
        invWTMAT.m11 = WTMAT.m11;
        invWTMAT.m12 = WTMAT.m21;
        invWTMAT.m13 = WTMAT.m31;
        invWTMAT.m20 = WTMAT.m02;
        invWTMAT.m21 = WTMAT.m12;
        invWTMAT.m22 = WTMAT.m22;
        invWTMAT.m23 = WTMAT.m32;
        invWTMAT.m30 = WTMAT.m03;
        invWTMAT.m31 = WTMAT.m13;
        invWTMAT.m32 = WTMAT.m23;
        invWTMAT.m33 = WTMAT.m33;

        worldTensor = WTMAT * invLT * invWTMAT;

        //Debug.Log("Final: \n" + convertToAngularFromTorque());
        angularAcceleration = convertToAngularFromTorque();

        //Quaternion transposed
        //Vector4 worldTensorVector = worldTransformMatAttemptTwo() * localTensor * new Vector4(rotation.x, rotation.y, rotation.z, rotation.w);

        //component-wise
        //angularAcceleration = new Vector3(worldTensorVector.x * f_torque.x, worldTensorVector.y * f_torque.y, worldTensorVector.z * f_torque.z);


        f_torque = Vector3.zero;
    }

#endregion

    #region Runtime

    void Start()
    {
        position = this.transform.position;

        setMass(startingMass);
        setTensor(Shape);
        worldCoM = localCoM + transform.position;
    }


    void FixedUpdate()
    {
        if(shouldUpdateParticle)
        {
            float sinTime = Mathf.Sin(Time.time);

            if (!useAngularVelocityInsteadOfAcceleration)
            {
                angularAcceleration = new Vector3(sinTime * spinAngularAcceleration.x, sinTime * spinAngularAcceleration.y, sinTime * spinAngularAcceleration.z);
            }
            else
            {
                angularVelocity = new Vector3(sinTime * spinAngularVelocity.x, sinTime * spinAngularVelocity.y, sinTime * spinAngularVelocity.z);
            }

            if (!useVelocityInsteadOfAcceleration)
            {
                acceleration = new Vector3(sinTime * moveAcceleration.x, sinTime * moveAcceleration.y, sinTime * moveAcceleration.z);
            }
            else
            {
                velocity = new Vector3(sinTime * moveVelocity.x, sinTime * moveVelocity.y, sinTime * moveVelocity.z);
            }

            /*
             var a: Vector3;
             var b: Vector3;
             var c: Vector3;

             var side1: Vector3 = b - a;
             var side2: Vector3 = c - a;

            var norm: Vector3 = Vector3.Cross(side1, side2);

            https://docs.unity3d.com/Manual/ComputingNormalPerpendicularVector.html
            */

            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * 20, Color.green);


            if (IntegrationMethod == PositionFunction.PositionEuler)
            {
                updatePosEulerExplicit(Time.fixedDeltaTime);

                transform.position = position;

                //Rotations
                if (RotationUpdateMethod == RotationFunction.RotationEuler)
                {
                    updateAngularAcceleration();
                    updateRotEulerExplicit(Time.fixedDeltaTime);

                    //Debug.Log("Rotaton calculated: " + rotation);

                    transform.rotation = rotation;

                    //Debug.Log("Object Rotation" + transform.rotation);
                }

            }
            else if (IntegrationMethod == PositionFunction.PositionKinematic)
            {
                updatePosKinematic(Time.fixedDeltaTime);

                transform.position = position;

                //Rotations
                if (RotationUpdateMethod == RotationFunction.RotationEuler)
                {
                    updateAngularAcceleration();
                    updateRotEulerExplicit(Time.fixedDeltaTime);

                    transform.rotation = rotation;


                }

            }

            if (Input.GetKey(KeyCode.F))
            {
                addTorque(calculateTorque(torqueMag, torqueDirection));
            }
        }

        if (Input.GetKey(KeyCode.X))
        {
            addTorque(calculateTorque(torqueMag, torqueDirection));
        }
    }


    

#endregion

}
