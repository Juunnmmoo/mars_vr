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

    //��Ȧ�� ���� ���� �ö��� �� �ε� �� �þ, �ε��� ���� ��쿣 EndUI�� ������
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

    //UI�����ִ� �޼ҵ� (���̵� ��, �ſ� ��� ������ ���)
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
        playTimeStr = playMin.ToString() + "�� " + Mathf.Round(playSec).ToString() + "��";
        Debug.LogError(playTimeStr);
    }
}
