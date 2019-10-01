using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisAlignedBoundingBoxHull2D : CollisionHull2D
{

    public AxisAlignedBoundingBoxHull2D() : base(CollisionHullType2D.Hull_AABB) { }

    public float length;
    public float height;

    float calculateSquareDiagonal(float side)
    {
        float diagonal;

        diagonal = 1.41f * side;

        return diagonal;
    }

    public override bool TestCollisionVsCircle(CircleHull2D circle) //David
    {
        //Call circle, flip arguments <=== ///What does this mean\\\?
        
        return circle.TestCollisionVsAABB(this);
    }

    public override bool TestCollisionVsAABB(AxisAlignedBoundingBoxHull2D box) //David
    {
        //on each axis, max extent of one < min extent of other
        //
        //1) Find Max and Min of each box  <=== \\\*** HOW? ***///
        //Max = 
        //Min = 

        //2) Compare max 1 to min 2
        //if(max01 >= min2) is true, continue
        //else, stop - they are not colliding

        //3) Compare max 2 to min 1
        //if(max02 >= min1) is true, the boxes have collided  
        //else, false positive

        //4) Repeat 2 & 3 for the y axis

        //Finding corners/Max-Min of box
        //bottom left
        float box_X = box.transform.position.x - box.length * 0.5f;
        float box_Y = box.transform.position.y - box.height * 0.5f;
        Vector2 box_bottomLeft = new Vector2(box_X, box_Y);

        //top right
        box_X = box.transform.position.x + box.length * 0.5f;
        box_Y = box.transform.position.y + box.height * 0.5f;
        Vector2 box_topRight = new Vector2(box_X, box_Y);

        //Finding corners of this
        //bottom left
        float this_X = transform.position.x - length * 0.5f;
        float this_Y = transform.position.y - height * 0.5f;
        Vector2 this_bottomLeft = new Vector2(this_X, this_Y);

        //top right
        this_X = transform.position.x + length * 0.5f;
        this_Y = transform.position.y + height * 0.5f;
        Vector2 this_topRight = new Vector2(this_X, this_Y);




        return false;
    }

    public override bool TestCollisionVsOBB(ObjectBoundingBoxHull2D box) //David
    {
        //same as above twice:
        //first: find max extents of OBB, do AABB vs this
        //call test aabb

        //Second: transform this into OBB space, find max extents, repat AABB
        //1) transform into OBB space:

        //2) find max and min of [??]

        //3) Call testaabb again

        return false;
    }

}
