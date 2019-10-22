using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockExplosion : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject RockOne;
    private GameObject RockTwo;
    private GameObject RockThree;
    private GameObject RockFour;

    public GameObject ship;

    private Vector3 startPos;

    public float spawnBuffer;

    private Vector2 forceOfImpact_Up;
    private Vector2 forceOfImpact_Down;
    private Vector2 forceOfImpact_Left;
    private Vector2 forceOfImpact_Right;



    void Start()
    {
        //New Rock Objects
        RockOne = this.transform.GetChild(0).gameObject;
        RockTwo = this.transform.GetChild(1).gameObject;
        RockThree = this.transform.GetChild(2).gameObject;
        RockFour = this.transform.GetChild(3).gameObject;

        forceOfImpact_Up = new Vector2(0.0f, 100.0f);
        forceOfImpact_Down = new Vector2(0.0f, -100.0f);
        forceOfImpact_Left = new Vector2(-100.0f, 0.0f);
        forceOfImpact_Right = new Vector2(100.0f, 0.0f);

        spawnBuffer = 2.0f;
        startPos = this.transform.position;                 //THIS ONLY WORKS IF THE OBJECT IS STILL OTHERWISE THE ASTROID RESPAWN WILL BE FROM ITS ORIGINAL MOVE POINT
    }

    // Update is called once per frame
    void Update()
    {
        SpaceShipHit();
    }

    void SpaceShipHit()
    {
        if(this.GetComponent<CircleHull2D>().hitExplode == true)
        {
            ship.GetComponent<Particle2D>().SHIP_MODE = false;
            Exploded();
            this.GetComponent<CircleHull2D>().hitExplode = false;
        }
    }

    void Exploded()
    {

        //instantiate
        //Instantiate(RockOne);
        //Instantiate(RockTwo);
        //Instantiate(RockThree);
        //Instantiate(RockFour);

        //set the debris positions
        RockOne.transform.position = startPos + new Vector3(RockOne.GetComponent<CircleHull2D>().radius + spawnBuffer, 0,0);        //right
        RockTwo.transform.position = startPos + new Vector3(0, RockTwo.GetComponent<CircleHull2D>().radius + spawnBuffer, 0);       //up
        RockThree.transform.position = startPos - new Vector3(RockThree.GetComponent<CircleHull2D>().radius + spawnBuffer, 0, 0);   //left
        RockFour.transform.position = startPos - new Vector3(0, RockFour.GetComponent<CircleHull2D>().radius + spawnBuffer, 0);     //down

        RockOne.GetComponent<CircleHull2D>().enabled = false;
        RockTwo.GetComponent<CircleHull2D>().enabled = false;
        RockThree.GetComponent<CircleHull2D>().enabled = false;
        RockFour.GetComponent<CircleHull2D>().enabled = false;

        //shoot out in all directions
        RockOne.GetComponent<Particle2D>().addForce(forceOfImpact_Right);
        RockTwo.GetComponent<Particle2D>().addForce(forceOfImpact_Up);
        RockThree.GetComponent<Particle2D>().addForce(forceOfImpact_Left);
        RockFour.GetComponent<Particle2D>().addForce(forceOfImpact_Down);

        

        //this.gameObject.GetComponent<CircleCollider2D>().enabled = false;
        //this.gameObject.SetActive(false);
        //this.transform.position = new Vector3(0, 0, 0);

        //delete children
        //this.transform.GetChild(0).gameObject.SetActive(false);
        //this.transform.GetChild(1).gameObject.SetActive(false);
        //this.transform.GetChild(2).gameObject.SetActive(false);
        //this.transform.GetChild(3).gameObject.SetActive(false);

    }
}
