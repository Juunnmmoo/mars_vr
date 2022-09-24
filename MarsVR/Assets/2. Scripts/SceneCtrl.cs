using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCtrl : MonoBehaviour
{
    public static SceneCtrl Instance { get; private set; }

    //public int Level1IsOver = 0;

    private void Awake()
    {
        if (Instance != null) { return; }

        DontDestroyOnLoad(this);
        Instance = this;
        
    }

    public void NextTo(string scene) {
        SceneManager.LoadScene(scene);
    }
    public void ToLevel2() {
        //Level1IsOver = 1;
        SceneManager.LoadScene("Bar");
    }
    

}
