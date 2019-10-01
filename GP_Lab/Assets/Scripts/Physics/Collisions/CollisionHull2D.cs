using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollisionHull2D : MonoBehaviour
{

    public class Collision
    {
        public struct Contact
        {
            Vector2 point;
            Vector2 normal;
            float restitution;

        }

        //Collision event
        public CollisionHull2D a = null, b = null;
        public Contact[] contacts = new Contact[4];
        public int contactCount;
        public bool status = false;

        public Vector2 closingVelocity;
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

    void Update()
    {
        
    }

    public static bool TestCollision(CollisionHull2D a, CollisionHull2D b, ref Collision c)
    {

        return false;
    }

    public abstract bool TestCollisionVsCircle(CircleHull2D circle, ref Collision c);

    public abstract bool TestCollisionVsAABB(AxisAlignedBoundingBoxHull2D box, ref Collision c);

    public abstract bool TestCollisionVsOBB(ObjectBoundingBoxHull2D box, ref Collision c);
}
