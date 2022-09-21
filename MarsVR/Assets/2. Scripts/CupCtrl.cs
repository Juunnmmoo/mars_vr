using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CupCtrl : MonoBehaviour
{
    //점수 평가
    private PlayerCtrl player;
    private int bias = 0;
    //사용되고 있을때
    [HideInInspector]
    public OVRGrabbable ovrGrabbable;

    [Header("레시피 관련")]
    //레시피 재료
    public List<BottleType> receipt;
    public List<Attr> correct;
    //레시피 양
    public List<float> amounts;

    [Header("UI 관련")]
    //ML표시(UI)
    public GameObject amountUI;

    [Range(1f, 5f)]
    public float returnTime;
    private EvaluateManager cupholder;
    private bool isMoved;
    private Vector3 originPos;
    private Vector3 originRot;


    // Start is called before the first frame update
    void Start()
    {
        correct = FileIO.ReadReceipt("Tutorial");
        cupholder = GameObject.Find("CupHolder").GetComponent<EvaluateManager>();
        ovrGrabbable = gameObject.GetComponent<OVRGrabbable>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerCtrl>();
        originPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        originRot = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);
    }

    // Update is called once per frame
    void Update()
    {
        AmountUI();
        InitialCupPos();
    }

    //리스트 요소 원자값으로 만들어줌
    public void IntergrateReceipt()
    {
        for (int i = 0; i < receipt.Count; i++)
        {
            for (int j = 0; j < receipt.Count; j++)
            {
                if (i == j)
                    continue;

                if (receipt[i].Equals(receipt[j]))
                {
                    amounts[i] += amounts[j];
                }
            }
        }
        receipt = receipt.Distinct().ToList();
        for (int i = receipt.Count; i < amounts.Count; i++)
        {
            amounts.RemoveAt(i);
        }
    }

    //레시피 목록에 재료가 존재하는지 체크, 리턴 값은 레시피와 비교하는 인덱스의 거리
    //존재하지 않을 경우 -256 반환
    private int IsExistReceipt(BottleType bottleType, int idx)
    {
        for (int i = 0; i < correct.Count; i++)
        {
            if (correct[i].receipt.Equals(bottleType.ToString()))
            {
                Debug.LogWarning(bottleType.ToString() + ", " + (idx - i));
                return idx - i;
            }
        }
        return -256;
    }

    public void Evaluation()
    {
        //    if (receipt[receipt.Count - 2].Equals(bottleType) && receipt.Count > 1)
        //        idx--;

        //    if (receipt[receipt.Count - 1].ToString().Equals(correct[idx].receipt)) 
        //    {
        //        if (float.Parse(correct[idx].amount) * 1.1f < amounts[amounts.Count - 1])
        //        {
        //            Debug.LogWarning("높음!" + ((float.Parse(correct[idx].amount) * 1.1f) - amounts[amounts.Count - 1]).ToString());
        //            player.score += (float.Parse(correct[idx].amount) * 1.1f) - amounts[amounts.Count - 1];
        //        }
        //        if ((idx > 0)
        //            && (amounts.Count > 1)
        //            && (float.Parse(correct[idx - 1].amount) * 0.9f > amounts[amounts.Count - 2]))
        //        {
        //            Debug.LogWarning("낮음!" + ((float.Parse(correct[idx - 1].amount) * 0.9f) - amounts[amounts.Count - 2]).ToString());
        //            player.score += (float.Parse(correct[idx - 1].amount) * 0.9f) - amounts[amounts.Count - 2];
        //        }
        //        if (amounts[amounts.Count - 1] >= (float.Parse(correct[idx].amount)))
        //            idx++;
        //    }
        //    else
        //    {
        //        player.score -= 5;
        //        Debug.LogWarning("다름! : " + receipt[receipt.Count - 1].ToString() + ", " + correct[idx].receipt);
        //    }
        int idx = 0;
        IntergrateReceipt();
        List<BottleType> answerReceipt = new List<BottleType>();
        List<float> answerAmounts = new List<float>();



        //순서 체크
        if (correct.Count > receipt.Count)
        {
            for (int i = 0; i < correct.Count; i++)
            {
                if (!correct[i].receipt.Equals(receipt[i].ToString()))
                {
                    receipt.Insert(i, BottleType.NONE);
                    amounts.Insert(i, 0);
                    if (correct.Count == receipt.Count)
                        break;
                }
            }
        }

        for (int i = 0; i < receipt.Count; i++)
        {
            //존재하고 순서가 맞는 경우
            if (IsExistReceipt(receipt[i], i) == 0)
            {
                Debug.LogWarning("항목이 존재함");
                answerReceipt.Add(receipt[i]);
                answerAmounts.Add(amounts[i]);

            }
            //항목이 존재하지 않는 경우
            else if (IsExistReceipt(receipt[i], i) == -256)
            {
                Debug.LogWarning("항목이 존재하지 않는 경우");
                player.score -= 10;
            }
            //항목이 존재하나 순서가 맞지 않는 경우
            else
            {
                Debug.LogWarning("항목이 존재하나 순서가 맞지 않는 경우");
                idx = IsExistReceipt(receipt[i], i);
                answerReceipt.Add((BottleType)System.Enum.Parse(typeof(BottleType), correct[i].receipt));
                float tempAmount = amounts[i];
                amounts[i] = amounts[i - idx];
                amounts[i - idx] = tempAmount;
                answerAmounts.Add(amounts[i]);
                player.score -= 5;
            }
        }
        idx = 0;
        for (int i = 0; i < answerAmounts.Count; i++)
        {
            if (answerReceipt.Equals(BottleType.NONE))
                continue;

            if (float.Parse(correct[idx].amount) * 1.1f < answerAmounts[i])
            {
                player.score -= answerAmounts[i] - float.Parse(correct[idx].amount) * 1.1f;
            }
            else if(float.Parse(correct[idx].amount) * 0.9f > answerAmounts[i])
            {
                player.score -= float.Parse(correct[idx].amount) - answerAmounts[i];
            }
            idx++;
        }

        for (int i = 0; i < answerReceipt.Count; i++)
        {
            Debug.LogWarning(answerReceipt[i] + " : " + answerAmounts[i]);
        }

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
        amountUI.SetActive(ovrGrabbable.isGrabbed);
        if (ovrGrabbable.isGrabbed && amounts.Count > 0)
        {
            //사용 중이고 amounts List의 크기가 0 이상일 때 마지막 인덱스의 amounts의 양을 보여줌
            amountUI.GetComponentInChildren<Text>().text = (Mathf.Round(amounts[amounts.Count - 1])).ToString() + "ml";
        }
    }

    /*
     * 레시피 추가 함수
     * 레시피를 추가할 때 이전 단계와 같으면 레시피의 양을 더해줌
     */
    public void AddReceipt(BottleType bottleType, float inputAmount)
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
            if (ovrGrabbable.isGrabbed || cupholder.isHolding)
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
