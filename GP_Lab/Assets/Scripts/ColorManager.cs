using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ColorManager : MonoBehaviour
{

    Scene currentScene;

    public int delayCap = 25;
    private int delayIter = 0;

    // Start is called before the first frame update
    void Start()
    {
        //MyUnityPlugin.InitPool();


        currentScene = SceneManager.GetActiveScene();

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
        }
    }


    void updateColors()
    {
        //MyUnityPlugin.updateObjectsInPool(5);
        //foreach(GameObject obj in currentScene.GetRootGameObjects())
        //{
        //    int temp = obj.GetComponent<ColorScript>().ID;
        //    obj.GetComponent<ColorScript>().color = MyUnityPlugin.getObjColor(temp);

        //}
    }

    void findObj()
    {
        int iterID = 0;


        foreach (GameObject obj in currentScene.GetRootGameObjects())
        {
            if(obj.GetComponent<ColorScript>() != null)
            {
                obj.GetComponent<ColorScript>().ID = iterID;
            }

            //MyUnityPlugin.InitAndPushObj(obj.GetComponent<ColorScript>().ID);

            ++iterID;
        }
    }
}
