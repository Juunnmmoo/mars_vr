using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCtrl : MonoBehaviour
{
    public void NextTo(string scene) {
        SceneManager.LoadScene(scene);
    }
    public void ToScore() {
        SceneManager.LoadScene("Score");
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
        SceneManager.LoadScene("Baker");
    }
    public void ToBar() {
        SceneManager.LoadScene("Bar");
    }
}
