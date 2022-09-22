using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level1 : MonoBehaviour
{
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
    }

    void Update()
    {
        switch (tutorialNum)
        {
            case 0:
                NextScript();
                break;
            case 1:
                //앵커[0] 생성 (스크롤버튼)
                NextScript();
                break;
            case 2:
                //앵커[0] 삭제
                NextScript();
                break;
            case 3:
                //앵커[0] 생성 (평가 오브젝트)
                //평가 오브젝트 위에 올라가면 nextScript()실행
                NextScript();
                break;
            case 4:
                //앵거[0] 삭제
                NextScript();
                break;
            case 5:
            case 6:
            case 7:
                NextScript();
                break;
            case 8:
                //앵커[0] 생성 (평가 오브젝트)
                //평가 오브젝트 위에 올라가면 nextScript()실행
                NextScript();
                break;
            case 9:
                //앵거[0] 삭제
                NextScript();
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
