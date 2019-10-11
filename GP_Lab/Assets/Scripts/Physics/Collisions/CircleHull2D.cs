using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleHull2D : CollisionHull2D
{

    public CircleHull2D () : base (CollisionHullType2D.Hull_Circle) { }

    [Range(0.0f, 100.0f)]
    public float radius;


    //Collision col = null;

    void Start()
    {

    }


    public override bool TestCollisionVsCircle(CircleHull2D circle, ref Collision col)
    {
        //Collision test passes if(distance between them <= sum of radii)
        //Optimize: by ^2 both sides
        //1)    Get particles' positions(centers)
        //2)    Take difference between centers
        //3)    distance^2 = dot(diff, diff)
        //4)    summ of radii
        //5)    summ of radii ^2
        //6)    Test: dist^2 <= summ^2

        Debug.Log("CVC test start");

        Vector2 positionA = circle.transform.position;
        Vector2 positionB = this.transform.position;

        Vector2 diff = positionB - positionA;

        float distance = (diff.x * diff.x) + (diff.y * diff.y); //distance squared

        float sumOfRadii = radius + circle.radius;

        float squaredSumOfRadii = sumOfRadii * sumOfRadii;

        if (distance <= squaredSumOfRadii)
        {
            Debug.Log("CVC test pass");


            //Assign objects
            col.a = this;
            col.b = circle;
            

            //Calculate contact normal - is also the contact direction
            distance = Mathf.Sqrt(distance);
            col.contacts[0].normal = diff*(1/distance);

            //Calculate contact point => center of the overlap
            //take the diff
            //magnitude = distance
            //normalise by /magnitude
            Vector2 e0 = col.contacts[0].normal * -radius;
            Vector2 e1 = col.contacts[0].normal * circle.radius;
            col.contacts[0].point = (e0+e1)*0.5f;

            //Calculate interpenetration depth
            //subtract distance from sum of radii => interpen depth
            
            col.interpenDepth = sumOfRadii - distance;

            updateCollision(ref col);

            return true;
        }
        else
        {
            Debug.Log("CVC test fail");

            return false;
        }
    }

    public override bool TestCollisionVsAABB(AxisAlignedBoundingBoxHull2D box, ref Collision col)
    {
        //Calculate closest point by clamping(??) center; closest point vs circle test
        //
        //1)

        Debug.Log("AABB v C test");

        Vector2 circCenter = this.transform.position;
        Vector2 boxCenter = box.transform.position;

        float boxXMin = box.transform.position.x - (box.length * 0.5f);
        float boxYMin = box.transform.position.y - (box.height * 0.5f);

        float boxXMax = box.transform.position.x + (box.length * 0.5f);
        float boxYMax = box.transform.position.y + (box.height * 0.5f);


        float clampedX = Mathf.Clamp(circCenter.x, boxXMin, boxXMax);
        float clampedY = Mathf.Clamp(circCenter.y, boxYMin, boxYMax);

        Vector2 distance = new Vector2(circCenter.x - clampedX, circCenter.y - clampedY);

        float dSq = (distance.x * distance.x) + (distance.y * distance.y);

        if(dSq < (this.radius * this.radius))
        {
            Debug.Log("AABB v C pass");

            //Assign objects
            col.a = this;
            col.b = box;

            //get the clamped combinded vector as the point to have norm from
            Vector2 Point = new Vector2(clampedX, clampedY);

            //take the centerpoint of the circle, subtract the point to get the norm (it may be point - circ)
            //Vector2 norm = (circCenter - Point).normalized; //(same as distance)

            //col.contacts[0].normal = norm;
            col.contacts[0].normal = distance.normalized;
            col.contacts[0].point = Point;

            //radius of the circle minus the distance to the original point of entry
            col.interpenDepth = (this.radius * this.radius) - dSq;

            updateCollision(ref col);

            return true;
        }
        else
        {
            Debug.Log("AABB v C fail");

            return false;
        }

        /*
        bool colOnX = false;
        bool colOnY = false;

        if (circCenter.x + radius >= box.transform.position.x - (box.length * 0.5f) && box.transform.position.x + (box.length * 0.5f) >= circCenter.x - radius)
        {
            colOnX = true;
        }

        if (circCenter.y + radius >= box.transform.position.y - (box.length * 0.5f) && box.transform.position.y + (box.height * 0.5f) >= circCenter.y - radius)
        {
            colOnY = true;
        }

        if (colOnY && colOnX)
        {
            updateCollision(ref c);
            return true;
        }
        else
        {
            return false;
        }
        */

       
        //when we clamp on each dimension, there are only two dimesnions

        
    }

    public override bool TestCollisionVsOBB(ObjectBoundingBoxHull2D box, ref Collision c)
    {
        //Same as above, but first:
        //move circle center into box space by * -world transform
        //1) this.position is now * -box.position 
        //2) call testAABB with box

        //Vector2 circCenter = -box.transform.localToWorldMatrix.MultiplyVector(this.transform.position);
        Vector2 circCenter = this.transform.position;

        //get the norms of the box
        float RotZOBB = box.transform.eulerAngles.z;
        //Vector2 xNormOBB = new Vector2(Mathf.Cos(RotZOBB), Mathf.Sin(RotZOBB));
        //Vector2 yNormOBB = new Vector2(-Mathf.Sin(RotZOBB), Mathf.Cos(RotZOBB));

        //just rotate the circle around the centerpoint of the box using the norm (or angle) of the box then call AABB
        //      this is a copy over for AABB test (dont change these values, we just need to change the circle)
        //      the box is rotated by the -RotZOBB in a way to get back to axis aligned, so we need to do the same for the circle

        //AxisAlignedBoundingBoxHull2D newBox = this.gameObject.AddComponent(typeof(AxisAlignedBoundingBoxHull2D)) as AxisAlignedBoundingBoxHull2D;
        //AxisAlignedBoundingBoxHull2D newBox = gameObject.AddComponent<AxisAlignedBoundingBoxHull2D>();
        //CollisionHull2D newBox = null;

        //GameObject temp = new GameObject();
        //AxisAlignedBoundingBoxHull2D newBox = temp.AddComponent<AxisAlignedBoundingBoxHull2D>();

        //AxisAlignedBoundingBoxHull2D newBox = new AxisAlignedBoundingBoxHull2D(box);

        AxisAlignedBoundingBoxHull2D newBox = new AxisAlignedBoundingBoxHull2D();
        newBox.length = box.length;
        newBox.height = box.height;
        newBox.transform.position = box.transform.position;

        //create a new circle to edit
        CircleHull2D newCirc = new CircleHull2D();
        newCirc.radius = this.radius;

        //Rotate centerpoint of circ around the box pos point by the angle of the box
        float subPosX = circCenter.x - box.transform.position.x;
        float subPosY = circCenter.y - box.transform.position.y;
        Vector2 newPos = new Vector2(Mathf.Cos(-RotZOBB) * (subPosX) - Mathf.Sin(-RotZOBB) * (subPosY) + box.transform.position.x,
                                            Mathf.Sin(-RotZOBB) * (subPosX) + Mathf.Cos(-RotZOBB) * (subPosY) + box.transform.position.y);

        newCirc.transform.position = newPos;

        //a possible problem is that now that we have a new 
        return newCirc.TestCollisionVsAABB(newBox, ref c);


        /*
        //bottom left
        float x = box.transform.position.x - box.length * 0.5f;
        float y = box.transform.position.y - box.height * 0.5f;
        Vector2 bottomLeft = new Vector2(x, y);

        //bottom right
        x = box.transform.position.x + box.length * 0.5f;
        y = box.transform.position.y - box.height * 0.5f;
        Vector2 bottomRight = new Vector2(x, y);

        //top left
        x = box.transform.position.x + box.length * 0.5f;
        y = box.transform.position.y - box.height * 0.5f;
        Vector2 topLeft = new Vector2(x, y);

        //top right
        x = box.transform.position.x + box.length * 0.5f;
        y = box.transform.position.y + box.height * 0.5f;
        Vector2 topRight = new Vector2(x, y);


        //project all x valued (one for each corner, 4) vertices onto normal for x
        //Vector2 xNormProjectedVertices = Vector3.Project( , xNormOBB);
        Vector2 pointX1 = (bottomLeft * xNormOBB) * xNormOBB;
        Vector2 pointX2 = (bottomRight * xNormOBB) * xNormOBB;
        Vector2 pointX3 = (topLeft * xNormOBB) * xNormOBB;
        Vector2 pointX4 = (topRight * xNormOBB) * xNormOBB;


        //project all y valued (one for each corner, 4) vertices onto normal for y
        Vector2 pointY1 = (bottomLeft * yNormOBB) * yNormOBB;
        Vector2 pointY2 = (bottomRight * yNormOBB) * yNormOBB;
        Vector2 pointY3 = (topLeft * yNormOBB) * yNormOBB;
        Vector2 pointY4 = (topRight * yNormOBB) * yNormOBB;

        */



        /*
        //bottom left
        float x1 = box.transform.position.x - box.length * 0.5f;
        float x2 = box.transform.position.x + box.length * 0.5f;
        Vector2 bottomLeft = new Vector2(x1, x2);

        //top right
        float y1 = box.transform.position.y - box.height * 0.5f;
        float y2 = box.transform.position.y + box.height * 0.5f;
        Vector2 topRight = new Vector2(y1, y2);

        //go to world space
        Vector2 localToWorldX = box.transform.localToWorldMatrix.MultiplyVector(bottomLeft);
        Vector2 localToWorldY = box.transform.localToWorldMatrix.MultiplyVector(topRight);
        


        //AxisAlignedBoundingBoxHull2D newBox = new AxisAlignedBoundingBoxHull2D();

        
        //AABB, finding corners(sides) in if()
        bool colOnX = false;
        bool colOnY = false;

        Debug.Log("circPos: " + circCenter + "    localToWorldX: " + localToWorldX + "    localToWorldY: " + localToWorldY);

        if (circCenter.x + radius >= localToWorldX.x && localToWorldX.y >= circCenter.x - radius)
        {
            colOnX = true;
        }

        if (circCenter.y + radius >= localToWorldY.x && localToWorldY.y >= circCenter.y - radius)
        {
            colOnY = true;
        }

        if (colOnY && colOnX)
        {
            updateCollision(ref c);
            return true;
        }
        else
        {
            return false;
        }
        */



        //Assign objects
        //col.a = this;
        //col.b = box;

        //


        //return false;

        //return this.TestCollisionVsAABB(box, ref c);
    }

}
