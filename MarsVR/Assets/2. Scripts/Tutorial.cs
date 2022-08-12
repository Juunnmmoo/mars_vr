using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    CupCtrl cupctrl;
    PlayerCtrl playerctrl;
    List<string> scriptList = new List<string>();

    int TutorialNum = -1;

    [Header("UI °ü·Ã")]
    //scriptListUI
    public GameObject scriptUI;

    // Start is called before the first frame update
    void Start()
    {
        cupctrl = GameObject.Find("Cup").GetComponent<CupCtrl>();
        playerctrl = GameObject.Find("Player").GetComponent<PlayerCtrl>();

        scriptList = FileIO.ReadScript();
        foreach (string str in scriptList)
        {
            Debug.LogWarning(str);
        }

        scriptChange();
        InvokeRepeating("script1", 1.5f, 3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void script1() {
        if(TutorialNum<4)
            gameObject.GetComponentInChildren<Text>().text = scriptList[TutorialNum++];
    }

    //ScriptUI ¹Ù²ãÁÜ 
    private void scriptChange()
    {
           
    }

}
