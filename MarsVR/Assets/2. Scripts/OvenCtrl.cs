using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenCtrl : EvaluateManager
{
    private DoorCtrl door;
    public Transform ovenPos;
    private CupCtrl cup;

    void Start()
    {
        door = transform.Find("Door").GetComponent<DoorCtrl>();
        StartCoroutine(LoadingCor());
        StartCoroutine(PlayTimeCor());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (door.isOpened && other.gameObject.CompareTag("Cup"))
        {
            if (other.gameObject.GetComponent<Rigidbody>() is Rigidbody)
            {
                other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                other.transform.position = ovenPos.position;
                other.transform.rotation = ovenPos.rotation;
                cup = other.gameObject.GetComponent<CupCtrl>();
            }
        }
    }
}
