using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Data;
using UnityEngine.SceneManagement;

[System.Serializable]
public class User
{
    public string name;
    public int score;
    public string playing_time;
    public string kind;
}

public class DataCtrl : MonoBehaviour
{
    public GameObject sentUI;
    [Range(1f, 5f)]
    public float sentUITime = 3;
    private float sentUIDelta;

    public ScoreCtrl scorectrl;
    public InputField playerNameInput;
    private string playerName;
    private string curScene;
    private string playTime;
    private int userScore;

    void Start()
    {
        sentUI.SetActive(false);
    }

    [ContextMenu("Send")]
    public void Send()
    {
        playerName = playerNameInput.GetComponent<InputField>().text.ToString();
        if (string.IsNullOrEmpty(playerName))
            return;
        playTime = GameManager.instance.GetPlayTime();
        curScene = PlayerPrefs.GetString("CurScene");
        userScore = GameManager.instance.GetTotalScore(curScene);

        User user1 = new User
        {
            name = playerName,
            score = userScore,
            playing_time = playTime,
            kind = curScene
        };

        string json = JsonUtility.ToJson(user1);

        StartCoroutine(Upload("http://184.72.49.233:8080/play", json));

        /*StartCoroutine(UnityWebRequestPOSTTEST());*/
    }
    
    IEnumerator Upload(string URL, string json)
    {
        using (UnityWebRequest request = UnityWebRequest.Post(URL, json))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);

            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if(request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log(request.downloadHandler.text);
                sentUI.SetActive(true);
                sentUIDelta = 0;
                while(sentUIDelta < sentUITime)
                {
                    sentUIDelta += Time.deltaTime;
                    yield return null;
                }
                sentUI.SetActive(false);
            }
        }
    }
}