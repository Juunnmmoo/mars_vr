using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnBack : MonoBehaviour
{
    public void SceneChange() {
        SceneManager.LoadScene("GameTitle");
    }
}
