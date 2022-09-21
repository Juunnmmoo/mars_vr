using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CupCtrl : MonoBehaviour
{
    //���� ��
    private PlayerCtrl player;
    private int bias = 0;
    //���ǰ� ������
    [HideInInspector]
    public OVRGrabbable ovrGrabbable;

    [Header("������ ����")]
    //������ ���
    public List<BottleType> receipt;
    public List<Attr> correct;
    //������ ��
    public List<float> amounts;

    [Header("UI ����")]
    //MLǥ��(UI)
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

    //����Ʈ ��� ���ڰ����� �������
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

    //������ ��Ͽ� ��ᰡ �����ϴ��� üũ, ���� ���� �����ǿ� ���ϴ� �ε����� �Ÿ�
    //�������� ���� ��� -256 ��ȯ
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
        //            Debug.LogWarning("����!" + ((float.Parse(correct[idx].amount) * 1.1f) - amounts[amounts.Count - 1]).ToString());
        //            player.score += (float.Parse(correct[idx].amount) * 1.1f) - amounts[amounts.Count - 1];
        //        }
        //        if ((idx > 0)
        //            && (amounts.Count > 1)
        //            && (float.Parse(correct[idx - 1].amount) * 0.9f > amounts[amounts.Count - 2]))
        //        {
        //            Debug.LogWarning("����!" + ((float.Parse(correct[idx - 1].amount) * 0.9f) - amounts[amounts.Count - 2]).ToString());
        //            player.score += (float.Parse(correct[idx - 1].amount) * 0.9f) - amounts[amounts.Count - 2];
        //        }
        //        if (amounts[amounts.Count - 1] >= (float.Parse(correct[idx].amount)))
        //            idx++;
        //    }
        //    else
        //    {
        //        player.score -= 5;
        //        Debug.LogWarning("�ٸ�! : " + receipt[receipt.Count - 1].ToString() + ", " + correct[idx].receipt);
        //    }
        int idx = 0;
        IntergrateReceipt();
        List<BottleType> answerReceipt = new List<BottleType>();
        List<float> answerAmounts = new List<float>();



        //���� üũ
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
            //�����ϰ� ������ �´� ���
            if (IsExistReceipt(receipt[i], i) == 0)
            {
                Debug.LogWarning("�׸��� ������");
                answerReceipt.Add(receipt[i]);
                answerAmounts.Add(amounts[i]);

            }
            //�׸��� �������� �ʴ� ���
            else if (IsExistReceipt(receipt[i], i) == -256)
            {
                Debug.LogWarning("�׸��� �������� �ʴ� ���");
                player.score -= 10;
            }
            //�׸��� �����ϳ� ������ ���� �ʴ� ���
            else
            {
                Debug.LogWarning("�׸��� �����ϳ� ������ ���� �ʴ� ���");
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
        // �� ��ġ �ʱ�ȭ �޼ҵ�
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
        //isUsing�� ���� UI ���̱�
        amountUI.SetActive(ovrGrabbable.isGrabbed);
        if (ovrGrabbable.isGrabbed && amounts.Count > 0)
        {
            //��� ���̰� amounts List�� ũ�Ⱑ 0 �̻��� �� ������ �ε����� amounts�� ���� ������
            amountUI.GetComponentInChildren<Text>().text = (Mathf.Round(amounts[amounts.Count - 1])).ToString() + "ml";
        }
    }

    /*
     * ������ �߰� �Լ�
     * �����Ǹ� �߰��� �� ���� �ܰ�� ������ �������� ���� ������
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
            //3�ʰ� ������ �� ��� ���̰ų� �� ���� ��� ��� �ڷ�ƾ Ż��
            if (ovrGrabbable.isGrabbed || cupholder.isHolding)
            {
                isMoved = false;
                yield break;
            }
            delta += Time.deltaTime;
            yield return null;
        }
        //returnTime��ŭ ������
        if (delta >= returnTime)
        {
            transform.position = originPos;
            transform.rotation = Quaternion.Euler(originRot);
            isMoved = false;
        }
    }
}
