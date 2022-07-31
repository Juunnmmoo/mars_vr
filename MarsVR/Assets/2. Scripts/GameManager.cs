using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private List<Attr> xmlList = new List<Attr>();

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
        xmlList = FileIO.Read();

        foreach(Attr attr in xmlList)
        {
            Debug.LogWarning(attr.receipt + ", " + attr.amount);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

}
