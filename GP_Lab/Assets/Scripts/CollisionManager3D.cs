using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionManager3D : MonoBehaviour
{

    public static CollisionManager3D instance;

    CollisionHull3D.Collision desc;

    Scene currentScene;
    public List<CollisionHull3D> colliders;
    public List<GameObject> rootObjects;

    public bool enableManager = true;
    public bool shouldChangeColor = true;

    Camera cm;

    [Range(0.0f, 1.0f)]
    public float coefficientOfRestitution = 0.8f;

    // Start is called before the first frame update
    void Start()
    {
        desc = new CollisionHull3D.Collision();
        currentScene = SceneManager.GetActiveScene();

        colliders = new List<CollisionHull3D>();
        rootObjects = new List<GameObject>();

        currentScene.GetRootGameObjects(rootObjects);

        //Manually adding Grapple to the list
        rootObjects.Add(GameObject.Find("3D_Ship/Grapple"));
        rootObjects.Add(GameObject.Find("Finish/f1"));
        rootObjects.Add(GameObject.Find("Finish/f2"));
        rootObjects.Add(GameObject.Find("Finish/f3"));

        cm = Camera.main;

        findHulls();
    }

    // Update is called once per frame
    void Update()
    {   
        if(enableManager)
        {
            checkForCollisions();
            //dynamicAdd();
        }
        
    }




    //Look at every object in the camera view and add it to the list if it is not already on it
    void dynamicAdd()
    {
        bool onList = false;
        foreach (GameObject obj in rootObjects)
        {
            if(isInView(obj))
            {
                CollisionHull3D objHull = obj.GetComponent<CollisionHull3D>();

                foreach (CollisionHull3D hull in colliders)
                {
                    if (objHull != null && objHull.gameObject == hull.gameObject)
                    {
                        //they are equal
                        onList = true;
                    }
                }
                if(onList == false)
                {
                    colliders.Add(objHull);
                }
            }
            
        }
    }

    void findHulls()
    {
        CollisionHull3D hull;
        foreach (GameObject obj in rootObjects)
        {
            hull = obj.GetComponent<CollisionHull3D>();

            if (hull != null /*&& isInView(obj)*/)
            {
                colliders.Add(hull);
            }
        }
    }

    bool isInView(GameObject obj)
    {
        Vector3 pos = cm.WorldToViewportPoint(obj.transform.position);
        if (pos.x < 0.0 || pos.x > 1.0)
        {
            //Outside the camera
            return false;
        }
        else
        {
            //Inside camera view
            return true;
        }
    }

    void checkForCollisions()
    {
        for(int i = 0; i < colliders.Count; i++)
        {
            for(int j = i+1; j < colliders.Count; j++)
            {
                if(colliders[i].gameObject.activeSelf == false || colliders[j].gameObject.activeSelf == false)
                {
                    Debug.Log("One of the objects is inactive!");
                    //Don't update
                }
                else
                {
                    if (CollisionHull3D.TestCollision(colliders[i], colliders[j], ref desc))
                    {
                        //if (shouldChangeColor)
                        //{
                        //    CollisionHull3D.changeColor(colliders[i].gameObject, true);
                        //    CollisionHull3D.changeColor(colliders[j].gameObject, true);
                        //}

                        CollisionHull3D.resolveInterpenetration(ref desc);
                        CollisionHull3D.updateCollision(ref desc, coefficientOfRestitution);

                        if(colliders[i].GetComponent<FuelPickup>() != null && colliders[j].gameObject.tag == "Respawn") //Hit ground
                        {
                            //colliders[i].GetComponent<Particle3D>().velocity = ForceGenerator3D.genereateImpulse(colliders[i].GetComponent<Particle3D>().velocity, 5f);
                        }
                        else if(colliders[i].GetComponent<FuelPickup>() != null && colliders[j].gameObject.tag == "Player")//Picked up fuel
                        {
                            GameObject.Find("3D_Ship").GetComponent<ShipFuel>().addFuel(colliders[i].GetComponent<FuelPickup>().FUEL_VALUE);

                            colliders[i].gameObject.SetActive(false);
                            colliders.Remove(colliders[i]);
                        }
                        else if(colliders[i].gameObject.tag == "Respawn" || colliders[j].gameObject.tag == "Respawn") //Hit the ground -> death scene
                        {
                            if (colliders[i].GetComponent<ShipFuel>() != null)
                            {
                                Vector3 norm = new Vector3(0f, 10f, 0f);
                                float mult = ForceGenerator3D.calcImpulseMagnitude(0.5f, colliders[i].GetComponent<Particle3D>().getMass(), colliders[j].GetComponent<Particle3D>().getMass(),
                                    colliders[i].GetComponent<Particle3D>().rotation, colliders[j].GetComponent<Particle3D>().rotation,
                                    colliders[i].GetComponent<Particle3D>().angularVelocity, colliders[j].GetComponent<Particle3D>().angularVelocity,
                                    colliders[i].GetComponent<Particle3D>().localCoM, colliders[j].GetComponent<Particle3D>().localCoM,
                                    colliders[i].GetComponent<Particle3D>().velocity, colliders[j].GetComponent<Particle3D>().velocity,
                                    colliders[i].GetComponent<Particle3D>().worldTensor, colliders[j].GetComponent<Particle3D>().worldTensor,
                                    colliders[i].GetComponent<Particle3D>().localTensor, colliders[j].GetComponent<Particle3D>().localTensor,
                                    norm, colliders[i].transform.position, new Vector3(0f, 0f, 0f));
                                
                                colliders[i].GetComponent<Particle3D>().addForce(ForceGenerator3D.genereateImpulse(norm, mult));
                            }

                            //GameObject.Find("GameManager").GetComponent<SwitchScene>().Switch();
                        }
                        else if(colliders[i].gameObject.tag == "Finish" || colliders[j].gameObject.tag == "Finish")//Reached the end of the level
                        {
                            GameObject.Find("f1").GetComponent<SwitchScene>().Switch();
                            GameObject.Find("f2").GetComponent<SwitchScene>().Switch();
                            GameObject.Find("f3").GetComponent<SwitchScene>().Switch();
                        }

                    }
                    else
                    {
                        //if (shouldChangeColor)
                        //{
                        //    CollisionHull3D.changeColor(colliders[i].gameObject, false);
                        //    CollisionHull3D.changeColor(colliders[j].gameObject, false);
                        //}

                    }
                }

                
            }
        }
    }

}
