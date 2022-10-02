using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameResult
{
    public int gameLevel;
    public float score1;
    public float score2;
    public float totalScore;
}
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<List<string>> receipt = new List<List<string>>();
    public List<List<float>> amount = new List<List<float>>();
    public float playTime;

    public int GetTotalScore(string sceneName)
    {
        if(sceneName.ToUpper().Equals("BARTENDER"))
            return (PlayerPrefs.GetInt("Level1Score") + PlayerPrefs.GetInt("Level2Score")) / 2;
        if (sceneName.ToUpper().Equals("BAKER"))
            return PlayerPrefs.GetInt("Level1Score");
        else
            return 0;
    }

    public string GetPlayTime()
    {
        int min = (int)(playTime / 60f);
        int sec = (int)(playTime - min * 60);

        return min.ToString() + " : " + sec.ToString();
    }


    public void InitialList()
    {
        receipt.Clear();
        amount.Clear();
        playTime = 0f;
    }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }  
        else
            Destroy(this.gameObject);



        //List<string> tempR = new List<string>();
        //List<float> tempA = new List<float>();

        //tempR.Add("1");
        //tempR.Add("2");
        //tempR.Add("3");

        //tempA.Add(1);
        //tempA.Add(2);
        //tempA.Add(3);

        //receipt.Add(tempR);
        //receipt.Add(tempR);

        //amount.Add(tempA);
        //amount.Add(tempA);

    }
}
