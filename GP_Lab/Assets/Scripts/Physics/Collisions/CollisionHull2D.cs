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

    void Update()
    {
        
    }

    public bool TestCollision(CollisionHull2D a, CollisionHull2D b)
    {

        return false;
    }

    public abstract bool TestCollisionVsCircle(CircleHull2D circle);

    public abstract bool TestCollisionVsAABB(AxisAlignedBoundingBoxHull2D box);

    public abstract bool TestCollisionVsOBB(ObjectBoundingBoxHull2D box);
}
