using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class BottleCtrl : MonoBehaviour
{
    public enum BottleType
    {
        NONE,
        BOTTLETYPE1,
        BOTTLETYPE2,
        BOTTLETYPE3,
    }

    public bool isUsing;                            //현재 사용 중인 병인지
    [Header("Bottle Type 관련 (타입, etc...)")]
    public BottleType bottleType;
    [Header("Raycast")]
    public Transform bottleTop;                     //병 입구 (여기에서 붓는 이펙트 넣어주기)
    public Transform bottleBottom;
    [Range(1f, 10f)]
    public float rayDist;                           //레이캐스트 사거리
    private RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PouringLiquid();
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
            //z축 로테이션 값 = 컵의 기울기 값(0 ~ 90)
            float zRot = Mathf.Abs(Mathf.Abs(transform.localEulerAngles.z - 180) - 90);
            //디버깅용, 씬 뷰에서 레이캐스트를 시각적으로 보여줌 (인게임에선 안보임)
            Debug.DrawRay(bottleTop.position, Vector3.down * rayDist, Color.blue);
            if (Physics.Raycast(bottleTop.position, Vector3.down, out hit, rayDist))
            {
                //레이캐스트에 충동한 물체의 태그가 Cup일 경우
                if (hit.transform.gameObject.CompareTag("Cup"))
                {
                    //백분율로 환산, 초당 1 ~ 10만큼 채워줌
                    hit.transform.gameObject.GetComponent<CupCtrl>().AddReceipt(bottleType, zRot / 90 * 10);
                }
            }
        }
    }
}
