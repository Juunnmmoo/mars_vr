using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenCtrl : EvaluateManager
{
    public DoorCtrl door;
    public Transform ovenPos;
    public bool isContainedCup;

    void Start()
    {
        door = transform.Find("Door").GetComponent<DoorCtrl>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerCtrl>();
        endUI.SetActive(false);
        StartCoroutine(LoadingCor());
        StartCoroutine(PlayTimeCor());
    }

    private void Update()
    {
        if(!door.isOpened && isContainedCup)
        {
            isHolding = true;
        }
        else
        {
            isHolding = false;
        }

        if (isContainedCup)
        {
            if(cup.GetComponent<OVRGrabbable>().isGrabbed)
                isContainedCup = false;
            else
            {
                cup.transform.position = ovenPos.position;
                cup.transform.rotation = ovenPos.rotation;
            }
            
        }
    }


    new protected void OnTriggerEnter(Collider other)
    {
        if (door.isOpened && other.gameObject.CompareTag("Cup"))
        {
            if (other.gameObject.GetComponent<Rigidbody>() is Rigidbody)
            {
                other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                cup = other.gameObject.GetComponent<CupCtrl>();
                isContainedCup = true;
            }
        }
    }

    new protected void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Cup"))
        {
            cup = null;
            isContainedCup = false;
            isHolding = false;
        }
    }
}
