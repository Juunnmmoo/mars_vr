using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleManager : MonoBehaviour
{
    //�ʱ�ȭ �ð�
    [Range(1f, 5f)]
    public float returnTime;
    //Bottle ���ӿ�����Ʈ �迭
    private GameObject[] bottles;
    //Bottle�� ����ġ
    private List<Transform> originBottleTransform;
    //�����̰� returnTime��ŭ �������� üũ�ϴ� �ʵ� (������ �ڷ�ƾ�� ������Ʈ�Ǹ鼭 returnTime���� ��� ����)
    private bool[] isMoved;
    // Start is called before the first frame update
    void Start()
    {
        //������Ʈ�� �� ��ġ �� �ʱ�ȭ
        originBottleTransform = new List<Transform>();
        bottles = GameObject.FindGameObjectsWithTag("Bottle");
        isMoved = new bool[bottles.Length];
        for (int i = 0; i < bottles.Length; i++)
        {
            //���� ���縦 ���� �� ��ü ���� �־���
            Transform bTemp = new GameObject().GetComponent<Transform>();
            bTemp.position = bottles[i].transform.position;
            bTemp.rotation = bottles[i].transform.rotation;
            originBottleTransform.Add(bTemp);

            //�ʱ�ȭ
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
            //�� ��ġ�� �ٸ��� �ش� ��ü�� �������� ������ ��� ���� �ƴ� ��� 
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
            //3�ʰ� ������ �� ��� ���� ��� �ڷ�ƾ Ż��
            if (bottles[idx].GetComponent<OVRGrabbable>().isGrabbed)
            {
                isMoved[idx] = false;
                yield break;
            }
            delta += Time.deltaTime;
            yield return null;
        }
        //returnTime��ŭ ������
        if (delta >= returnTime)
        {
            bottles[idx].transform.position = originBottleTransform[idx].position;
            bottles[idx].transform.rotation = originBottleTransform[idx].rotation;
            isMoved[idx] = false;
        }
    }
}
