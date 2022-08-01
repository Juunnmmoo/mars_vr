using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CupCtrl : MonoBehaviour
{
    //레시피 재료
    public List<BottleCtrl.BottleType> receipt;
    //레시피 양
    public List<float> amounts;

    public GameObject bottle;
    public float bottleRotationX;
    public Vector3 bottleAngles;

    //ML표시(UI)
    private GameObject ML;
    //amounts int형
    int newAmounts;

    //사용되고 있을때
    public bool isUsing;


    // Start is called before the first frame update
    void Start()
    {
        bottle = GameObject.Find("Bottle");
        ML = GameObject.Find("Canvas/ML");
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

        //ML표시(UI) CUP 따라다니기
        ML.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0.3f, 0.1f, 0.1f));
        newAmounts = (int)amounts[amounts.Count - 1];

        //사용되고 있을때 MLUI 증가
        if (isUsing)
        {
            ML.gameObject.SetActive(true);
            if (amounts.Count > 0)
            {
                ML.GetComponent<Text>().text = newAmounts.ToString() + " ML";
            }
        }
        else
            ML.gameObject.SetActive(false);
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
