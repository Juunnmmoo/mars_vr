using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvaluateManager : MonoBehaviour
{
    public GameObject endUI;
    public Text endUIText;
    public Image background;
    public Image loadingBar;
    [SerializeField]
    [Range(1f, 3f)]
    private float enableTime = 0f;
    private bool isHolding = false;
    private bool isEnd = false;
    private float elapsedTime = 0f;
    private GameObject cup;

    // Start is called before the first frame update
    void Start()
    {
        endUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Loading();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Cup"))
        {
            cup = other.gameObject;
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
        if(isEnd != (elapsedTime / enableTime >= 1f))
        {
            isEnd = true;
            ShowEndUI();
        }
    }
    private void ShowEndUI()
    {
        endUI.SetActive(true);
        StartCoroutine(FadeUI(0.5f));
        List<BottleCtrl.BottleType> receipts = cup.GetComponent<CupCtrl>().receipt;
        List<float> amount = cup.GetComponent<CupCtrl>().amounts;

        endUIText.text = "Receipt\n";
        for (int i = 0; i < receipts.Count; i++)
        {
            endUIText.text += receipts[i].ToString() + " : " + Mathf.Round(amount[i]).ToString() + "\n";
        }
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
}
