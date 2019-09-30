using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollisionHull2D : MonoBehaviour
{
    public enum CollisionHullType2D
    {
        Hull_Circle,
        Hull_AABB,
        Hull_OBB
    }

    private CollisionHullType2D type { get; }

    protected CollisionHull2D(CollisionHullType2D hullType)
    {
        type = hullType;
    }

    protected Particle2D particle;

    void Start()
    {
        particle = GetComponent<Particle2D>();
    }


    public static bool TestCollision(CollisionHull2D a, CollisionHull2D b) //Decides which collision detection function needs to be called
    {
        //if a is cirlce and b is also circle
            //call the circel-circle function

        //if a is a circle and b is an AABB
            //call the 

        //if a is a circle and b ia an OBB
            // ....



        if(a.type == CollisionHull2D.CollisionHullType2D.Hull_Circle && b.type == CollisionHull2D.CollisionHullType2D.Hull_Circle)
        {
            //do circle-circle collision
            return ((CircleHull2D)a).TestCollisionVsCircle((CircleHull2D)b);
        }
        else if (a.type == CollisionHull2D.CollisionHullType2D.Hull_Circle && b.type == CollisionHull2D.CollisionHullType2D.Hull_AABB)
        {
            return ((CircleHull2D)a).TestCollisionVsAABB((AxisAlignedBoundingBoxHull2D)b);
        }
        else if(a.type == CollisionHull2D.CollisionHullType2D.Hull_Circle && b.type == CollisionHull2D.CollisionHullType2D.Hull_OBB)
        {
            return ((CircleHull2D)a).TestCollisionVsOBB((ObjectBoundingBoxHull2D)b);
        }
        else if(a.type == CollisionHull2D.CollisionHullType2D.Hull_AABB && b.type == CollisionHull2D.CollisionHullType2D.Hull_AABB)
        {
            return ((AxisAlignedBoundingBoxHull2D)a).TestCollisionVsAABB((AxisAlignedBoundingBoxHull2D)b);
        }
        else if (a.type == CollisionHull2D.CollisionHullType2D.Hull_AABB && b.type == CollisionHull2D.CollisionHullType2D.Hull_OBB)
        {
            return ((AxisAlignedBoundingBoxHull2D)a).TestCollisionVsOBB((ObjectBoundingBoxHull2D)b); ;
        }
        else if (a.type == CollisionHull2D.CollisionHullType2D.Hull_OBB && b.type == CollisionHull2D.CollisionHullType2D.Hull_OBB)
        {
            return ((ObjectBoundingBoxHull2D)a).TestCollisionVsOBB((ObjectBoundingBoxHull2D)b);
        }


        return false;
    }

    public abstract bool TestCollisionVsCircle(CircleHull2D circle);

    public abstract bool TestCollisionVsAABB(AxisAlignedBoundingBoxHull2D box);

    public abstract bool TestCollisionVsOBB(ObjectBoundingBoxHull2D box);
}
