using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCtrl : MonoBehaviour
{
    public void NextTo(string scene) {
        SceneManager.LoadScene(scene);
    }
    [ContextMenu("ToScore")]
    public void ToScore() {
        SceneManager.LoadScene("Score");
    }
    [ContextMenu("ToGameTitle")]
    public void ToGameTitle()
    {
        SceneManager.LoadScene("GameTitle");
    }

    [ContextMenu("ToGameSelect")]
    public void ToGameSelect()
    {
        SceneManager.LoadScene("GameSelect");
    }
    [ContextMenu("ToTutorial")]
    public void ToTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
    [ContextMenu("ToBaker")]
    public void ToBaker()
    {
        SceneManager.LoadScene("Baker");
    }
    [ContextMenu("ToBartender")]
    public void ToBar() {
        SceneManager.LoadScene("Bartender");
    }
}
