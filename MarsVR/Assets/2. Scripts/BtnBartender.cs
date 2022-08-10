using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnBartender : MonoBehaviour
{
    public void SceneChange() {
        SceneManager.LoadScene("SampleScene");
    }
}
