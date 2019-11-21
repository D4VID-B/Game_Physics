using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ColorManager : MonoBehaviour
{

    Scene currentScene;

    List<GameObject> rootObjects;

    public int delayCap = 25;
    private int delayIter = 0;

    // Start is called before the first frame update
    void Start()
    {

        currentScene = SceneManager.GetActiveScene();

        rootObjects = new List<GameObject>();
        currentScene.GetRootGameObjects(rootObjects);

        findObj();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (delayIter == delayCap)
        {
            updateColors();
            delayIter = 0;
        }
        else
        {
            delayIter++;
        }s
    }


    void updateColors()
    {
        
    }

    void findObj()
    {
        int iterID = 0;
        foreach (GameObject obj in rootObjects)
        {
            obj.GetComponent<ColorScript>().ID = iterID;

            iterID++;
        }
    }
}
