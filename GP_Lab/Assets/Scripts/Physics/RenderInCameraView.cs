using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RenderInCameraView : MonoBehaviour
{
    // Start is called before the first frame update

    List<GameObject> rootObjects;
    Scene currentScene;
    Camera cm;

    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        cm = this.gameObject.GetComponent<Camera>();
        rootObjects = new List<GameObject>();
        currentScene.GetRootGameObjects(rootObjects);
    }

    // Update is called once per frame
    void Update()
    {
        findHullsRendered();
    }

    void findHullsRendered()
    {
        CollisionHull2D hull;
        foreach (GameObject obj in rootObjects)
        {
            hull = obj.GetComponent<CollisionHull2D>();
            Vector3 pos = cm.WorldToViewportPoint(obj.transform.position);

            if (hull != null)
            {
                if(pos.x < 0.0 || pos.x > 1.0)
                {
                    Debug.Log("OUTSIDE: " + obj.name);
                    obj.SetActive(false);
                    //obj.GetComponent<CollisionHull2D>().enabled = false;

                }
                else
                {
                    Debug.Log("INSIDE: " + obj.name);
                    obj.SetActive(true);
                    //obj.GetComponent<CollisionHull2D>().enabled = true;
                }
            }
        }
    }
}
