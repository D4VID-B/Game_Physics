using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CircleHull3D : CollisionHull3D
{

    public CircleHull3D () : base (CollisionHullType3D.Hull_Sphere) { }

    [Range(0.0f, 100.0f)]
    public float radius;

    public bool hitExplode;


    void Start()
    {
        hitExplode = false;
        
    }


    public override bool TestCollisionVsCircle(CircleHull3D circle, ref Collision col)
    {
        //Collision test passes if(distance between them <= sum of radii)
        //Optimize: by ^2 both sides
        //1)    Get particles' positions(centers)
        //2)    Take difference between centers
        //3)    distance^2 = dot(diff, diff)
        //4)    summ of radii
        //5)    summ of radii ^2
        //6)    Test: dist^2 <= summ^2

        Vector3 positionA = circle.transform.position;
        Vector3 positionB = this.transform.position;

        Vector3 diff = positionB - positionA;

        float distance = Vector3.Dot(diff, diff); //distance squared

        float sumOfRadii = radius + circle.radius;

        float squaredSumOfRadii = sumOfRadii * sumOfRadii;

        //Debug.Log("Dis: " + distance +"   sqSum: " + squaredSumOfRadii);

        if (distance <= squaredSumOfRadii)
        {
            //changeColor(this.gameObject, true);
            //changeColor(circle.gameObject, true);
            //Debug.Log("Changing color!");
            return true;
        }
        else
        {
            //changeColor(this.gameObject, false);
            //changeColor(circle.gameObject, false);
            return false;
        }
    }

    public override bool TestCollisionVsAABB(AxisAlignedBoundingBoxHull3D box, ref Collision col)
    {
        //Calculate closest point by clamping(??) center; closest point vs circle test
        //
        //1)


        Vector3 circCenter = this.transform.position;
        Vector3 boxCenter = box.transform.position;

        float boxXMin = box.transform.position.x - (box.length * 0.5f);
        float boxYMin = box.transform.position.y - (box.height * 0.5f);
        float boxZMin = box.transform.position.z - (box.depth * 0.5f);

        float boxXMax = box.transform.position.x + (box.length * 0.5f);
        float boxYMax = box.transform.position.y + (box.height * 0.5f);
        float boxZMax = box.transform.position.z + (box.depth * 0.5f);


        float clampedX = Mathf.Clamp(circCenter.x, boxXMin, boxXMax);
        float clampedY = Mathf.Clamp(circCenter.y, boxYMin, boxYMax);
        float clampedZ = Mathf.Clamp(circCenter.z, boxZMin, boxZMax);

        Vector3 distance = new Vector3(circCenter.x - clampedX, circCenter.y - clampedY, circCenter.z - clampedZ);

        float dSq = (distance.x * distance.x) + (distance.y * distance.y) + (distance.z * distance.z);

        if(dSq < (this.radius * this.radius))
        {

            return true;
        }
        else
        {
            return false;
        }

        
    }

    public override bool TestCollisionVsOBB(ObjectBoundingBoxHull3D box, ref Collision col)
    {
        //Same as above, but first:
        //move circle center into box space by * -world transform
        //1) this.position is now * -box.position 
        //2) call testAABB with box

        Matrix4x4 boxMAT = box.gameObject.GetComponent<Particle3D>().worldTransform;
        Matrix4x4 circMAT = this.gameObject.GetComponent<Particle3D>().worldTransform;
        Matrix4x4 boxMATinv = box.gameObject.GetComponent<Particle3D>().inverseWorldTransform;
        Matrix4x4 circMATinv = this.gameObject.GetComponent<Particle3D>().inverseWorldTransform;

        //Debug.Log("BoxWorldTransfer: \n" + box.gameObject.GetComponent<Particle3D>().worldTransform + "circWorldTransform: \n" + this.gameObject.GetComponent<Particle3D>().worldTransform + "boxInvWorld: \n" + box.gameObject.GetComponent<Particle3D>().inverseWorldTransform);

        Matrix4x4 sphereCenter = circMAT;
        Vector4 sphereCenterPoint = new Vector4(sphereCenter.m03, sphereCenter.m13, sphereCenter.m23, 1.0f);
        sphereCenterPoint = new Vector4(sphereCenterPoint.x - box.transform.position.x, sphereCenterPoint.y - box.transform.position.y, sphereCenterPoint.z - box.transform.position.z, 1.0f);
        sphereCenterPoint = boxMAT * sphereCenterPoint;

        //Debug.Log("SCP: " + sphereCenterPoint);


        Debug.DrawRay(sphereCenterPoint, new Vector3(1.0f, 0.0f, 0.0f) * 0.5f, Color.blue);



        //positions of points (THIS ASSUMES THE GOD DAMN BOX IS AABB, how do we use the rotMat of boxWorldTransform to adjust this)
        float boxXMin = (box.transform.position.x - (box.length * 0.5f));
        float boxYMin = (box.transform.position.y - (box.height * 0.5f));
        float boxZMin = (box.transform.position.z - (box.depth * 0.5f));

        float boxXMax = (box.transform.position.x + (box.length * 0.5f));
        float boxYMax = (box.transform.position.y + (box.height * 0.5f));
        float boxZMax = (box.transform.position.z + (box.depth * 0.5f));


        //clamped values
        float clampedX = Mathf.Clamp(sphereCenterPoint.x, boxXMin, boxXMax);
        float clampedY = Mathf.Clamp(sphereCenterPoint.y, boxYMin, boxYMax);
        float clampedZ = Mathf.Clamp(sphereCenterPoint.z, boxZMin, boxZMax);

        Vector4 clampedXYZ = new Vector4(clampedX, clampedY, clampedZ, 1.0f);
        
        /*
        clampedXYZ = (boxMAT * this.gameObject.GetComponent<Particle3D>().inverseWorldTransform * box.gameObject.GetComponent<Particle3D>().inverseWorldTransform).MultiplyVector(clampedXYZ);
        Vector3 distance = new Vector3(sphereCenterPoint.x - clampedXYZ.x, sphereCenterPoint.y - clampedXYZ.y, sphereCenterPoint.z - clampedXYZ.z);
        */

        //float clampedX = Mathf.Clamp(sphereCenterPoint.x, rotatedMins.x, rotatedMaxs.x);
        //float clampedY = Mathf.Clamp(sphereCenterPoint.y, rotatedMins.y, rotatedMaxs.y);
        //float clampedZ = Mathf.Clamp(sphereCenterPoint.z, rotatedMins.z, rotatedMaxs.z);

        

        Vector3 distance = new Vector3(sphereCenterPoint.x - clampedX, sphereCenterPoint.y - clampedY, sphereCenterPoint.z - clampedZ);
        
        //Vector3 distance = new Vector3(sphereCenterPoint.x - clampedVals.x, sphereCenterPoint.y - clampedVals.y, sphereCenterPoint.z - clampedVals.z);

        float dSq = (distance.x * distance.x) + (distance.y * distance.y) + (distance.z * distance.z);

        //float d = distance.x + distance.y + distance.z;
        //d = Mathf.Abs(d);

        //Debug.Log("Distance: " + distance + "  d: " + d + "  dSq: " + dSq + "  R: " + this.radius + "  R2: " + this.radius * this.radius);

        Debug.DrawRay(clampedXYZ, distance, Color.green);
        //Debug.DrawRay(new Vector3(clampedMat.m00, clampedMat.m11, clampedMat.m22), distance * (this.radius / distance.magnitude), Color.green);

        //Debug.Log("DistanceMag: " + distance.magnitude + "   rad: " + this.radius);

        if (dSq < this.radius * this.radius)
        {
        //    /*
        //    //Debug.Log("OBB v C pass");
        //    hitExplode = true;

        //    //Assign objects
        //    col.a = this;
        //    col.b = box;

        //    Vector3 clamped = new Vector3(clampedX, clampedY, clampedZ);

        //    //get the clamped combinded vector as the point to have norm from
        //    Vector3 Point = (clamped + (distance.normalized * this.radius)) * 0.5f;


        //    //col.contacts[0].normal = norm;
        //    col.contacts[0].normal = distance.normalized;
        //    col.contacts[0].point = Point;

        //    //radius of the circle minus the distance to the original point of entry
        //    col.interpenDepth = (this.radius * this.radius) - dSq;

        //    */

            return true;
        }
        else
        {
        //    //Debug.Log("OBB v C fail");

            return false;
        }

        
    }

}
