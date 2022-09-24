using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    private GameObject cup;
    private GameObject tequila;
    private GameObject whiteRum;
    private GameObject lemon;
    private GameObject cupHolder;
    private AnchorCtrl[] anchorList = new AnchorCtrl[2];
    public List<string> scriptList = new List<string>();

    [SerializeField]
    private int tutorialNum = 0;

    private bool check;



    [Range(1.5f, 5.0f)]
    public float nextTime = 2.0f;

    [Header("UI 관련")]
    //scriptListUI
    public GameObject scriptUI;
    private GameObject nextBtn;

    //SpotLight
    public GameObject anchorPrefab;
    
    private Vector3 offset = Vector3.up * 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        cup = GameObject.Find("Cup");
        tequila = GameObject.Find("Tequila");
        whiteRum = GameObject.Find("WhiteRum");
        cupHolder = GameObject.Find("CupHolder");
        lemon = GameObject.Find("Lemon");
        nextBtn = transform.Find("NextBtn").gameObject;

        scriptList = FileIO.ReadScript("Tutorial");
    }

    // Update is called once per frame
    void Update()
    {

        switch (tutorialNum)
        {
            case 3:
                nextBtn.SetActive(false);
                if (anchorList[0] == null)
                {
                    anchorList[0] = CreateAnchor(cup.transform.position + offset);
                }
                // 컵을 집을때
                if (cup.GetComponent<OVRGrabbable>().isGrabbed)
                {
                    nextBtn.SetActive(true);
                    anchorList[0].EndAnchor();
                    anchorList[0] = null;
                    tutorialNum++;
                }
                break;
            case 6:
                // 병을 집을때
                nextBtn.SetActive(false);
                if (anchorList[0] == null)
                {
                    anchorList[0] = CreateAnchor(lemon.transform.position + offset);
                }
                if (lemon.GetComponent<OVRGrabbable>().isGrabbed)
                {
                    nextBtn.SetActive(true);
                    anchorList[0].EndAnchor();
                    anchorList[0] = null;
                    tutorialNum++;
                }
                break;
            case 9:
                nextBtn.SetActive(false);
                if (CheckAmount(BottleType.LEMON, 20))
                {
                    nextBtn.SetActive(true);
                    tutorialNum++;
                }
                break;
            case 13:
                {
                    nextBtn.SetActive(false);
                    if (anchorList[0] == null)
                        anchorList[0] = CreateAnchor(tequila.transform.position + offset);

                    if (anchorList[1] == null)
                        anchorList[1] = CreateAnchor(whiteRum.transform.position + offset);
                    // 데킬라 20ml, 화이트럼 20ml 넣었을때
                    if (CheckAmount(BottleType.TEQUILA, 20) && CheckAmount(BottleType.WHITE_RUM, 20)) 
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            anchorList[i].EndAnchor();
                        anchorList[i] = null;
                        }
                        tutorialNum++;
                    }
                }
                break;
            case 14:
                if (anchorList[0] == null)
                {
                    anchorList[0] = CreateAnchor(cupHolder.transform.position + offset);
                }
                if (cupHolder.GetComponent<EvaluateManager>().isEnd)
                {
                    nextBtn.SetActive(true);
                    anchorList[0].EndAnchor();
                    anchorList[0] = null;
                    tutorialNum++;
                }
                break;
            default:
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

    //private void NextScript()
    //{
    //    if (check == true)
    //        return;
    //    check = true;
    //    StartCoroutine(NextScriptCor(nextTime));
    //}

    private bool CheckAmount(BottleType bottleType, float amount)
    {
        for (int i = 0; i < cup.GetComponent<CupCtrl>().receipt.Count; i++)
        {
            if (cup.GetComponent<CupCtrl>().receipt[i].Equals(bottleType))
            {
                return cup.GetComponent<CupCtrl>().amounts[i] > amount;
            }
        }
        return false;
    }

    //private IEnumerator NextScriptCor(float time)
    //{
    //    float elaspedTime = 0;      // 경과 시간
    //    while (elaspedTime < time)
    //    {
    //        elaspedTime += Time.deltaTime;
    //        yield return null;
    //    }
    //    check = false;
    //    tutorialNum++;
    //}
}
