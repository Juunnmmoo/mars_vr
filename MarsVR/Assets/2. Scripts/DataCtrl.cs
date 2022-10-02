using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Data;

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
    public ScoreCtrl scorectrl;
    public InputField playerNameInput;
    public string playerName;
    public int userScore;

    void Start()
    {
        // playerName = "박명지";
        playerName = playerNameInput.GetComponent<InputField>().text.ToString();
        // userScore = int.Parse(scorectrl.myScore.text.ToString());
        userScore = (int)GameManager.instance.GetTotalScore().totalScore;

        User user1 = new User
        {
            name = playerName,
            score = userScore,
            playing_time = "0",
            kind = "baker"
        };

        string json = JsonUtility.ToJson(user1);

        StartCoroutine(Upload("http://localhost:8080/play", json));

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
            }
        }
    }
}