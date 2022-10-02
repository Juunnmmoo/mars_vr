using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCtrl : MonoBehaviour
{

    public Scene scene;
    
    public void NextTo(string scene) {
        SceneManager.LoadScene(scene);
    }

    
    public void ToScore() {
        if(scene.name == "bar") {
            SceneManager.LoadScene("BarScore");
        }else
        {
            SceneManager.LoadScene("BakerScore");
        }
    }
    public void ToGameTitle()
    {
        SceneManager.LoadScene("GameTitle");
    }

    public void ToGameSelect()
    {
        SceneManager.LoadScene("GameSelect");
    }
    public void ToTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
    public void ToBaker()
    {
        SceneManager.LoadScene("baker");
    }
    public void ToBar() {
        SceneManager.LoadScene("bar");
    }
}
