using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class ReceiptCtrl : MonoBehaviour
{
    private GameObject receiptUI;
    // Start is called before the first frame update
    void Start()
    {
        receiptUI = transform.Find("ReceiptUI").gameObject;

        receiptUI.SetActive(false);

        
    }

    public void ShowReceipt(string receiptName)
    {
        List<Attr> list = FileIO.ReadReceipt(receiptName);
        receiptUI.SetActive(true);

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < list.Count; i++)
        {
            sb.Append(list[i].receipt).Append(", ").Append(list[i].amount).Append("ml\n");
        }


        receiptUI.GetComponentInChildren<Text>().text = sb.ToString();
    }

    public void CloseReceipt()
    {
        receiptUI.SetActive(false);
    }
}