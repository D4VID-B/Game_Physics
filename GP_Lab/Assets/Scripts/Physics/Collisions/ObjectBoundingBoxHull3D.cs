using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBoundingBoxHull3D : CollisionHull3D
{
    public float length;
    public float height;
    public float depth;

    public ObjectBoundingBoxHull3D(): base(CollisionHullType3D.Hull_OBB) { }

   

    public override bool TestCollisionVsCircle(CircleHull3D circle, ref Collision c)
    {
        return circle.TestCollisionVsOBB(this, ref c);
    }

    public override bool TestCollisionVsAABB(AxisAlignedBoundingBoxHull3D box, ref Collision c)
    {
        return box.TestCollisionVsOBB(this, ref c);
    }

    public override bool TestCollisionVsOBB(ObjectBoundingBoxHull3D box, ref Collision c)
    {
        //AABB-OBB part 2 twice:

        //Second: transform this into OBB space, find max extents, repat AABB
        //1) transform into OBB space:

        //2) find max and min of [??]

        //3) Call testaabb again

        changeBasis(box, this);



        return false;
    }

}
