using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bakery : MonoBehaviour
{
    private GameObject bour;
    private GameObject flour;
    private GameObject egg;
    private GameObject water;
    private GameObject buter;
    private GameObject yeast;
    private GameObject cupHolder;
    private SceneCtrl sceneCtrl;
    private AnchorCtrl[] anchorList = new AnchorCtrl[2];
    public List<string> scriptList = new List<string>();

    [SerializeField]
    private int tutorialNum = 0;

    private GameObject nextBtn;

    public GameObject anchorPrefab;
    private Vector3 offset = Vector3.up * 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        sceneCtrl = GameObject.Find("SceneCtrl").GetComponent<SceneCtrl>();
        //bour = GameObject.Find("Bour");
        flour = GameObject.Find("Flour");
        egg = GameObject.Find("Egg");
        water = GameObject.Find("Water");
        buter = GameObject.Find("Buter");
        yeast = GameObject.Find("Yeast");
        //cupHolder = GameObject.Find("CupHolder");
        
        nextBtn = transform.Find("NextBtn").gameObject;

        scriptList = FileIO.ReadScript("Bakery");
    }

    // Update is called once per frame
    void Update()
    {
        switch (tutorialNum) {
            //보울 잡아보세요.
            /*
            case 3:
                nextBtn.SetActive(false);
                if (anchorList[0] == null)
                {
                    anchorList[0] = CreateAnchor(bour.transform.position + offset);
                }
                // 컵을 집을때
                if (bour.GetComponent<OVRGrabbable>().isGrabbed)
                {
                    nextBtn.SetActive(true);
                    anchorList[0].EndAnchor();
                    anchorList[0] = null;
                    tutorialNum++;
                }
                break;
            */

            //밀가루 잡아보세요
            /*
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
            */
            //밀가루 300넣기
            /*
            case 10:
                nextBtn.SetActive(false);
                if (CheckAmount(BottleType.FLOUR, 300))
                {
                    nextBtn.SetActive(true);
                    tutorialNum++;
                }
                break;
            */
            //물, 이스트 넣기
            /*
            case 12:
                nextBtn.SetActive(false);
                if (CheckAmount(BottleType.WATER, 150) && CheckAmount(BottleType.YEAST, 5)) {
                    nextBtn.SetActive(true);
                    tutorialNum++;
                }
                break;
            */

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

    /*
    private bool CheckAmount(BottleType bottleType, float amount)
    {
        float result = 0;
        for (int i = 0; i < bour.GetComponent<CupCtrl>().receipt.Count; i++)
        {
            if (bour.GetComponent<CupCtrl>().receipt[i].Equals(bottleType))
            {
                result += bour.GetComponent<CupCtrl>().amounts[i];

            }
        }
        return result > amount;
    }
    */

}
