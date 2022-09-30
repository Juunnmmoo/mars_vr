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
