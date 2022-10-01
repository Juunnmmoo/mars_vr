using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bakery : MonoBehaviour
{
    private PlayerCtrl player;
    private GameObject oven;
    private GameObject bowl;
    private GameObject flour;
    private GameObject egg;
    private GameObject water;
    private GameObject buter;
    private GameObject yeast;
    private SceneCtrl sceneCtrl;
    private AnchorCtrl[] anchorList = new AnchorCtrl[2];
    public List<string> scriptList = new List<string>();

    [SerializeField]
    private int tutorialNum = 0;

    private GameObject nextBtn;

    public GameObject anchorPrefab;
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerCtrl>();
        oven = GameObject.Find("Oven");
        sceneCtrl = GameObject.Find("SceneCtrl").GetComponent<SceneCtrl>();
        bowl = GameObject.Find("Bowl");
        flour = GameObject.Find("Flour");
        egg = GameObject.Find("Egg");
        water = GameObject.Find("Water");
        buter = GameObject.Find("Buter");
        yeast = GameObject.Find("Yeast");
        //cupHolder = GameObject.Find("CupHolder");
        
        nextBtn = transform.Find("NextBtn").gameObject;

        scriptList = FileIO.ReadScript("Bakery");
        offset = Vector3.up * 0.2f;
        if (PlayerPrefs.GetInt("OncePlayed") == 1)
        {

            tutorialNum = 23;
        }
        else
        {
            GameManager.instance.InitialList();
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (tutorialNum) {
            case 0:
                PlayerPrefs.SetInt("LevelReceipt", 1);
                break;
            
            case 3:
                nextBtn.SetActive(false);
                if (anchorList[0] == null)
                {
                    anchorList[0] = CreateAnchor(bowl.transform.position + offset);
                }
                // 컵을 집을때
                if (bowl.GetComponent<OVRGrabbable>().isGrabbed)
                {
                    nextBtn.SetActive(true);
                    anchorList[0].EndAnchor();
                    anchorList[0] = null;
                    tutorialNum++;
                }
                break;
            
            case 6:
                nextBtn.SetActive(false);
                if (anchorList[0] == null)
                {
                    anchorList[0] = CreateAnchor(flour.transform.position + offset);
                }
                if (flour.GetComponent<OVRGrabbable>().isGrabbed)
                {
                    nextBtn.SetActive(true);
                    anchorList[0].EndAnchor();
                    anchorList[0] = null;
                    tutorialNum++;
                }
                break;
           
            case 10:
                nextBtn.SetActive(false);
                if (CheckAmount(BottleType.FLOUR, 300))
                {
                    nextBtn.SetActive(true);
                    tutorialNum++;
                }
                break;
            
       
            //물, 이스트 넣기
            case 12:
                nextBtn.SetActive(false);
                if (anchorList[0] == null)
                    anchorList[0] = CreateAnchor(water.transform.position + offset);
                if (anchorList[1] == null)
                    anchorList[1] = CreateAnchor(yeast.transform.position + offset);
                if (CheckAmount(BottleType.WATER, 150) && CheckAmount(BottleType.YEAST, 5)) {
                    for (int i = 0; i < 2; i++)
                    {
                        anchorList[i].EndAnchor();
                        anchorList[i] = null;
                    }
                    nextBtn.SetActive(true);
                    tutorialNum++;
                }
                break;
            
            
            case 16:
                if (anchorList[0] == null)
                {
                    anchorList[0] = CreateAnchor(oven.transform.position + offset);
                }
                if (oven.GetComponent<EvaluateManager>().isEnd)
                {
                    nextBtn.SetActive(true);
                    anchorList[0].EndAnchor();
                    anchorList[0] = null;
                    tutorialNum++;
                }
                break;
            
            case 18:
                //오븐 열기 되면 스크립트 넘어가기
                break;

            case 22:
                PlayerPrefs.SetInt("OncePlayed", 1);
                sceneCtrl.ToBaker();
                break;
            
            case 27:
                PlayerPrefs.SetInt("LevelReceipt", 1);
                if (anchorList[0] == null)
                {
                    anchorList[0] = CreateAnchor(oven.transform.position + offset);
                }
                if (oven.GetComponent<EvaluateManager>().isEnd)
                {
                    nextBtn.SetActive(true);
                    anchorList[0].EndAnchor();
                    anchorList[0] = null;
                    tutorialNum++;
                }
                break;
            
            case 30:
                PlayerPrefs.SetFloat("Level1Score", Mathf.Round(player.score));
                PlayerPrefs.SetInt("OncePlayed", 0);
                sceneCtrl.ToScore();
                break;
        }
        if (tutorialNum < scriptList.Count)
            gameObject.GetComponentInChildren<Text>().text = scriptList[tutorialNum];
    }
    public void NextScript()
    {
        tutorialNum++;
    }

    private AnchorCtrl CreateAnchor(Vector3 pos)
    {
        AnchorCtrl temp = Instantiate(anchorPrefab, pos, Quaternion.identity).GetComponent<AnchorCtrl>();
        temp.originPos = pos;
        return temp;
    }

  
    private bool CheckAmount(BottleType bottleType, float amount)
    {
        float result = 0;
        for (int i = 0; i < bowl.GetComponent<CupCtrl>().receipt.Count; i++)
        {
            if (bowl.GetComponent<CupCtrl>().receipt[i].Equals(bottleType))
            {
                result += bowl.GetComponent<CupCtrl>().amounts[i];

            }
        }
        return result > amount;
    }
  

}
