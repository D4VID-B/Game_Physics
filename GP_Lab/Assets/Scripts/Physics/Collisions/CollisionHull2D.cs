using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollisionHull2D : MonoBehaviour
{

    public class Collision
    {
        public struct Contact
        {
            public Vector2 point; //Point of contact
            public Vector2 normal; //sin() cos() of position
        }

        //Collision event
        public CollisionHull2D a = null, b = null; //Colliding objects
        public Contact[] contacts = new Contact[4]; //???
        public int contactCount;
        public bool status = false; //has the collision occured?

        float restitution; //0-1, collision elasticity
        public Vector2 closingVelocity; //a.velocity - b.velocity
    }



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

    public static void changeColor(GameObject obj,  bool shouldChange)
    {
        if(shouldChange)
        {
            //Debug.Log("Previous material: " + obj.GetComponent<Renderer>().material);
            obj.GetComponent<MeshRenderer>().material.color = Color.green;
            //Debug.Log("New material: " + obj.GetComponent<Renderer>().material);
        }

        //if(!shouldChange)
        //{
        //    obj.GetComponent<MeshRenderer>().material.color = Color.red;
        //}
        

    }

    public static bool TestCollision(CollisionHull2D a, CollisionHull2D b, ref Collision c)
    {
        //if a is cirlce and b is also circle
        //call the circel-circle function

        //if a is a circle and b is an AABB
        //call the 

        if (a.type == CollisionHull2D.CollisionHullType2D.Hull_Circle && b.type == CollisionHull2D.CollisionHullType2D.Hull_Circle)
        {
            return ((CircleHull2D)a).TestCollisionVsCircle((CircleHull2D)b, ref c);
        }
        else if (a.type == CollisionHull2D.CollisionHullType2D.Hull_Circle && b.type == CollisionHull2D.CollisionHullType2D.Hull_AABB)
        {
            return ((CircleHull2D)a).TestCollisionVsAABB((AxisAlignedBoundingBoxHull2D)b, ref c);
        }
        else if(a.type == CollisionHull2D.CollisionHullType2D.Hull_Circle && b.type == CollisionHull2D.CollisionHullType2D.Hull_OBB)
        {
            return ((CircleHull2D)a).TestCollisionVsOBB((ObjectBoundingBoxHull2D)b, ref c);
        }
        else if(a.type == CollisionHull2D.CollisionHullType2D.Hull_AABB && b.type == CollisionHull2D.CollisionHullType2D.Hull_AABB)
        {
            return ((AxisAlignedBoundingBoxHull2D)a).TestCollisionVsAABB((AxisAlignedBoundingBoxHull2D)b, ref c);
        }
        else if (a.type == CollisionHull2D.CollisionHullType2D.Hull_AABB && b.type == CollisionHull2D.CollisionHullType2D.Hull_OBB)
        {
            return ((AxisAlignedBoundingBoxHull2D)a).TestCollisionVsOBB((ObjectBoundingBoxHull2D)b, ref c); 
        }
        else if (a.type == CollisionHull2D.CollisionHullType2D.Hull_OBB && b.type == CollisionHull2D.CollisionHullType2D.Hull_OBB)
        {
            return ((ObjectBoundingBoxHull2D)a).TestCollisionVsOBB((ObjectBoundingBoxHull2D)b, ref c);
        }


        return false;
    }

    public abstract bool TestCollisionVsCircle(CircleHull2D circle, ref Collision c);

    public abstract bool TestCollisionVsAABB(AxisAlignedBoundingBoxHull2D box, ref Collision c);

    public abstract bool TestCollisionVsOBB(ObjectBoundingBoxHull2D box, ref Collision c);

    public static void updateCollision(ref Collision col)
    {
        
    }
}
