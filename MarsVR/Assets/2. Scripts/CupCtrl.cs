using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupCtrl : MonoBehaviour
{
    //레시피 재료
    public List<BottleCtrl.BottleType> receipt;
    //레시피 양
    public List<int> amounts;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
     * 레시피 추가 함수
     * 레시피를 추가할 때 이전 단계와 같으면 레시피의 양을 더해줌
     */
    public void AddReceipt(BottleCtrl.BottleType bottleType)
    {
        if(receipt.Count == 0)
        {
            receipt.Add(bottleType);
            amounts.Add(1);
        }
        else
        {
            if (receipt[receipt.Count - 1].Equals(bottleType))
            {
                amounts[amounts.Count - 1]++;
            }
            else
            {
                receipt.Add(bottleType);
                amounts.Add(1);
            }
        }
    }
}
