using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
    ������ ��Ģ��
    Class = �Ľ�Į
    Method = �Ľ�Į
    Variable = ī��
    ���ϸ� = �Ľ�Į

    �� BottleCtrl ��ü�� ������ BottleType�� ������ ����.
    Bottle ��ü�� �Ա����� RayCast�� �Ͽ� Cup Bottom�� �ݶ��̴��� ���� ��� �ſ� ������ ������ �Ǵ�.
    Cup ��ü�� ���� �� CupCtrl�� receipt ����Ʈ�� �߰�
    �� receipt �ʵ带 ���� ���ϴ� ������ ����

 */

public enum BottleType
{
    NONE,
    TEQUILA,
    WHITE_RUM,
    VODKA,
    PEACH_TREE,
    DRY_GIN,
    LEMON,

}

[RequireComponent(typeof(OVRGrabbable), typeof(Rigidbody))]

public class BottleCtrl : MonoBehaviour
{

    [Header("Bottle Type ���� (Ÿ��, etc...)")]
    public BottleType bottleType;

    [Header("Raycast")]
    public Transform bottleTop;                     //�� �Ա� (���⿡�� �״� ����Ʈ �־��ֱ�);
    public Transform bottleBottom;
    public GameObject offset;
    private CupCtrl curCup;
    private bool isExistCup;

    [Header("Liquid Associated...")]
    [Range(1f, 10f)]
    public float rayDist = 5f;                           //����ĳ��Ʈ ��Ÿ�
    private RaycastHit hit;
    private Liquid liquid = null;
    public GameObject liquidPrefab;
    private bool isPouring = false;

    [Header("UI Associated...")]
    public GameObject nameUI;

    //���� ����
    [Header("Sounds Associated...")]
    public AudioSource audioSource;
    private Coroutine nowCor;

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        gameObject.GetComponentInChildren<Text>().text = name;
        nameUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        PouringLiquid();

        nameUI.SetActive(gameObject.GetComponent<OVRGrabbable>().isGrabbed);
        //�� ���� (IsPoured()�� ���� �ٲ� ������ ȣ���
        if(isPouring != IsPoured())
        {
            isPouring = IsPoured();
            //������ ������ ���� ��
            if (isPouring)
            {
                //�� �ױ�
                StartPour();
                //�ε巴�� �� �Ҹ� ����
                nowCor = SoundManager.instance.PlayAudioSmooth(audioSource, SoundManager.instance.waterSound2, 0.5f, 1f, true);
            }
            //�������� �ʾ��� ��
            else
            {
                //�� �ױ� �ߴ�
                EndPour();
                //�ε巴�� �� �Ҹ� ���� (���� PlayAudioSmooth �ڷ�ƾ�� ���ư��ٸ� �ش� �ڷ�ƾ�� ������Ű�� �ε巴�� ����������)
                if(nowCor != null)
                {
                    SoundManager.instance.GetComponent<SoundManager>().StopCoroutine(nowCor);
                    nowCor = null;
                }
                SoundManager.instance.StopAudioSmooth(audioSource, 0.5f);

                //���� ���÷� ����ĳ��Ʈ�� ������ �� ���� ��
                if (isExistCup && curCup != null)
                {
                    isExistCup = false;
                }
                curCup = null;
            }
        }

    }

    private void StartPour()
    {
        liquid = CreateLiquid();
        liquid.Begin();
    }
    private void EndPour()
    {
        liquid.End();
        liquid = null;
    }

    private Liquid CreateLiquid()
    {
        GameObject temp = Instantiate(liquidPrefab, bottleTop.position, Quaternion.identity, transform);
        return temp.GetComponent<Liquid>();
    }

    /*
     * ���� ���� �������� �ִ� �� �Ǵ� ����
     * Top y�� Bottom y���� ���� �� true
     */
    public bool IsPoured()
    {
        if (bottleTop.position.y > bottleBottom.position.y)
            return false;
        else
            return true;
    }

    public void PouringLiquid()
    {
        if (IsPoured())
        {
            //�� ��(x��, z��)�� �����̼� ��, �� ���� ������ ����� �� �� 0 -> 10���� ���� �������
            float xRot = (transform.eulerAngles.x < 180 ? 90 - transform.eulerAngles.x : transform.eulerAngles.x - 270) / 90 * 10;
            float zRot = Mathf.Abs(Mathf.Abs(transform.localEulerAngles.z - 180) - 90) / 90 * 10;

            //�� ���� ���� �� �������� �������(0 ~ 100�� ���� 0 ~ 10���� �������)
            float amount = Mathf.Sqrt(xRot * zRot);
            //������, �� �信�� ����ĳ��Ʈ�� �ð������� ������ (�ΰ��ӿ��� �Ⱥ���)
            Debug.DrawRay(bottleTop.position, Vector3.down * rayDist, Color.blue);


            if (Physics.Raycast(bottleTop.position, Vector3.down, out hit, rayDist))
            {
                //����ĳ��Ʈ�� �浿�� ��ü�� �±װ� Cup�� ���
                if (hit.transform.gameObject.CompareTag("Cup"))
                {
                    //������� ȯ��, �ʴ� 0 ~ 10��ŭ ä����
                    isExistCup = true;
                    curCup = hit.transform.gameObject.GetComponent<CupCtrl>();
                    curCup.AddReceipt(bottleType, amount);
                }
                else
                {
                    //����ĳ��Ʈ�� ���� �ʴ� �������� ���� ��
                    if (isExistCup && curCup != null)
                    {
                        isExistCup = false;
                    }
                    curCup = null;
                }
            }
        }
    }
}
