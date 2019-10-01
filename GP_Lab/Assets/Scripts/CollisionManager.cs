using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionManager : MonoBehaviour
{

    Collision desc;

    Scene currentScene;
    List<CollisionHull2D> colliders;
    List<GameObject> rootObjects;

    // Start is called before the first frame update
    void Start()
    {
        colliders = new List<CollisionHull2D>();
        rootObjects = new List<GameObject>();
        currentScene.GetRootGameObjects(rootObjects);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject obj in rootObjects)
        {
            if(obj.tag == "Collider")
            {
                colliders.Add(obj);
            }
        }

        checkForCollisions();
    }

    void checkForCollisions()
    {
        for(int i = 0; i < colliders.Count; i++)
        {
            for(int j = 0; j < colliders.Count; j++)
            {
                //Check if they are the same object
                if(colliders[i].gameObject == colliders[j].gameObject)
                {
                    //Do nothing - these are the same object
                }
                else
                {
                    CollisionHull2D.TestCollision(colliders[i], colliders[j]);
                }
            }
        }
    }

}
