using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVR;


public class PlayerCtrl : MonoBehaviour
{
    public OVRGrabber lGrabber;
    public OVRGrabber rGrabber;

    public GameObject lGrabbedObject;
    public GameObject rGrabbedObject;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 1.5f, -6);

    }

    // Update is called once per frame
    void Update()
    {
        ControllerAction();
    }

    public void ControllerAction()
    {
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
            //¿ÞÂÊ ÁÂÅ¬¸¯
            if(lGrabber.GetGrabbedObj() != null)
            {
                lGrabbedObject = lGrabber.GetGrabbedObj();
                if (lGrabbedObject.CompareTag("Bottle"))
                {
                    lGrabbedObject.GetComponent<BottleCtrl>().isUsing = true;
                }
            }
        }
        if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
        {
            if(lGrabbedObject != null)
            {
                if (lGrabbedObject.CompareTag("Bottle"))
                {
                    lGrabbedObject.GetComponent<BottleCtrl>().isUsing = false;
                }
                lGrabbedObject = null;
            }
        }

        if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            //¿À¸¥ÂÊ ÁÂÅ¬¸¯
            if (rGrabber.GetGrabbedObj() != null)
            {
                rGrabbedObject = rGrabber.GetGrabbedObj();
                if (rGrabbedObject.CompareTag("Bottle"))
                {
                    Debug.LogWarning(rGrabbedObject.name);
                    rGrabbedObject.GetComponent<BottleCtrl>().isUsing = true;
                }
            }
        }
        if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
        {
            if(rGrabbedObject != null)
            {
                if (rGrabbedObject.CompareTag("Bottle"))
                {
                    rGrabbedObject.GetComponent<BottleCtrl>().isUsing = false;
                }
                rGrabbedObject = null;
            }
        }
    }

    
}
