using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleHull2D : CollisionHull2D
{

    public CollisionHull2D AABB;
    public CollisionHull2D OBB;
    public CollisionHull2D Circle;

    public CircleHull2D () : base (CollisionHullType2D.Hull_Circle) { }

    [Range(0.0f, 100.0f)]
    public float radius;

    void Start()
    {
        
    }


    void Update()
    {
        TestCollision(this, AABB);
        TestCollision(this, OBB);
        TestCollision(this, Circle);
    }


    public override bool TestCollisionVsCircle(CircleHull2D circle)
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
            Debug.Log("Circ V Circ : true");
            return true;
        }
        else
        {
            Debug.Log("Circ V Circ : false");
            return false;
        }
    }

    public override bool TestCollisionVsAABB(AxisAlignedBoundingBoxHull2D box)
    {
        //Calculate closest point by clamping(??) center; closest point vs circle test
        //
        //1)

        Vector2 circCenter = this.transform.position;

        //when we clamp on each dimension, there are only two dimesnions

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

        //get all corners
        //get the diff between circ center and all corners
        //choose the shortest diff
        //do vs circ

        Vector2 tempDiffOne = bottomLeft - circCenter;
        Vector2 tempDiffTwo = bottomRight - circCenter;
        Vector2 tempDiffThree = topLeft - circCenter;
        Vector2 tempDiffFour = topRight - circCenter;

        float[] diffArr = new float[4];
        diffArr[0] = tempDiffOne.magnitude;
        diffArr[1] = tempDiffTwo.magnitude;
        diffArr[2] = tempDiffThree.magnitude;
        diffArr[3] = tempDiffFour.magnitude;
        */
        Vector2 closestAABBPoint;
        Vector2 diff = new Vector2();
        /*
        if (Mathf.Max(diffArr) == diffArr[0])
        {
            closestAABBPoint = tempDiffOne;
            diff = circCenter - closestAABBPoint;
        }
        if (Mathf.Max(diffArr) == diffArr[1])
        {
            closestAABBPoint = tempDiffTwo;
            diff = circCenter - closestAABBPoint;
        }
        if (Mathf.Max(diffArr) == diffArr[2])
        {
            closestAABBPoint = tempDiffThree;
            diff = circCenter - closestAABBPoint;
        }
        if (Mathf.Max(diffArr) == diffArr[3])
        {
            closestAABBPoint = tempDiffFour;
            diff = circCenter - closestAABBPoint;
        }
        */


        float disSq = (diff.x * diff.x) + (diff.y * diff.y);
        float sumSq = radius * radius; //we are just going to a point with a radius of zero
        if (disSq <= sumSq)
        {
            Debug.Log("Circ V AABB : true");
            return true;
        }
        else
        {
            Debug.Log("Circ V AABB : true");
            return false;
        }
    }

    public override bool TestCollisionVsOBB(ObjectBoundingBoxHull2D box)
    {
        //Same as above, but first:
        //move circle center into box space by * -world transform
        //1) this.position is now * -box.position 
        //2) call testAABB with box

        Vector2 circCenter = new Vector2();
        circCenter.x = this.transform.position.x * -box.transform.position.x;
        circCenter.y = this.transform.position.y * -box.transform.position.y;


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

        //get all corners
        //get the diff between circ center and all corners
        //choose the shortest diff
        //do vs circ

        Vector2 tempDiffOne = bottomLeft - circCenter;
        Vector2 tempDiffTwo = bottomRight - circCenter;
        Vector2 tempDiffThree = topLeft - circCenter;
        Vector2 tempDiffFour = topRight - circCenter;

        float[] diffArr = new float[4];
        diffArr[0] = tempDiffOne.magnitude;
        diffArr[1] = tempDiffTwo.magnitude;
        diffArr[2] = tempDiffThree.magnitude;
        diffArr[3] = tempDiffFour.magnitude;

        Vector2 closestAABBPoint;
        Vector2 diff = new Vector2();

        if (Mathf.Max(diffArr) == diffArr[0])
        {
            closestAABBPoint = tempDiffOne;
            diff = circCenter - closestAABBPoint;
        }
        if (Mathf.Max(diffArr) == diffArr[1])
        {
            closestAABBPoint = tempDiffTwo;
            diff = circCenter - closestAABBPoint;
        }
        if (Mathf.Max(diffArr) == diffArr[2])
        {
            closestAABBPoint = tempDiffThree;
            diff = circCenter - closestAABBPoint;
        }
        if (Mathf.Max(diffArr) == diffArr[3])
        {
            closestAABBPoint = tempDiffFour;
            diff = circCenter - closestAABBPoint;
        }


        float disSq = (diff.x * diff.x) + (diff.y * diff.y);
        float sumSq = radius * radius; //we are just going to a point with a radius of zero
        if (disSq <= sumSq)
        {
            Debug.Log("Circ V OBB : true");
            return true;
        }
        else
        {
            Debug.Log("Circ V OBB : false");
            return false;
        }
    }
}
