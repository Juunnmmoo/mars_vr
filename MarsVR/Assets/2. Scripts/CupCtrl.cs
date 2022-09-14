using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CupCtrl : MonoBehaviour
{
    //���ǰ� ������
    [HideInInspector]
    public OVRGrabbable ovrGrabbable;

    [Header("������ ����")]
    //������ ���
    public List<BottleCtrl.BottleType> receipt;
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
        cupholder = GameObject.Find("CupHolder").GetComponent<EvaluateManager>();
        ovrGrabbable = gameObject.GetComponent<OVRGrabbable>();
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
