using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
    변수명 규칙은
    Class = 파스칼
    Method = 파스칼
    Variable = 카멜
    파일명 = 파스칼

    각 BottleCtrl 객체는 고유한 BottleType을 가지고 있음.
    Bottle 객체의 입구에서 RayCast를 하여 Cup Bottom의 콜라이더에 닿을 경우 컵에 따르는 것으로 판단.
    Cup 객체에 따를 때 CupCtrl의 receipt 리스트에 추가
    이 receipt 필드를 통해 평가하는 식으로 진행

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
    [Header("Bottle Type 관련 (타입, etc...)")]
    public BottleType bottleType;
    [Header("Raycast")]
    public Transform bottleTop;                     //병 입구 (여기에서 붓는 이펙트 넣어주기)
    public Transform bottleBottom;
    [Range(1f, 10f)]
    public float rayDist = 5f;                           //레이캐스트 사거리
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
        //물 생성 (IsPoured()의 값이 바뀔 때마다 호출됨
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
     * 현재 병이 따라지고 있는 지 판단 여부
     * Top y가 Bottom y보다 낮을 때 true
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
            //각 축(x축, z축)의 로테이션 값, 각 축이 수직에 가까워 질 때 0 -> 10까지 수가 가까워짐
            float xRot = (transform.eulerAngles.x < 180 ? 90 - transform.eulerAngles.x : transform.eulerAngles.x - 270) / 90 * 10;
            float zRot = Mathf.Abs(Mathf.Abs(transform.localEulerAngles.z - 180) - 90) / 90 * 10;

            //각 축을 곱한 뒤 제곱근을 계산해줌(0 ~ 100의 값을 0 ~ 10으로 만들어줌)
            float amount = Mathf.Sqrt(xRot * zRot);
            //디버깅용, 씬 뷰에서 레이캐스트를 시각적으로 보여줌 (인게임에선 안보임)
            Debug.DrawRay(bottleTop.position, Vector3.down * rayDist, Color.blue);


            if (Physics.Raycast(bottleTop.position, Vector3.down, out hit, rayDist))
            {
                //레이캐스트에 충동한 물체의 태그가 Cup일 경우
                if (hit.transform.gameObject.CompareTag("Cup"))
                {
                    //백분율로 환산, 초당 0 ~ 10만큼 채워줌
                    hit.transform.gameObject.GetComponent<CupCtrl>().AddReceipt(bottleType, amount);
                }
            }
        }
    }
}
