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
    public static void WriteReceipt(List<Attr> wordList)
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

    public static List<Attr> ReadReceipt(string receiptName)
    {
        XmlDocument document = new XmlDocument();
        document.Load(Application.dataPath + "/Receipt_Resource.xml");
        //XmlElement receiptListElement = document[receiptName];

        XmlNodeList receiptListElement = document.SelectNodes("ReceiptList/" + receiptName);
        List<Attr> receiptList = new List<Attr>();

        foreach (XmlElement receiptElement in receiptListElement)
        {
            Attr attr = new Attr();
            attr.receipt = receiptElement.GetAttribute("Receipt");
            attr.amount = receiptElement.GetAttribute("Amount");
            receiptList.Add(attr);
        }
        return receiptList;
    }

    public static List<string> ReadScript(string scriptName)
    {
        XmlDocument document = new XmlDocument();
        document.Load(Application.dataPath + "/Script_Resource.xml");
        //XmlElement receiptListElement = document["ScriptList"];
        XmlNodeList scriptListElement = document.SelectNodes("ScriptList/" + scriptName);
        List<string> scriptList = new List<string>();

        foreach (XmlElement scriptElement in scriptListElement)
        {
            string temp = scriptElement.GetAttribute("Script");
            scriptList.Add(temp);
        }
        return scriptList;
    }
}