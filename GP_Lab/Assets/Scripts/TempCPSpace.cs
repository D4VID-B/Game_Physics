//Source w/ original code: https://forum.unity.com/threads/looking-for-efficient-obb-aabb-3d-collider-solutions.544439/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 3D Box collision
/// </summary>
public class TempCPSpace : MonoBehaviour
{

    Transform AABB;

    Transform OBB;


    // Update is called once per frame
    public bool CheckOBB_AABB(GameObject aabb, GameObject obb)
    {
        AABB = aabb.transform;
        OBB = obb.transform;

            Vector3[] aabbCorners = _CornersAABB(AABB);
            Vector3[] obbCorners = _CornersOBB(AABB);

            // then consider axis rotation in calculations
            bool isColliding = _ABB2OBBCollisionTest(aabbCorners, obbCorners);

            if (isColliding)
            {
            // Debug.Log ( "AABB + OBB" ) ;
            return true;
            }
            else
            {
            // Debug.Log ( "ABB + AABB" ) ;
            return false;
            }
        

    }

    private Vector3[] _CornersAABB(Transform tr)
    {


        Vector3[] a_corners = new Vector3[8];

        a_corners[0] = tr.position + new Vector3(-tr.localScale.x, -tr.localScale.y, -tr.localScale.z) * 0.5f;
        a_corners[1] = tr.position + new Vector3(tr.localScale.x, -tr.localScale.y, -tr.localScale.z) * 0.5f;
        a_corners[2] = tr.position + new Vector3(tr.localScale.x, -tr.localScale.y, tr.localScale.z) * 0.5f;
        a_corners[3] = tr.position + new Vector3(-tr.localScale.x, -tr.localScale.y, tr.localScale.z) * 0.5f;
        a_corners[4] = tr.position + new Vector3(-tr.localScale.x, tr.localScale.y, -tr.localScale.z) * 0.5f;
        a_corners[5] = tr.position + new Vector3(tr.localScale.x, tr.localScale.y, -tr.localScale.z) * 0.5f;
        a_corners[6] = tr.position + new Vector3(tr.localScale.x, tr.localScale.y, tr.localScale.z) * 0.5f;
        a_corners[7] = tr.position + new Vector3(-tr.localScale.x, tr.localScale.y, tr.localScale.z) * 0.5f;

        return a_corners;
    }

    private Vector3[] _CornersOBB(Transform tr)
    {


        Vector3[] a_corners = new Vector3[8];

        a_corners[0] = tr.position + tr.rotation * (new Vector3(-tr.localScale.x, -tr.localScale.y, -tr.localScale.z) * 0.5f);
        a_corners[1] = tr.position + tr.rotation * (new Vector3(tr.localScale.x, -tr.localScale.y, -tr.localScale.z) * 0.5f);
        a_corners[2] = tr.position + tr.rotation * (new Vector3(tr.localScale.x, -tr.localScale.y, tr.localScale.z) * 0.5f);
        a_corners[3] = tr.position + tr.rotation * (new Vector3(-tr.localScale.x, -tr.localScale.y, tr.localScale.z) * 0.5f);
        a_corners[4] = tr.position + tr.rotation * (new Vector3(-tr.localScale.x, tr.localScale.y, -tr.localScale.z) * 0.5f);
        a_corners[5] = tr.position + tr.rotation * (new Vector3(tr.localScale.x, tr.localScale.y, -tr.localScale.z) * 0.5f);
        a_corners[6] = tr.position + tr.rotation * (new Vector3(tr.localScale.x, tr.localScale.y, tr.localScale.z) * 0.5f);
        a_corners[7] = tr.position + tr.rotation * (new Vector3(-tr.localScale.x, tr.localScale.y, tr.localScale.z) * 0.5f);

        return a_corners;
    }

    //private Vector3[] _MinMaxAABB(Transform tr)
    //{

    //    Vector3 V3_min = tr.position + new Vector3(-tr.localScale.x, -tr.localScale.y, -tr.localScale.z) * 0.5f;
    //    Vector3 V3_max = tr.position + new Vector3(tr.localScale.x, tr.localScale.y, tr.localScale.z) * 0.5f;


    //    // pass properties to an array
    //    Vector3[] V3_a = new Vector3[2];
    //    V3_a[0] = V3_min;
    //    V3_a[1] = V3_max;

    //    return V3_a;

    //}
    //private Vector3[] _MinMaxOBB(Transform tr, Bounds bounds)
    //{

    //    Vector3 V3_min = bounds.min;
    //    Vector3 V3_max = bounds.max;


    //    // pass properties to an array
    //    Vector3[] V3_a = new Vector3[2];
    //    V3_a[0] = V3_min;
    //    V3_a[1] = V3_max;

    //    return V3_a;
    //}

    // aCorn and bCorn are arrays containing all corners (vertices) of the two OBBs
    private static bool _IntersectsWhenProjected(Vector3[] aCorn, Vector3[] bCorn, Vector3 V3_axis)
    {

        // Handles the cross product = {0,0,0} case
        if (V3_axis == Vector3.zero)
            return true;

        float aMin = float.MaxValue;
        float aMax = float.MinValue;
        float bMin = float.MaxValue;
        float bMax = float.MinValue;

        // Define two intervals, a and b. Calculate their min and max values
        for (int i = 0; i < 8; i++)
        {

            float aDist = Vector3.Dot(aCorn[i], V3_axis);
            aMin = (aDist < aMin) ? aDist : aMin;
            aMax = (aDist > aMax) ? aDist : aMax;
            float bDist = Vector3.Dot(bCorn[i], V3_axis);
            bMin = (bDist < bMin) ? bDist : bMin;
            bMax = (bDist > bMax) ? bDist : bMax;
        }

        // One-dimensional intersection test between a and b
        float longSpan = Mathf.Max(aMax, bMax) - Mathf.Min(aMin, bMin);
        float sumSpan = aMax - aMin + bMax - bMin;


        return longSpan < sumSpan; // Change this to <= if you want the case were they are touching but not overlapping, to count as an intersection
    }


    private bool _ABB2OBBCollisionTest(Vector3[] aabbCorners, Vector3[] obbCorners)
    {

        Vector3 V3_frwAABB = Vector3.forward;
        Vector3 V3_frwOBB = OBB.rotation * Vector3.forward;
        Vector3 V3_frwFrw = Vector3.Cross(V3_frwAABB, V3_frwOBB);
        if (!_IntersectsWhenProjected(aabbCorners, obbCorners, V3_frwFrw)) return false;
        // Debug.Log ( "1" ) ;

        Vector3 V3_upAABB = Vector3.up;
        Vector3 V3_upFrw = Vector3.Cross(V3_upAABB, V3_frwOBB);
        if (!_IntersectsWhenProjected(aabbCorners, obbCorners, V3_upFrw)) return false;
        // Debug.Log ( "2" ) ;

        Vector3 V3_rightAABB = Vector3.right;
        Vector3 V3_rightFrw = Vector3.Cross(V3_rightAABB, V3_frwOBB);
        if (!_IntersectsWhenProjected(aabbCorners, obbCorners, V3_rightFrw)) return false;
        // Debug.Log ( "3" ) ;

        Vector3 V3_upOBB = OBB.rotation * Vector3.up;
        Vector3 V3_upUp = Vector3.Cross(V3_upAABB, V3_upOBB);
        if (!_IntersectsWhenProjected(aabbCorners, obbCorners, V3_upUp)) return false;
        //Debug.Log ( "4" ) ;

        Vector3 V3_frwUp = Vector3.Cross(V3_frwAABB, V3_upOBB);
        if (!_IntersectsWhenProjected(aabbCorners, obbCorners, V3_frwUp)) return false;
        //Debug.Log ( "5" ) ;

        Vector3 V3_rightUp = Vector3.Cross(V3_rightAABB, V3_upOBB);
        if (!_IntersectsWhenProjected(aabbCorners, obbCorners, V3_rightUp)) return false;
        //Debug.Log ( "6" ) ;

        Vector3 V3_rightOBB = OBB.rotation * Vector3.right;
        Vector3 V3_frwRight = Vector3.Cross(V3_frwAABB, V3_rightOBB);
        if (!_IntersectsWhenProjected(aabbCorners, obbCorners, V3_frwRight)) return false;
        //Debug.Log ( "7" ) ;

        Vector3 V3_upRight = Vector3.Cross(V3_upAABB, V3_rightOBB);
        if (!_IntersectsWhenProjected(aabbCorners, obbCorners, V3_upRight)) return false;
        //Debug.Log ( "8" ) ;

        Vector3 V3_rightRight = Vector3.Cross(V3_rightAABB, V3_rightOBB);
        if (!_IntersectsWhenProjected(aabbCorners, obbCorners, V3_rightRight)) return false;
        //Debug.Log ( "9" ) ;

        return true;
    }
}
