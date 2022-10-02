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
    private Scene scene;

    public GameResult GetGameLevel()
    {
        GameResult gameResult = new GameResult();
        gameResult.gameLevel = PlayerPrefs.GetInt("oncePlayed") == 0 ? 1 : 2;
        return gameResult;
    }
    public GameResult GetScore1()
    {
        GameResult gameResult = new GameResult();
        gameResult.score1 = PlayerPrefs.GetInt("Level1Score");
        return gameResult;
    }
    public GameResult GetScore2()
    {
        GameResult gameResult = new GameResult();
        gameResult.score1 = PlayerPrefs.GetInt("Level2Score");
        return gameResult;
    }
    public GameResult GetTotalScore()
    {
        GameResult gameResult = new GameResult();
        if(scene.name=="Bar")
            gameResult.totalScore = (gameResult.score1 + gameResult.score2) / 2;
        if (scene.name == "Bakery")
            gameResult.totalScore = gameResult.score1;
        return gameResult;
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
        scene = SceneManager.GetActiveScene();
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }  
        else
            Destroy(this.gameObject);
    }
}
