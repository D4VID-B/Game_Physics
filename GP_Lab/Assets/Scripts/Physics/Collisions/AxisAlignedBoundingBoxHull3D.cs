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

        Matrix4x4 boxMat = box.gameObject.GetComponent<Particle3D>().worldTransform;


        //THIS BOX
        Vector4 centerThisBox = new Vector4(this.transform.position.x - box.transform.position.x,
                                                this.transform.position.y - box.transform.position.y,
                                                    this.transform.position.z - box.transform.position.z, 1.0f);
        centerThisBox = boxMat * centerThisBox;

        float halfLengthThisBox = this.length * 0.5f;
        float halfHeightThisBox = this.height * 0.5f;
        float halfDepthThisBox = this.depth * 0.5f;

        //get the corners
        Vector4 thisBoxCorner1 = centerThisBox + new Vector4(-halfLengthThisBox, halfHeightThisBox, -halfDepthThisBox, 0.0f);  //leave it as 0.0f on end otherwise addition will make for 2.0f = w
        Vector4 thisBoxCorner2 = centerThisBox + new Vector4(-halfLengthThisBox, halfHeightThisBox, halfDepthThisBox, 0.0f);
        Vector4 thisBoxCorner3 = centerThisBox + new Vector4(halfLengthThisBox, halfHeightThisBox, halfDepthThisBox, 0.0f);
        Vector4 thisBoxCorner4 = centerThisBox + new Vector4(halfLengthThisBox, halfHeightThisBox, -halfDepthThisBox, 0.0f);
        Vector4 thisBoxCorner5 = centerThisBox + new Vector4(-halfLengthThisBox, -halfHeightThisBox, -halfDepthThisBox, 0.0f);
        Vector4 thisBoxCorner6 = centerThisBox + new Vector4(-halfLengthThisBox, -halfHeightThisBox, halfDepthThisBox, 0.0f);
        Vector4 thisBoxCorner7 = centerThisBox + new Vector4(halfLengthThisBox, -halfHeightThisBox, halfDepthThisBox, 0.0f);
        Vector4 thisBoxCorner8 = centerThisBox + new Vector4(halfLengthThisBox, -halfHeightThisBox, -halfDepthThisBox, 0.0f);

        //thisBoxCorner1 = thisBoxMat * thisBoxCorner1;
        //thisBoxCorner2 = thisBoxMat * thisBoxCorner2;
        //thisBoxCorner3 = thisBoxMat * thisBoxCorner3;
        //thisBoxCorner4 = thisBoxMat * thisBoxCorner4;
        //thisBoxCorner5 = thisBoxMat * thisBoxCorner5;
        //thisBoxCorner6 = thisBoxMat * thisBoxCorner6;
        //thisBoxCorner7 = thisBoxMat * thisBoxCorner7;
        //thisBoxCorner8 = thisBoxMat * thisBoxCorner8;

        Debug.DrawLine(thisBoxCorner1, thisBoxCorner2, Color.blue);
        Debug.DrawLine(thisBoxCorner2, thisBoxCorner3, Color.blue);
        Debug.DrawLine(thisBoxCorner3, thisBoxCorner4, Color.blue);
        Debug.DrawLine(thisBoxCorner4, thisBoxCorner1, Color.blue);

        Debug.DrawLine(thisBoxCorner5, thisBoxCorner6, Color.blue);
        Debug.DrawLine(thisBoxCorner6, thisBoxCorner7, Color.blue);
        Debug.DrawLine(thisBoxCorner7, thisBoxCorner8, Color.blue);
        Debug.DrawLine(thisBoxCorner8, thisBoxCorner5, Color.blue);

        Debug.DrawLine(thisBoxCorner1, thisBoxCorner5, Color.blue);
        Debug.DrawLine(thisBoxCorner2, thisBoxCorner6, Color.blue);
        Debug.DrawLine(thisBoxCorner3, thisBoxCorner7, Color.blue);
        Debug.DrawLine(thisBoxCorner4, thisBoxCorner8, Color.blue);


        //AABB Test:

        bool colOnX = false;
        bool colOnY = false;
        bool colOnZ = false;

        if (centerThisBox.x + (this.length * 0.5f) >= box.transform.position.x - (box.length * 0.5f) 
            && box.transform.position.x + (box.length * 0.5f) >= centerThisBox.x - (this.length * 0.5f))
        {
            colOnX = true;
        }

        if (centerThisBox.y + (this.height * 0.5f) >= box.transform.position.y - (box.length * 0.5f) 
            && box.transform.position.y + (box.height * 0.5f) >= centerThisBox.y - (this.length * 0.5f))
        {
            colOnY = true;
        }

        if (centerThisBox.z + (this.depth * 0.5f) >= box.transform.position.z - (box.depth * 0.5f) 
            && box.transform.position.z + (box.depth * 0.5f) >= centerThisBox.z - (this.depth * 0.5f))
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

}
