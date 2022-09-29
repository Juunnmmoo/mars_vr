using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvaluateManager : MonoBehaviour
{
    private PlayerCtrl player;
    [Header("���� UI ����")]
    public GameObject endUI;
    public Text endUIText;
    public Image background;
    public Image loadingBar;
    [Header("�ε� �ٰ� ���� ä������ ������ �ð�")]
    [SerializeField]
    [Range(1f, 3f)]
    private float enableTime = 2f;
    public bool isHolding = false;
    [HideInInspector]
    public bool isEnd = false;
    private float elapsedTime = 0f;
    private CupCtrl cup;
    [Header("����ð� ����")]
    public string endTimeStr;
    private float playTime;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerCtrl>();
        endUI.SetActive(false);
        StartCoroutine(LoadingCor());
        StartCoroutine(PlayTimeCor());
    }

    private void OnTriggerEnter(Collider other)
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

    //��Ȧ�� ���� ���� �ö��� �� �ε� �� �þ, �ε��� ���� ��쿣 EndUI�� ������
    private void Loading()
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
            ShowEndUI();
        }
    }

    //UI�����ִ� �޼ҵ� (���̵� ��, �ſ� ��� ������ ���)
    private void ShowEndUI()
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

    IEnumerator FadeUI(float fadeTime)
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

    IEnumerator LoadingCor()
    {
        while (!isEnd)
        {
            Loading();
            yield return null;
        }
    }

    IEnumerator PlayTimeCor()
    {
        while (!isEnd)
        {
            playTime += Time.deltaTime;
            yield return null;
        }

        playTime = Mathf.Round(playTime);
        int min = (int)(playTime / 60);
        float sec = Mathf.Round(playTime % min * 60);
        endTimeStr = min.ToString() + "�� " + sec.ToString() + "��";
        Debug.LogError(endTimeStr);
    }
}
