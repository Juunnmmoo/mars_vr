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

    private int tutorialNum = 0;
    private bool check;

    [Range(1.5f, 3.0f)]
    public float nextTime = 2.0f;

    public GameObject anchorPrefab;

    private Vector3 offset = Vector3.up * 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        cup = GameObject.Find("Cup");
        cupHolder = GameObject.Find("CupHolder");

        scriptList = FileIO.ReadScript("Level1");
        foreach (string script in scriptList)
        {
            Debug.LogError(script);
        }

        if (PlayerPrefs.GetInt("oncePlayed")==1) {
            tutorialNum = 6;
        }
    }

    void Update()
    {
        switch (tutorialNum)
        {
            case 0:
                //NextScript();
                break;
            case 1:
                // 스크롤 에 앵커 띄우기
                //if (anchorList[0] == null)
                //{
                //    anchorList[0] = CreateAnchor(cup.transform.position + offset);
                //}
                //NextScript();
                break;
            case 2:
                //앵커[0] 삭제
                //anchorList[0].EndAnchor();
                //anchorList[0] = null;
                //NextScript();
                break;
            case 3:
                //앵커[0] 생성 (평가 오브젝트)
                if (anchorList[0] == null)
                {
                    anchorList[0] = CreateAnchor(cupHolder.transform.position + offset);
                }
                //평가 오브젝트 위에 올라가면 nextScript()실행
                if (cupHolder.GetComponent<EvaluateManager>().isEnd) {
                    NextScript();
                    //앵거[0] 삭제
                    anchorList[0].EndAnchor();
                    anchorList[0] = null;
                }

                break;
            case 4:
                //NextScript();
                break;
            case 5:
                PlayerPrefs.SetFloat("Level1Score", Mathf.Round(player.score));
                PlayerPrefs.SetInt("oncePlayed", 1);
                sceneCtrl.ToLevel2();
                break;
            //2단계 시작
            case 6:
            case 7:
                //NextScript();
                break;
            case 8:
                //앵커[0] 생성 (평가 오브젝트)
                if (anchorList[0] == null)
                {
                    anchorList[0] = CreateAnchor(cupHolder.transform.position + offset);
                }
                //평가 오브젝트 위에 올라가면 nextScript()실행
                if (cupHolder.GetComponent<EvaluateManager>().isEnd)
                {
                    NextScript();
                    //앵거[0] 삭제
                    anchorList[0].EndAnchor();
                    anchorList[0] = null;
                }
                break;
            case 9:
                PlayerPrefs.SetFloat("Level2Score", Mathf.Round(player.score));
                break;
            case 10:
                gameObject.GetComponentInChildren<Text>().text = "당신의 점수는 " + ((PlayerPrefs.GetFloat("Level1Score") + PlayerPrefs.GetFloat("Level2Score"))/2).ToString() + " 입니다";
                break;
            case 11:
                gameObject.GetComponentInChildren<Text>().text = "게임이 종료되었습니다,";
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

    public void NextScript2() {
        tutorialNum++;
    }
    private void NextScript()
    {
        if (check == true)
            return;
        check = true;
        StartCoroutine(NextScriptCor(nextTime));
    }
    private IEnumerator NextScriptCor(float time)
    {
        float elaspedTime = 0;      // 경과 시간
        while (elaspedTime < time)
        {
            elaspedTime += Time.deltaTime;
            yield return null;
        }
        check = false;
        tutorialNum++;
    }
}
