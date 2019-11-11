using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisAlignedBoundingBoxHull3D : CollisionHull3D
{

    public AxisAlignedBoundingBoxHull3D() : base(CollisionHullType3D.Hull_AABB) { }

    [Range(0, 3)]
    public float length;
    [Range(0, 3)]
    public float height;
    [Range(0, 3)]
    public float depth;



    public override bool TestCollisionVsCircle(CircleHull3D circle, ref Collision c)
    {
        //Call circle, flip arguments <=== ///What does this mean\\\?
        
        return circle.TestCollisionVsAABB(this, ref c);
    }

    public override bool TestCollisionVsAABB(AxisAlignedBoundingBoxHull3D box, ref Collision c)
    {
        //on each axis, max extent of one < min extent of other
        //
        //1) Find Max and Min of each box  
        //Max = 
        //Min = 

        //2) Compare max 1 to min 2
        //if(max01 >= min2) is true, continue
        //else, stop - they are not colliding

        //3) Compare max 2 to min 1
        //if(max02 >= min1) is true, the boxes have collided  
        //else, false positive

        //4) Repeat 2 & 3 for the y axis


        //AABB, finding corners(sides) in if()
        bool colOnX = false;
        bool colOnY = false;
        bool colOnZ = false;

        if (this.transform.position.x + (this.length * 0.5f) >= box.transform.position.x - (box.length * 0.5f) && box.transform.position.x + (box.length * 0.5f) >= this.transform.position.x - (this.length * 0.5f))
        {
            colOnX = true;
        }

        if (this.transform.position.y + (this.height * 0.5f) >= box.transform.position.y - (box.length * 0.5f) && box.transform.position.y + (box.height * 0.5f) >= this.transform.position.y - (this.length * 0.5f))
        {
            colOnY = true;
        }

        if(this.transform.position.z + (this.depth * 0.5f) >= box.transform.position.z - (box.depth * 0.5f) && box.transform.position.z + (box.depth * 0.5f) >= this.transform.position.z - (this.depth * 0.5f))
        {
            colOnZ = true;
        }

        if (colOnY && colOnX && colOnZ)
        {
            return true;
            
        }
        else
        {
            return false;
        }

    }

    public override bool TestCollisionVsOBB(ObjectBoundingBoxHull3D box, ref Collision c)
    {
        /*
        same as above twice:
        first: find max extents of OBB, do AABB vs this
        call test aabb

        Second: transform this into OBB space, find max extents, repat AABB
        1) transform into OBB space:

        2) find max and min of [??]

        3) Call testaabb again
        */

        //change basis of box:

        Vector3 pos = box.transform.position;
        Quaternion rot = box.transform.rotation;
        Vector3 scl = box.transform.localScale;

        Vector3.Dot(pos, new Vector3(worldTransform.m03, worldTransform.m13, worldTransform.m23));
        Vector3.Dot(scl, new Vector3(worldTransform.m00, worldTransform.m11, worldTransform.m22));

        //Call the AABB - AABB test
        //return TestCollisionVsAABB((AxisAlignedBoundingBoxHull3D)box, ref c);
        return false;
    }

}
