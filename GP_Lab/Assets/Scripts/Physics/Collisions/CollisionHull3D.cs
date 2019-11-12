using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollisionHull3D : MonoBehaviour
{

    public class Collision
    {
        public struct Contact
        {
            public Vector3 point; //Point of contact
            public Vector3 normal; //sin() cos() of position
        }

        //Collision event
        public CollisionHull3D a = null, b = null; //Colliding objects
        public Contact[] contacts = new Contact[4]; //???
        public int contactCount;
        public bool status = false; //has the collision occured?

        public float interpenDepth;
        public float restitution; //0-1, collision elasticity
        public Vector3 closingVelocity; //a.velocity - b.velocity

    }



    public enum CollisionHullType3D
    {
        Hull_Sphere,
        Hull_AABB,
        Hull_OBB
    }

    private CollisionHullType3D type { get; }

    protected CollisionHull3D(CollisionHullType3D hullType)
    {
        type = hullType;
    }

    protected Particle3D particle;



#region World Transform Stuff


    public Matrix4x4 worldTransform, worldTransformInv;

    /// <summary>
    /// Change first parameter obb to second paramter obb's space
    /// </summary>
    /// <param name="obb"></param>
    /// <param name="obb2"></param>
    protected void changeBasis(ObjectBoundingBoxHull3D obb, ObjectBoundingBoxHull3D obb2)
    {
        //assume the first param is this collisionHullObjects obb

        //Particle3D  secParamP3D = obb2.gameObject.GetComponent<Particle3D>();

       // obb.worldTransform = obb2.worldTransform * obb.worldTransform * obb2.worldTransformInv;
    }

    /// <summary>
    /// Change obb to circle's space
    /// </summary>
    /// <param name="obb"></param>
    /// <param name="circle"></param>
    protected void changeBasis(ObjectBoundingBoxHull3D obb, CircleHull3D circle)
    {
        //Debug.Log("before");
        //obb.worldTransform = circle.worldTransform * obb.worldTransform * circle.worldTransformInv;
        //circle.worldTransform = obb.worldTransformInv 
    }

    /// <summary>
    /// Change obb to the aabb's space
    /// </summary>
    /// <param name="obb"></param>
    /// <param name="aabb"></param>
    protected void changeBasis(ObjectBoundingBoxHull3D obb, AxisAlignedBoundingBoxHull3D aabb)
    {
        //obb.worldTransform = aabb.worldTransform * obb.worldTransform * aabb.worldTransform;
    }
#endregion

    void Start()
    {
        particle = this.gameObject.GetComponent<Particle3D>();

    }

    void Update()
    {
        //worldTransform = particle.worldTransform;
        //worldTransformInv = particle.inverseWorldTransform;
    }

    public static void changeColor(GameObject obj,  bool shouldChange)
    {
        if(shouldChange == true)
        {
            //Debug.Log("Color: " + obj.GetComponent<Renderer>().material.color);
            obj.GetComponent<Renderer>().material.color = Color.green;
            //obj.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            //Debug.Log("Color: " + obj.GetComponent<Renderer>().material.color);
        }
        else
        {
            obj.GetComponent<Renderer>().material.color = Color.red;
        }
        

    }

    public static bool TestCollision(CollisionHull3D a, CollisionHull3D b, ref Collision c)
    {
        
        //You need the nine conditions seperately, you cant even use or statements
        //the original way this was set up, if a was circ and b was AABB, then there wasn't a condition for a to be AABB and b to be circ (which matters for the return)

        if (a.type == CollisionHull3D.CollisionHullType3D.Hull_Sphere && b.type == CollisionHull3D.CollisionHullType3D.Hull_Sphere)
        {
            return ((CircleHull3D)a).TestCollisionVsCircle((CircleHull3D)b, ref c);
        }

        else if (a.type == CollisionHull3D.CollisionHullType3D.Hull_Sphere && b.type == CollisionHull3D.CollisionHullType3D.Hull_AABB)
        {
            return ((CircleHull3D)a).TestCollisionVsAABB((AxisAlignedBoundingBoxHull3D)b, ref c);
        }

        else if (b.type == CollisionHull3D.CollisionHullType3D.Hull_Sphere && a.type == CollisionHull3D.CollisionHullType3D.Hull_AABB)
        {
            return ((CircleHull3D)b).TestCollisionVsAABB((AxisAlignedBoundingBoxHull3D)a, ref c);
        }

        else if(a.type == CollisionHull3D.CollisionHullType3D.Hull_Sphere && b.type == CollisionHull3D.CollisionHullType3D.Hull_OBB)
        {
            return ((CircleHull3D)a).TestCollisionVsOBB((ObjectBoundingBoxHull3D)b, ref c);
        }

        else if (b.type == CollisionHull3D.CollisionHullType3D.Hull_Sphere && a.type == CollisionHull3D.CollisionHullType3D.Hull_OBB)
        {
            return ((CircleHull3D)b).TestCollisionVsOBB((ObjectBoundingBoxHull3D)a, ref c);
        }

        else if(a.type == CollisionHull3D.CollisionHullType3D.Hull_AABB && b.type == CollisionHull3D.CollisionHullType3D.Hull_AABB)
        {
            return ((AxisAlignedBoundingBoxHull3D)a).TestCollisionVsAABB((AxisAlignedBoundingBoxHull3D)b, ref c);
        }

        else if (a.type == CollisionHull3D.CollisionHullType3D.Hull_AABB && b.type == CollisionHull3D.CollisionHullType3D.Hull_OBB)
        {
            return ((AxisAlignedBoundingBoxHull3D)a).TestCollisionVsOBB((ObjectBoundingBoxHull3D)b, ref c);
        }

        else if (b.type == CollisionHull3D.CollisionHullType3D.Hull_AABB && a.type == CollisionHull3D.CollisionHullType3D.Hull_OBB)
        {
            return ((AxisAlignedBoundingBoxHull3D)b).TestCollisionVsOBB((ObjectBoundingBoxHull3D)a, ref c);
        }

        else if (a.type == CollisionHull3D.CollisionHullType3D.Hull_OBB && b.type == CollisionHull3D.CollisionHullType3D.Hull_OBB)
        {
            return ((ObjectBoundingBoxHull3D)a).TestCollisionVsOBB((ObjectBoundingBoxHull3D)b, ref c);
        }


        return false;
    }

    public abstract bool TestCollisionVsCircle(CircleHull3D circle, ref Collision c);

    public abstract bool TestCollisionVsAABB(AxisAlignedBoundingBoxHull3D box, ref Collision c);

    public abstract bool TestCollisionVsOBB(ObjectBoundingBoxHull3D box, ref Collision c);

    public static void updateCollision(ref Collision col, float coeff)
    {
        col.restitution = coeff;
        col.closingVelocity = -col.restitution * Vector3.Scale((col.a.GetComponent<Particle3D>().velocity - col.b.GetComponent<Particle3D>().velocity), col.contacts[0].normal);
        

        col.a.GetComponent<Particle3D>().velocity = col.closingVelocity * 1/col.a.GetComponent<Particle3D>().startingMass;//Inverse mass   
        col.b.GetComponent<Particle3D>().velocity = -col.closingVelocity * 1/col.b.GetComponent<Particle3D>().startingMass;
    }

    /*
     Move col.b by the interpenetration depth in the direction of the collision normal
         */
    public static void resolveInterpenetration(ref Collision col)
    {
            col.b.transform.Translate(col.contacts[0].normal * col.interpenDepth * -1);
    }
}
