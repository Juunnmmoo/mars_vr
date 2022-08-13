using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    private CupCtrl cupctrl;
    private PlayerCtrl playerctrl;
    private BottleCtrl bottlectrl1;
    private BottleCtrl bottlectrl2;
    private BottleCtrl bottlectrl3;
    private List<string> scriptList = new List<string>();

    private int tutorialNum = 0;

    private bool check;

    [Range(1.5f, 3.0f)]
    public float nextTime = 2.0f;

    [Header("UI ����")]
    //scriptListUI
    public GameObject scriptUI;

    // Start is called before the first frame update
    void Start()
    {
        cupctrl = GameObject.Find("Cup").GetComponent<CupCtrl>();
        playerctrl = GameObject.Find("Player").GetComponent<PlayerCtrl>();
        bottlectrl1 = GameObject.Find("Bottle").GetComponent<BottleCtrl>();
        bottlectrl2 = GameObject.Find("Bottle (1)").GetComponent<BottleCtrl>();
        bottlectrl3 = GameObject.Find("Bottle (2)").GetComponent<BottleCtrl>();

        scriptList = FileIO.ReadScript();
        for (int i = 0; i < scriptList.Count; i++)
        {
            Debug.LogWarning(i + "=" + scriptList[i]);

        }


    }

    // Update is called once per frame
    void Update()
    {
        script1();
    }

    // Ʃ�丮�� ��ũ��Ʈ ���� �Լ�
    void script1()
    {
        switch (tutorialNum)
        {
            case 0:
            case 1:
            case 2:
                NextScript();
                break;
            case 3:
                // ���� ������
                if (cupctrl.isUsing)
                    NextScript();
                break;
            case 4:
            case 5:
                NextScript();
                break;
            case 6:
                // ���� ������
                if (bottlectrl1.isUsing)
                    NextScript();
                break;
            case 7:
            case 8:
                NextScript();
                break;
            case 9:
                // ������ 20ml �־�����
                if(((int)cupctrl.amounts[cupctrl.amounts.Count - 1]) >= 20)
                {
                    NextScript();
                }
                break;
            case 10:
            case 11:
            case 12:
                NextScript();
                break;
            case 13:
                // ��ų�� 20ml, ȭ��Ʈ�� 20ml �־�����
            case 14:
                
            default:
                break;
        }
        gameObject.GetComponentInChildren<Text>().text = scriptList[tutorialNum];

    }

    //ScriptUI �ٲ��� 
    private void scriptChange()
    {

    }

    private IEnumerator NextScriptCor(float time)
    {
        float elaspedTime = 0;      // ��� �ð�
        while (elaspedTime < time)
        {
            elaspedTime += Time.deltaTime;
            yield return null;
        }
        check = false;
        tutorialNum++;
    }

    void NextScript()
    {
        if (check == true)
            return;
        check = true;
        StartCoroutine(NextScriptCor(nextTime));

    }


}
