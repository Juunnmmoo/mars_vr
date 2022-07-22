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

    [Header("Bottle Type 관련 (타입, etc...)")]
    public BottleType bottleType;
    public Transform bottleTop;                     //병 입구 (여기에서 붓는 이펙트 넣어주기)
    public Transform bottleBottom;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.LogWarning("병이 따라지는지? : " + isPoured());
    }

    /*
     * 현재 병이 따라지고 있는 지 판단 여부
     * Top y가 Bottom y보다 낮을 때 true
     */
    public bool isPoured()
    {
        Debug.Log(bottleTop.position.y+", "+ bottleBottom.position.y);
        if (bottleTop.position.y > bottleBottom.position.y)
            return false;
        else
            return true;
    }
}
