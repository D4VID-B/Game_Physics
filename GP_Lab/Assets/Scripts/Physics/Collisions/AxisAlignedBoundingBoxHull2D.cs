using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisAlignedBoundingBoxHull2D : CollisionHull2D
{

    public AxisAlignedBoundingBoxHull2D() : base(CollisionHullType2D.Hull_AABB) { }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public override bool TestCollisionVsCircle(CircleHull2D circle)
    {
        //Call circle, flip arguments

        return false;
    }

    public override bool TestCollisionVsAABB(AxisAlignedBoundingBoxHull2D box)
    {
        //on each axis, max extent of one < min extent of other
        //
        //1)

        return false;
    }

    public override bool TestCollisionVsOBB(ObjectBoundingBoxHull2D box)
    {
        //same as above twice:
        //first: find max extents of OBB, do AABB vs this
        //Second: transform this into OBB space, find max extents, repat AABB
        //1)

        return false;
    }

}
