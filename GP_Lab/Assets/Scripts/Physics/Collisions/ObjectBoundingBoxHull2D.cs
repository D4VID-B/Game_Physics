using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBoundingBoxHull2D : CollisionHull2D
{
    public ObjectBoundingBoxHull2D(): base(CollisionHullType2D.Hull_OBB) { }

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
        //Use circle
        return false;
    }

    public override bool TestCollisionVsAABB(AxisAlignedBoundingBoxHull2D box)
    {
        //Use AABB
        return false;
    }

    public override bool TestCollisionVsOBB(ObjectBoundingBoxHull2D box) //David
    {
        //AABB-OBB part 2 twice
        return false;
    }

}
