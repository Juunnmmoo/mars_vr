using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleManager : MonoBehaviour
{

    private GameObject[] bottles;
    private List<Transform> originBottleTransform;
    private bool[] isMoved;
    // Start is called before the first frame update
    void Start()
    {
        originBottleTransform = new List<Transform>();
        bottles = GameObject.FindGameObjectsWithTag("Bottle");
        isMoved = new bool[bottles.Length];
        for (int i = 0; i < bottles.Length; i++)
        {
            Transform temp = new GameObject().GetComponent<Transform>();

            temp.position = bottles[i].transform.position;
            temp.rotation = bottles[i].transform.rotation;
            originBottleTransform.Add(temp);

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
            if (bottles[i].transform.position != originBottleTransform[i].position && !bottles[i].GetComponent<BottleCtrl>().isUsing && !isMoved[i])
            {
                isMoved[i] = true;
                StartCoroutine(InitialBottlePosCor(i));
            }
        }
    }

    IEnumerator InitialBottlePosCor(int idx)
    {
        float delta = 0;
        while (delta < 3f)
        {
            if (bottles[idx].GetComponent<BottleCtrl>().isUsing)
                yield break;
            delta += Time.deltaTime;
            yield return null;
        }
        if (delta >= 3f)
        {
            bottles[idx].transform.position = originBottleTransform[idx].position;
            bottles[idx].transform.rotation = originBottleTransform[idx].rotation;
            isMoved[idx] = false;
        }
    }
}
