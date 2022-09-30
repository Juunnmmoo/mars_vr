using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCtrl : MonoBehaviour
{
    
    public PlayerCtrl player;
    public Text myScore;
    public int score;

    void Start()
    {
        
        player = GameObject.FindWithTag("Player").GetComponent<PlayerCtrl>();

        // score = (int)player.score;
        // score = 80;
        score = (int)player.score;
        myScore.text = score.ToString();

        Debug.Log(myScore.text);

    }

    void Update(){
      

    }
    

}
/*
 
        List<string> receipt1 = GameManager.instance.receipt[0];
        List<float> amount1 = GameManager.instance.amount[0];
        List<string> receipt2 = GameManager.instance.receipt[1];
        List<float> amount2 = GameManager.instance.amount[1];

        for (int i = 0; i < receipt1.Count-1; i++)
        {
            endUIText.text += receipt1[i].ToString() + " : " + Mathf.Round(amount1[i]).ToString() + "\n";
        }

        for (int i = 0; i < receipt2.Count - 1; i++)
        {
            endUIText.text += receipt2[i].ToString() + " : " + Mathf.Round(amount2[i]).ToString() + "\n";
        }

        ÃÑ ½ºÄÚ¾î
        GameManager.instance.GetTotalScore().ToString();

 */