using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    // Start is called before the first frame update

    public bool getToEnd;
    public bool hitByAstroid;

    public int health;
    public Text healthBar;
    
    void Start()
    {
        getToEnd = false;
        hitByAstroid = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void gameState()
    {
        if(getToEnd)
        {

        }

        if(hitByAstroid)
        {

        }
    }
}
