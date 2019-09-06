using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour
{

    public Vector3 position01, position02, position03;

    public GameObject particle02, particle03;

    void FixedUpdate()
    {
        if(particle02.transform.position.x >= gameObject.transform.position.x)
        {
            particle02.GetComponent<Particle2D>().position = position02;
        }

        if (particle03.transform.position.x >= gameObject.transform.position.x)
        {
            particle03.GetComponent<Particle2D>().position = position03;
        }

    }
}
