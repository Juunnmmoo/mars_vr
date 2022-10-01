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
        if(scene.name=="bar")
            gameResult.totalScore = (gameResult.score1 + gameResult.score2) / 2;
        if (scene.name == "bakery")
            gameResult.totalScore = gameResult.score1;
        return gameResult;
    }

    public List<List<string>> receipt = new List<List<string>>();
    public List<List<float>> amount = new List<List<float>>();

    public void InitialList()
    {
        receipt.Clear();
        amount.Clear();
    }


    public static GameManager instance;
    private List<Attr> xmlList = new List<Attr>();
    List<string> scriptList = new List<string>();

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
