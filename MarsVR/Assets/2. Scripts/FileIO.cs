using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using System.IO;
using UnityEditor;

public class Attr
{
    public string receipt;
    public string amount;
}

public sealed class FileIO
{
    public static void Write(List<Attr> wordList)
    {

        XmlDocument document = new XmlDocument();
        XmlElement receiptListElement = document.CreateElement("ReceiptList");
        document.AppendChild(receiptListElement);

        foreach (Attr attr in wordList)
        {
            XmlElement receiptElement = document.CreateElement("Receipts");
            receiptElement.SetAttribute("Receipt", attr.receipt.ToString());
            receiptElement.SetAttribute("Amount", attr.amount.ToString());
            receiptListElement.AppendChild(receiptElement);
        }
        document.Save(Application.dataPath + "/Receipt_Resource.xml");
    }

    public static List<Attr> Read()
    {
        //TextAsset textAsset = (TextAsset)Resources.Load(fileName);
        //string filePath = textAsset.text;
        XmlDocument document = new XmlDocument();
        document.Load(Application.dataPath + "/Receipt_Resource.xml");
        XmlElement receiptListElement = document["ReceiptList"];

        List<Attr> receiptList = new List<Attr>();

        foreach (XmlElement receiptElement in receiptListElement.ChildNodes)
        {
            Attr attr = new Attr();
            attr.receipt = receiptElement.GetAttribute("Receipt");
            attr.amount = receiptElement.GetAttribute("Amount");
            receiptList.Add(attr);
        }
        return receiptList;
    }
}