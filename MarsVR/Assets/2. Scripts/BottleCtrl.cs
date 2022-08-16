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
[RequireComponent(typeof(OVRGrabbable), typeof(Rigidbody))]

public class BottleCtrl : MonoBehaviour
{
    public enum BottleType
    {
        NONE,
        TEQUILA,
        WHITERUM,
        LEMON
    }
    [Header("Bottle Type ���� (Ÿ��, etc...)")]
    public BottleType bottleType;
    [Header("Raycast")]
    public Transform bottleTop;                     //�� �Ա� (���⿡�� �״� ����Ʈ �־��ֱ�)
    public Transform bottleBottom;
    [Range(1f, 10f)]
    public float rayDist = 5f;                           //����ĳ��Ʈ ��Ÿ�
    private RaycastHit hit;
    private Liquid liquid = null;

    public GameObject liquidPrefab;
    private bool isPouring = false;
    public GameObject nameUI;
    public AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        gameObject.GetComponentInChildren<Text>().text = name;
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
            if (isPouring)
            {
                StartPour();
                SoundManager.instance.PlayAudioSmooth(audioSource, SoundManager.instance.water1, 0.5f, 1f);
            }
            else
            {
                EndPour();
                SoundManager.instance.StopAudioSmooth(audioSource, 0.5f, 1f);
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
                    hit.transform.gameObject.GetComponent<CupCtrl>().AddReceipt(bottleType, amount);
                }
            }
        }
    }
}
