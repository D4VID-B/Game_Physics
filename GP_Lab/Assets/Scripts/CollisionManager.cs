using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionManager : MonoBehaviour
{

    Collision desc;

    Scene currentScene;
    List<GameObject> colliders;
    List<GameObject> rootObjects;

    // Start is called before the first frame update
    void Start()
    {
        colliders = new List<GameObject>();
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

    }

}
