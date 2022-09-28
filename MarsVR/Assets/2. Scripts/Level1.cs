using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level1 : MonoBehaviour
{
    private PlayerCtrl player;
    private SceneCtrl sceneCtrl;
    private GameObject cup;
    private GameObject cupHolder;
    private AnchorCtrl[] anchorList = new AnchorCtrl[2];
    private List<string> scriptList = new List<string>();

    [SerializeField]
    private int tutorialNum = 0;

    public GameObject anchorPrefab;
    private GameObject nextBtn;

    private Vector3 offset = Vector3.up * 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        sceneCtrl = GameObject.Find("SceneCtrl").GetComponent<SceneCtrl>(); 
        player = GameObject.FindWithTag("Player").GetComponent<PlayerCtrl>(); 
        cup = GameObject.Find("Cup");
        cupHolder = GameObject.Find("CupHolder");
        scriptList = FileIO.ReadScript("Level1");
        nextBtn = transform.Find("NextBtn").gameObject;       

        if (PlayerPrefs.GetInt("oncePlayed")==1) {
            tutorialNum = 7;
            PlayerPrefs.SetInt("oncePlayed", 0);
        }
    }

    void Update()
    {
        switch (tutorialNum)
        {
            case 1:
                // ��ũ�� �� ��Ŀ ����
                //if (anchorList[0] == null)
                //{
                //    anchorList[0] = CreateAnchor(��ũ��.transform.position + offset);
                //}
                break;
            case 3:
                PlayerPrefs.SetInt("LevelReceipt", 1);
                nextBtn.SetActive(false);     
                if (cupHolder.GetComponent<EvaluateManager>().isEnd) {
                    nextBtn.SetActive(true);                   
                    tutorialNum++;
                }
                break;
            case 5:
                PlayerPrefs.SetFloat("Level1Score", Mathf.Round(player.score));
                PlayerPrefs.SetInt("oncePlayed", 1);
                break;
            case 6:
                sceneCtrl.ToLevel2();
                break;
            //1�ܰ� ���� �� 2�ܰ� ����
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
                PlayerPrefs.SetFloat("Level2Score", Mathf.Round(player.score));
                break;
            case 11:
                
                gameObject.GetComponentInChildren<Text>().text = "����� ������ " + ((PlayerPrefs.GetFloat("Level1Score") + PlayerPrefs.GetFloat("Level2Score"))/2).ToString() + " �Դϴ�";
                break;
            case 12:
                PlayerPrefs.SetInt("LevelReceipt", 0);
                gameObject.GetComponentInChildren<Text>().text = "������ ����Ǿ����ϴ�,";               
                break;
            case 13:
                gameObject.GetComponentInChildren<Text>().text = "Detail score check please";  // 맥북이라 한글깨질까봐 영어로 적었습니다,,
                sceneCtrl.ToScore();
                break;
        }
        if (tutorialNum < scriptList.Count)
            gameObject.GetComponentInChildren<Text>().text = scriptList[tutorialNum];
    }

    private AnchorCtrl CreateAnchor(Vector3 pos)
    {
        AnchorCtrl temp = Instantiate(anchorPrefab, pos, Quaternion.identity).GetComponent<AnchorCtrl>();
        temp.originPos = pos;
        return temp;
    }

    public void NextScript() {
        tutorialNum++;
    }
    
   
}
