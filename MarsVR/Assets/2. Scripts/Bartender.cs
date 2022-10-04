using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Bartender : MonoBehaviour
{
    private PlayerCtrl player;
    private SceneCtrl sceneCtrl;
    private GameObject cupHolder;
    private List<string> scriptList = new List<string>();

    [SerializeField]
    private int tutorialNum = 0;

    public GameObject anchorPrefab;
    private GameObject nextBtn;

    // Start is called before the first frame update
    void Start()
    {
        sceneCtrl = GameObject.Find("SceneCtrl").GetComponent<SceneCtrl>(); 
        player = GameObject.FindWithTag("Player").GetComponent<PlayerCtrl>(); 
        cupHolder = GameObject.Find("CupHolder");
        scriptList = FileIO.ReadScript("Level1");
        nextBtn = transform.Find("NextBtn").gameObject;

        if (PlayerPrefs.GetInt("OncePlayed") == 1) {
            
            tutorialNum = 7;
        }
        else
        {
            GameManager.instance.InitialList();
        }
    }

    void Update()
    {
        switch (tutorialNum)
        {
            case 3:
                PlayerPrefs.SetInt("LevelReceipt", 1);
                nextBtn.SetActive(false);     
                if (cupHolder.GetComponent<EvaluateManager>().isEnd) {
                    nextBtn.SetActive(true);                   
                    tutorialNum++;
                }
                break;
            case 5:
                PlayerPrefs.SetInt("Level1Score", (int)Mathf.Round(player.score));
                Debug.LogError(Mathf.Round(player.score));
                PlayerPrefs.SetInt("OncePlayed", 1);
                break;
            case 6:
                sceneCtrl.ToBar();
                break;
            case 9:
                PlayerPrefs.SetInt("LevelReceipt", 2);
                nextBtn.SetActive(false);               
                if (cupHolder.GetComponent<EvaluateManager>().isEnd)
                {
                    nextBtn.SetActive(true);                    
                    tutorialNum++;
                }
                break;
            case 10:
                PlayerPrefs.SetInt("Level2Score", (int)Mathf.Round(player.score));
                PlayerPrefs.SetString("CurScene", SceneManager.GetActiveScene().name);
                PlayerPrefs.SetInt("OncePlayed", 0);
                break;
            case 12:
                sceneCtrl .ToScore();
                break;
        }
        if (tutorialNum < scriptList.Count)
            gameObject.GetComponentInChildren<Text>().text = scriptList[tutorialNum];
        }


    [ContextMenu("Next Script")]
    public void NextScript() {
        tutorialNum++;
    }
    
   
}
