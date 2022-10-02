using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetName : MonoBehaviour
{
    public InputField InputName;
    public string userName;

    public void GetInputName()
    {
        userName = InputName.text;
        Debug.Log(userName);
        
    }

}
