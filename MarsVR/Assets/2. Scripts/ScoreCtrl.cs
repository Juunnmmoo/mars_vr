using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCtrl : MonoBehaviour
{

    public PlayerCtrl player;
    public Text myScore;
    public ScrollRect[] resultScroll = new ScrollRect[2];
    public GameObject[] contents = new GameObject[2];
    public GameObject resultItemPrefab;
    private List<List<string>> receiptList;
    private List<List<float>> amountList;

    void Start()
    {
        receiptList = GameManager.instance.receipt;
        amountList = GameManager.instance.amount;


        for (int i = 0; i < receiptList.Count; i++)
        {
            resultScroll[i].content.sizeDelta = new Vector2(0, receiptList[i].Count * 50);
            for (int j = 0; j < receiptList[i].Count; j++)
            {
                GameObject temp = Instantiate(resultItemPrefab, Vector2.zero, Quaternion.identity, contents[i].transform);
                temp.transform.Find("Receipt").GetComponent<Text>().text = receiptList[i][j];
                temp.transform.Find("Amount").GetComponent<Text>().text = amountList[i][j].ToString();
                temp.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, (-50 * j));
            }
        }

        myScore.text = GameManager.instance.GetTotalScore().totalScore.ToString();

    }
}