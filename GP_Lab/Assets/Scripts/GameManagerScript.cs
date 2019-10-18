using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    // Start is called before the first frame update

    public bool getToEnd;
    public bool hitByAstroid;

    private Coroutine killPlayer;
    //public int health;
    //public Text healthBar;

    public Text CountDown;
    
    void Start()
    {
        getToEnd = false;
        hitByAstroid = false;
        CountDown.text = " ";
    }

    // Update is called once per frame
    void Update()
    {
        if(GameObject.Find("TempShip").transform.position.x >= 89)
        {
            getToEnd = true;
        }

        gameState();
    }

    void gameState()
    {
        if(getToEnd)
        {
            SceneManager.LoadScene("EndScene");   
        }

        if(hitByAstroid)
        {
            //killPlayer = hitAst(.1f);
            StartCoroutine(hitAst());
        }
    }


    IEnumerator hitAst()
    {
        CountDown.text = "TERMINAL DAMAGE";
        yield return new WaitForSeconds(1.0f);
        CountDown.text = "3";
        yield return new WaitForSeconds(0.5f);
        CountDown.text = "2";
        yield return new WaitForSeconds(0.5f);
        CountDown.text = "1";
        yield return new WaitForSeconds(0.5f);
        CountDown.text = " ";
        SceneManager.LoadScene("Midterm");

    }
}
