using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionManager : MonoBehaviour
{

    public static CollisionManager instance;

    CollisionHull2D.Collision desc;

    Scene currentScene;
    public List<CollisionHull2D> colliders;
    List<GameObject> rootObjects;

    public Material success, fail;

    public bool enableManager = true;
    public bool shouldChangeColor;

    [Range(0.0f, 1.0f)]
    public float coefficientOfRestitution = 0.8f;

    // Start is called before the first frame update
    void Start()
    {
        desc = new CollisionHull2D.Collision();
        currentScene = SceneManager.GetActiveScene();

        colliders = new List<CollisionHull2D>();
        rootObjects = new List<GameObject>();
        currentScene.GetRootGameObjects(rootObjects);

        findHulls();
    }

    // Update is called once per frame
    void Update()
    {   
        if(enableManager)
        {
            checkForCollisions();
        }
        
    }

    void findHulls()
    {
        CollisionHull2D hull;
        foreach (GameObject obj in rootObjects)
        {
            hull = obj.GetComponent<CollisionHull2D>();
            if (hull != null)
            {
                colliders.Add(hull);
            }
        }
    }

    void checkForCollisions()
    {
        for(int i = 0; i < colliders.Count; i++)
        {
            for(int j = i+1; j < colliders.Count; j++)
            {
                if (CollisionHull2D.TestCollision(colliders[i], colliders[j], ref desc))
                {
                    if(shouldChangeColor)
                    {
                        CollisionHull2D.changeColor(colliders[i].gameObject, true);
                        CollisionHull2D.changeColor(colliders[j].gameObject, true);
                    }

                    CollisionHull2D.resolveInterpenetration(ref desc);
                    CollisionHull2D.updateCollision(ref desc, coefficientOfRestitution);

                    //Kill the player
                    GameObject.Find("CollisionManagerHost").GetComponent<GameManagerScript>().hitByAstroid = true;
                    

                    Debug.Log("Objects collided");
                }
                else
                {
                    if(shouldChangeColor)
                    {
                        CollisionHull2D.changeColor(colliders[i].gameObject, false);
                        CollisionHull2D.changeColor(colliders[j].gameObject, false);
                    }
                    
                }
            }
        }
    }

}
