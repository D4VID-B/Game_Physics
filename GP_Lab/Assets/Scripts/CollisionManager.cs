using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionManager : MonoBehaviour
{

    CollisionHull2D.Collision desc;

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
                //Convert to collisionHull and add to the list
                colliders.Add(obj.GetComponent<CollisionHull2D>());
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
                    //Do nothing - they are the same object
                }
                else
                {
                    if (CollisionHull2D.TestCollision(colliders[i], colliders[j], ref desc))
                    {

                    }
                    else
                    {

                    }
                }
            }
        }
    }

}
