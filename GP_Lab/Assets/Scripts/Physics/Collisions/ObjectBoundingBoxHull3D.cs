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
        /*
        AABB-OBB part 2 twice:

        Second: transform this into OBB space, find max extents, repat AABB
        1) transform into OBB space:

        2) find max and min of [??]

        3) Call testaabb again
        */

        //Convert one of the obbs (prob "this") through change of basis
        //call testAABBOBB w/ this and box

        //positions of points (THIS ASSUMES THE GOD DAMN BOX IS AABB, how do we use the rotMat of boxWorldTransform to adjust this)

        //   BOX CORNER REP (i made this myself thank you)
        //          1              4
        //           _____________
        //         /|            /|
        //    2  /__L_______3_ /  | 
        //      |   |         |   | 
        //      |   |         |   |
        //      | 5 |_________|___| 8
        //      |  /          |  /
        //      | /           | /
        //      |/____________|/
        //    6              7            


        //WorldMats and invWorldMats
        Matrix4x4 thisBoxMat = this.gameObject.GetComponent<Particle3D>().worldTransform;
        Matrix4x4 thisInvBoxMat = this.gameObject.GetComponent<Particle3D>().inverseWorldTransform;
        Matrix4x4 boxMat = box.gameObject.GetComponent<Particle3D>().worldTransform;
        Matrix4x4 invBoxMat = box.gameObject.GetComponent<Particle3D>().inverseWorldTransform;

        //Vector4 sphereCenterPoint = new Vector4(sphereCenter.m03, sphereCenter.m13, sphereCenter.m23, 1.0f);
        //sphereCenterPoint = new Vector4(sphereCenterPoint.x - box.transform.position.x, sphereCenterPoint.y - box.transform.position.y, sphereCenterPoint.z - box.transform.position.z, 1.0f);
        //sphereCenterPoint = boxMAT * sphereCenterPoint;

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
        


        //INCOMING BOX
        Vector4 centerBox = new Vector4(box.transform.position.x - this.transform.position.x,
                                            box.transform.position.y - this.transform.position.y, 
                                                box.transform.position.z - this.transform.position.z, 1.0f);

        centerBox = thisBoxMat * centerBox;

        float halfLengthBox = box.length * 0.5f;
        float halfHeightBox = box.height * 0.5f;
        float halfDepthBox = box.depth * 0.5f;

        //get the corners
        Vector4 boxCorner1 = centerBox + new Vector4(-halfLengthBox, halfHeightBox, -halfDepthBox, 0.0f);  //leave it as 0.0f on end otherwise addition will make for 2.0f = w
        Vector4 boxCorner2 = centerBox + new Vector4(-halfLengthBox, halfHeightBox, halfDepthBox, 0.0f);
        Vector4 boxCorner3 = centerBox + new Vector4(halfLengthBox, halfHeightBox, halfDepthBox, 0.0f);
        Vector4 boxCorner4 = centerBox + new Vector4(halfLengthBox, halfHeightBox, -halfDepthBox, 0.0f);
        Vector4 boxCorner5 = centerBox + new Vector4(-halfLengthBox, -halfHeightBox, -halfDepthBox, 0.0f);
        Vector4 boxCorner6 = centerBox + new Vector4(-halfLengthBox, -halfHeightBox, halfDepthBox, 0.0f);
        Vector4 boxCorner7 = centerBox + new Vector4(halfLengthBox, -halfHeightBox, halfDepthBox, 0.0f);
        Vector4 boxCorner8 = centerBox + new Vector4(halfLengthBox, -halfHeightBox, -halfDepthBox, 0.0f);


        //boxCorner1 = boxMat * boxCorner1;
        //boxCorner2 = boxMat * boxCorner2;
        //boxCorner3 = boxMat * boxCorner3;
        //boxCorner4 = boxMat * boxCorner4;
        //boxCorner5 = boxMat * boxCorner5;
        //boxCorner6 = boxMat * boxCorner6;
        //boxCorner7 = boxMat * boxCorner7;
        //boxCorner8 = boxMat * boxCorner8;
        


        Debug.DrawLine(boxCorner1, boxCorner2, Color.blue);
        Debug.DrawLine(boxCorner2, boxCorner3, Color.blue);
        Debug.DrawLine(boxCorner3, boxCorner4, Color.blue);
        Debug.DrawLine(boxCorner4, boxCorner1, Color.blue);

        Debug.DrawLine(boxCorner5, boxCorner6, Color.blue);
        Debug.DrawLine(boxCorner6, boxCorner7, Color.blue);
        Debug.DrawLine(boxCorner7, boxCorner8, Color.blue);
        Debug.DrawLine(boxCorner8, boxCorner5, Color.blue);

        Debug.DrawLine(boxCorner1, boxCorner5, Color.blue);
        Debug.DrawLine(boxCorner2, boxCorner6, Color.blue);
        Debug.DrawLine(boxCorner3, boxCorner7, Color.blue);
        Debug.DrawLine(boxCorner4, boxCorner8, Color.blue);

        /*
        float boxXMin = (box.transform.position.x - (box.length * 0.5f));
        float boxYMin = (box.transform.position.y - (box.height * 0.5f));
        float boxZMin = (box.transform.position.z - (box.depth * 0.5f));

        float boxXMax = (box.transform.position.x + (box.length * 0.5f));
        float boxYMax = (box.transform.position.y + (box.height * 0.5f));
        float boxZMax = (box.transform.position.z + (box.depth * 0.5f));
        */

        /*
        Vector4 boxXminVect = new Vector4(boxXMin, box.transform.position.y, box.transform.position.z, 1.0f);
        Vector4 boxYminVect = new Vector4(box.transform.position.x, boxYMin, box.transform.position.z, 1.0f);
        Vector4 boxZminVect = new Vector4(box.transform.position.x, box.transform.position.y, boxZMin, 1.0f);

        Vector4 boxXmaxVect = new Vector4(boxXMax, box.transform.position.y, box.transform.position.z, 1.0f);
        Vector4 boxYmaxVect = new Vector4(box.transform.position.x, boxYMax, box.transform.position.z, 1.0f);
        Vector4 boxZmaxVect = new Vector4(box.transform.position.x, box.transform.position.y, boxZMax, 1.0f);

        */


        bool colOnX = false;
        bool colOnY = false;
        bool colOnZ = false;


        //AABB test
        if (centerThisBox.x + (this.length * 0.5f) >= centerBox.x - (box.length * 0.5f) 
                && centerBox.x + (box.length * 0.5f) >= centerThisBox.x - (this.length * 0.5f))
        {
            colOnX = true;
        }

        if (centerThisBox.y + (this.height * 0.5f) >= centerBox.y - (box.length * 0.5f) 
                && centerBox.y + (box.height * 0.5f) >= centerThisBox.y - (this.length * 0.5f))
        {
            colOnY = true;
        }

        if (centerThisBox.z + (this.depth * 0.5f) >= centerBox.z - (box.depth * 0.5f) 
                && centerBox.z + (box.depth * 0.5f) >= centerThisBox.z - (this.depth * 0.5f))
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
