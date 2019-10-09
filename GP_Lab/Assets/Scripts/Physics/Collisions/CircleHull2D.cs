using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleHull2D : CollisionHull2D
{

    public CircleHull2D () : base (CollisionHullType2D.Hull_Circle) { }

    [Range(0.0f, 100.0f)]
    public float radius;

    Collision col;

    void Update()
    {

    }


    public override bool TestCollisionVsCircle(CircleHull2D circle, ref Collision c)
    {
        //Collision test passes if(distance between them <= sum of radii)
        //Optimize: by ^2 both sides
        //1)    Get particles' positions(centers)
        //2)    Take difference between centers
        //3)    distance^2 = dot(diff, diff)
        //4)    summ of radii
        //5)    summ of radii ^2
        //6)    Test: dist^2 <= summ^2

        Vector2 positionA = circle.transform.position;
        Vector2 positionB = this.transform.position;

        Vector2 diff = positionB - positionA;

        float distance = (diff.x * diff.x) + (diff.y * diff.y); 

        float sumOfRadii = radius + circle.radius;

        float squaredSumOfRadii = sumOfRadii * sumOfRadii;

        if (distance <= squaredSumOfRadii)
        {
            updateCollision(ref c);
            return true;
        }
        else
        {
            return false;
        }
    }

    public override bool TestCollisionVsAABB(AxisAlignedBoundingBoxHull2D box, ref Collision c)
    {
        //Calculate closest point by clamping(??) center; closest point vs circle test
        //
        //1)

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
            return true;
        }
        else
        {
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
        float RotZOBB = box.transform.rotation.z;
        Vector2 xNormOBB = new Vector2(Mathf.Cos(RotZOBB), Mathf.Sin(RotZOBB));
        Vector2 yNormOBB = new Vector2(-Mathf.Sin(RotZOBB), Mathf.Cos(RotZOBB));

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

        return false;

        //return this.TestCollisionVsAABB(box, ref c);
    }

}
