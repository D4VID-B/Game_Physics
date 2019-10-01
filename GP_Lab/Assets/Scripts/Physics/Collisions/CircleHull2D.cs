using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleHull2D : CollisionHull2D
{

    public CircleHull2D () : base (CollisionHullType2D.Hull_Circle) { }

    [Range(0.0f, 100.0f)]
    public float radius;

    void Start()
    {
        
    }


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


        return false;
    }

    public override bool TestCollisionVsAABB(AxisAlignedBoundingBoxHull2D box, ref Collision c)
    {
        //Calculate closest point by clamping(??) center; closest point vs circle test
        //
        //1)


        return false;
    }

    public override bool TestCollisionVsOBB(ObjectBoundingBoxHull2D box, ref Collision c)
    {
        //Same as above, but first:
        //move circle center into box space by * -world transform
        //1)

        return false;
    }
}
