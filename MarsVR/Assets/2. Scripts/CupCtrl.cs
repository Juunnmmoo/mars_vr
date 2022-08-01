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

    public GameObject bottle;
    public float bottleRotationX;
    public Vector3 bottleAngles;

    [Header("UI 관련")]
    //ML표시(UI)
    public Transform uiPos;
    public GameObject amountUI;
    //amounts int형
    int newAmounts;

    // Start is called before the first frame update
    void Start()
    {
        bottle = GameObject.Find("Bottle");
        //ML = GameObject.Find("Canvas/ML");
    }

    // Update is called once per frame
    void Update()
    {
        bottleAngles = bottle.transform.rotation.eulerAngles;

        if (bottleAngles.y >= 180 && bottleAngles.z >= 180)
        {
            if (bottleAngles.x <= 180)
            {
                bottleRotationX = 90 + (90 - bottleAngles.x);
            }
        }
        else
        {
            bottleRotationX = bottleAngles.x;
        }

        AmountUI();
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
    public void AddReceipt(BottleCtrl.BottleType bottleType)
    {
        if (receipt.Count == 0)
        {
            receipt.Add(bottleType);
            amounts.Add(1);
        }
        else
        {
            if (receipt[receipt.Count - 1].Equals(bottleType))
            {
                //amounts[amounts.Count - 1]++;
                
                if (bottleRotationX > 90 & bottleRotationX <= 180)
                {
                    if (bottleRotationX >= 140)
                    {
                        amounts[amounts.Count - 1] += 10.0f * Time.deltaTime;
                    }
                    else
                    {
                        amounts[amounts.Count - 1] += (bottleRotationX - 90) * 0.2f * Time.deltaTime;
                    }
                }

            }
            else
            {
                receipt.Add(bottleType);
                amounts.Add(1);
            }
        }
    }
}
