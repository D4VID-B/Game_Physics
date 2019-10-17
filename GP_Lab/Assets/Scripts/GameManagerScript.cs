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
        
    }

    void gameState()
    {
        if(getToEnd)
        {
            SceneManager.LoadScene("EndScene");   
        }

        if(hitByAstroid)
        {
            StartCoroutine(hitAst());
            SceneManager.LoadScene("Midterm");
        }
    }


    IEnumerator hitAst()
    {
        CountDown.text = "TERMINAL DAMAGE";
        yield return new WaitForSeconds(2.0f);
        CountDown.text = "3";
        yield return new WaitForSeconds(0.5f);
        CountDown.text = "2";
        yield return new WaitForSeconds(0.5f);
        CountDown.text = "1";
        yield return new WaitForSeconds(0.5f);
        CountDown.text = " ";

    }
}
