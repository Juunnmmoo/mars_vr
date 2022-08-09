using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CupCtrl : MonoBehaviour
{
    //사용되고 있을때
    public bool isUsing;

    [Header("레시피 관련")]
    //레시피 재료
    public List<BottleCtrl.BottleType> receipt;
    //레시피 양
    public List<float> amounts;

    [Header("UI 관련")]
    //ML표시(UI)
    public GameObject amountUI;

    [Range(1f, 5f)]
    public float returnTime;
    [SerializeField]
    private GameObject cupholder;
    private bool isMoved;
    private Vector3 originPos;
    private Vector3 originRot;


    // Start is called before the first frame update
    void Start()
    {
        cupholder = GameObject.Find("CupHolder");
        originPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        originRot = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);
    }

    // Update is called once per frame
    void Update()
    {
        AmountUI();
        InitialCupPos();
    }

    private void InitialCupPos()
    {
        // 컵 위치 초기화 메소드
        if (transform.position.x != originPos.x &&
            transform.position.z != originPos.z &&
            !isMoved)
        {
            isMoved = true;

            StartCoroutine(InitialCupPosCor());
        }
    }
    private void AmountUI()
    {
        //isUsing에 따라 UI 보이기
        amountUI.SetActive(isUsing);
        if (isUsing && amounts.Count > 0)
        {
            //사용 중이고 amounts List의 크기가 0 이상일 때 마지막 인덱스의 amounts의 양을 보여줌
            amountUI.GetComponentInChildren<Text>().text = ((int)amounts[amounts.Count - 1]).ToString() + "ml";
        }
    }

    /*
     * 레시피 추가 함수
     * 레시피를 추가할 때 이전 단계와 같으면 레시피의 양을 더해줌
     */
    public void AddReceipt(BottleCtrl.BottleType bottleType, float inputAmount)
    {
        if (receipt.Count == 0)
        {
            receipt.Add(bottleType);
            amounts.Add(inputAmount);
        }
        else
        {
            if (receipt[receipt.Count - 1].Equals(bottleType))
            {
                amounts[amounts.Count - 1] += inputAmount * Time.deltaTime;
            }
            else
            {
                receipt.Add(bottleType);
                amounts.Add(inputAmount);
            }
        }
    }

    IEnumerator InitialCupPosCor()
    {
        float delta = 0;
        while (delta < returnTime)
        {
            //3초가 지나기 전 사용 중이거나 평가 중일 경우 경우 코루틴 탈출
            if (isUsing || cupholder.GetComponent<EvaluateManager>().isHolding)
            {
                isMoved = false;
                yield break;
            }
            delta += Time.deltaTime;
            yield return null;
        }
        //returnTime만큼 지나면
        if (delta >= returnTime)
        {
            transform.position = originPos;
            transform.rotation = Quaternion.Euler(originRot);
            isMoved = false;
        }
    }
}
