using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private List<Attr> xmlList = new List<Attr>();
    List<string> scriptList = new List<string>();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }  
        else
            Destroy(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        xmlList = FileIO.ReadReceipt();

        foreach(Attr attr in xmlList)
        {
            Debug.LogWarning(attr.receipt + ", " + attr.amount);
        }


        scriptList = FileIO.ReadScript();
        foreach(string str in scriptList)
        {
            Debug.LogWarning(str);
        }
    }
}
