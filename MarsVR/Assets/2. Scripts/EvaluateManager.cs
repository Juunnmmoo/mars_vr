using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvaluateManager : MonoBehaviour
{
    private PlayerCtrl player;
    [Header("종료 UI 관련")]
    public GameObject endUI;
    public Text endUIText;
    public Image background;
    public Image loadingBar;
    [Header("로딩 바가 전부 채워지기 까지의 시간")]
    [SerializeField]
    [Range(1f, 3f)]
    private float enableTime = 2f;
    public bool isHolding = false;
    [HideInInspector]
    public bool isEnd = false;
    private float elapsedTime = 0f;
    private CupCtrl cup;
    [Header("종료시간 관련")]
    [HideInInspector]
    public string playTimeStr;
    private float playMin;
    private float playSec;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerCtrl>();
        endUI.SetActive(false);
        StartCoroutine(LoadingCor());
        StartCoroutine(PlayTimeCor());
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Cup"))
        {
            cup = other.gameObject.GetComponent<CupCtrl>();
            isHolding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Cup"))
        {
            cup = null;
            isHolding = false;
        }
    }

    //컵홀더 위에 컵이 올라갔을 때 로딩 바 늘어남, 로딩이 됐을 경우엔 EndUI를 보여줌
    protected void Loading()
    {
        if (isHolding && elapsedTime < enableTime)
        {
            elapsedTime += Time.deltaTime;
        }
        else if (!isHolding && elapsedTime > 0)
        {
            elapsedTime -= Time.deltaTime;
        }
        loadingBar.fillAmount = elapsedTime / enableTime;
        if(!isEnd && (elapsedTime / enableTime >= 1f) && !isEnd)
        {
            isEnd = true;
            List<string> temp = new List<string>();
            for (int i = 0; i < cup.receipt.Count; i++)
            {
                temp.Add(cup.receipt[i].ToString());
            }
            GameManager.instance.receipt.Add(temp);
            GameManager.instance.amount.Add(cup.amounts);
            ShowEndUI();
        }
    }

    //UI보여주는 메소드 (페이드 인, 컵에 담긴 레시피 출력)
    protected void ShowEndUI()
    {
        if (cup == null)
            return;
        endUI.SetActive(true);
        StartCoroutine(FadeUI(0.5f));
        cup.Evaluation();

        endUIText.text = "Receipt\n";
        for (int i = 0; i < cup.receipt.Count; i++)
        {
            endUIText.text += cup.receipt[i].ToString() + " : " + Mathf.Round(cup.amounts[i]).ToString() + "\n";
        }
        endUIText.text += "Score : " + Mathf.Round(player.score).ToString();
    }

    protected IEnumerator FadeUI(float fadeTime)
    {
        float a = 0;
        Color backColor = background.color;
        Color textColor = endUIText.color;
        while (a < 1)
        {
            a += Time.deltaTime / fadeTime;
            backColor.a = a * (float)(200f / 255f);
            textColor.a = a;
            background.color = backColor;
            endUIText.color = textColor;
            yield return null;
        }
    }

    protected IEnumerator LoadingCor()
    {
        while (!isEnd)
        {
            Loading();
            yield return null;
        }
    }

    protected IEnumerator PlayTimeCor()
    {
        while (!isEnd)
        {
            playSec += Time.deltaTime;
            if(playSec >= 60)
            {
                playMin += 1;
                playSec = 0;
            }
            yield return null;
        }
        playTimeStr = playMin.ToString() + "분 " + Mathf.Round(playSec).ToString() + "초";
        Debug.LogError(playTimeStr);
    }
}
