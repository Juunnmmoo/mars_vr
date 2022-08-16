using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleManager : MonoBehaviour
{
    //초기화 시간
    [Range(1f, 5f)]
    public float returnTime;
    //Bottle 게임오브젝트 배열
    private GameObject[] bottles;
    //Bottle의 원위치
    private List<Transform> originBottleTransform;
    //움직이고 returnTime만큼 지났는지 체크하는 필드 (없으면 코루틴이 업데이트되면서 returnTime동안 계속 실행)
    private bool[] isMoved;
    // Start is called before the first frame update
    void Start()
    {
        //오브젝트의 원 위치 등 초기화
        originBottleTransform = new List<Transform>();
        bottles = GameObject.FindGameObjectsWithTag("Bottle");
        isMoved = new bool[bottles.Length];
        for (int i = 0; i < bottles.Length; i++)
        {
            //깊은 복사를 위해 새 객체 만들어서 넣어줌
            Transform bTemp = new GameObject().GetComponent<Transform>();
            bTemp.position = bottles[i].transform.position;
            bTemp.rotation = bottles[i].transform.rotation;
            originBottleTransform.Add(bTemp);

            //초기화
            isMoved[i] = false;
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        InitialBottlePos();
    }

    private void InitialBottlePos()
    {
        for (int i = 0; i < bottles.Length; i++)
        {
            //원 위치와 다르고 해당 객체가 움직이지 않으며 사용 중이 아닐 경우 
            if (bottles[i].transform.position.x != originBottleTransform[i].position.x &&
                bottles[i].transform.position.z != originBottleTransform[i].position.z &&
                !bottles[i].GetComponent<OVRGrabbable>().isGrabbed && !isMoved[i])
            {
                isMoved[i] = true;
                StartCoroutine(InitialBottlePosCor(i));
            }
        }
    }

    IEnumerator InitialBottlePosCor(int idx)
    {
        float delta = 0;
        while (delta < returnTime)
        {
            //3초가 지나기 전 사용 중일 경우 코루틴 탈출
            if (bottles[idx].GetComponent<OVRGrabbable>().isGrabbed)
            {
                isMoved[idx] = false;
                yield break;
            }
            delta += Time.deltaTime;
            yield return null;
        }
        //returnTime만큼 지나면
        if (delta >= returnTime)
        {
            bottles[idx].transform.position = originBottleTransform[idx].position;
            bottles[idx].transform.rotation = originBottleTransform[idx].rotation;
            isMoved[idx] = false;
        }
    }
}
