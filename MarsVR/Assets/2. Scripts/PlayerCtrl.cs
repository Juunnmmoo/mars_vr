using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVR;


public class PlayerCtrl : MonoBehaviour
{
    public OVRGrabber lGrabber;
    public OVRGrabber rGrabber;
    private OVRGrabbable grabbedObj;
    // Start is called before the first frame update
    void Start()
    {
        //lGrabber = GetComponent<OVRGrabber>(); 
        //rGrabber = GetComponent<OVRGrabber>();
        transform.position = new Vector3(0, 1.5f, -6);

    }

    // Update is called once per frame
    void Update()
    {
        ControllerAction();
    }

    public void ControllerAction()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch))
        {
            //¿ÞÂÊ ÁÂÅ¬¸¯
                Debug.LogWarning(lGrabber.GetGrabbedObj().gameObject.name);

        }
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
        {
            //¿À¸¥ÂÊ ÁÂÅ¬¸¯
                Debug.LogWarning(rGrabber.GetGrabbedObj().gameObject.name);
        }
    }

    
}
