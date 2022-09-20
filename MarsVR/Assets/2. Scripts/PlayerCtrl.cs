using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVR;
using OculusSampleFramework;

public class PlayerCtrl : MonoBehaviour
{
    public float score;
    //왼손, 오른손 그래버
    public OVRGrabber lGrabber;
    public OVRGrabber rGrabber;

    //각 손의 집고 있는 오브젝트
    public GameObject lGrabbedObject;
    public GameObject rGrabbedObject;

    [Header("키보드 디버깅 관련")]
    public bool isDebug;
    [Range(0.1f, 0.3f)]
    public float keyboardMoveSpeed;
    public bool isHolding;
    [Range(0.5f, 1f)]
    public float keyboardRotationSpeed;
    private float xRot, zRot;
    private bool isRotate = false;
    private Vector3 InitialPos;
    private OVRGrabber nowControl;

    // Start is called before the first frame update
    void Start()
    {
        score = 100;
        InitialPos = new Vector3(0, 1.5f, -6); 
        nowControl = rGrabber;
        transform.position = InitialPos;
        lGrabber.transform.position = InitialPos;
        rGrabber.transform.position = InitialPos;
    }

    // Update is called once per frame
    void Update()
    {
        ControllerAction();
        if(isDebug)
            KeyboardAction();
    }

    private void ControllerAction()
    {
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
            //왼쪽 좌클릭
            if(lGrabber.GetGrabbedObj() != null)
            {
                lGrabbedObject = lGrabber.GetGrabbedObj();
            }
        }
        if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
        {
            if(lGrabbedObject != null)
            {
                lGrabbedObject = null;
            }
        }

        if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            //오른쪽 좌클릭
            if (rGrabber.GetGrabbedObj() != null)
            {
                rGrabbedObject = rGrabber.GetGrabbedObj();
                //Debug.LogWarning(((
                //    rGrabbedObject.GetComponent<Rigidbody>().velocity.x + 
                //    rGrabbedObject.GetComponent<Rigidbody>().velocity.y + 
                //    rGrabbedObject.GetComponent<Rigidbody>().velocity.z) / 3f) > 0.5f);
            }
        }
        if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
        {
            if(rGrabbedObject != null)
            {
                rGrabbedObject = null;
            }
        }
    }

    private void KeyboardAction()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            isRotate = false;
            nowControl = rGrabber;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            isRotate = false;
            nowControl = lGrabber;
        }
        if (Input.GetKeyDown(KeyCode.Q))
            isRotate = !isRotate;
        if (Input.GetKeyDown(KeyCode.E))
            isHolding = !isHolding;

        float xSpeed = Input.GetAxisRaw("Horizontal");
        float zSpeed = Input.GetAxisRaw("Vertical");
        if (!isRotate)
        {
            if(nowControl == rGrabber)
                nowControl.transform.Translate(new Vector3(0, xSpeed, zSpeed) * Time.deltaTime * keyboardMoveSpeed);
            else
                nowControl.transform.Translate(new Vector3(0, -xSpeed, zSpeed) * Time.deltaTime * keyboardMoveSpeed);
        }
        else
        {
            xRot -= xSpeed * keyboardRotationSpeed;
            zRot -= zSpeed * keyboardRotationSpeed;
            nowControl.transform.localRotation = Quaternion.Euler(new Vector3(nowControl.transform.localRotation.x + xRot, nowControl.transform.localRotation.z + zRot, 0));
        }

        if (isHolding)
        {
            if(nowControl.GetGrabbedObj() != null)
            {
                if (nowControl == rGrabber)
                {
                    if (rGrabbedObject == null && lGrabbedObject != nowControl.GetGrabbedObj())
                    {
                        rGrabbedObject = nowControl.GetGrabbedObj();
                        //rGrabbedObject.GetComponent<OVRGrabbable>().isGrabbed = true;
                    }
                    if(rGrabbedObject != null)
                    {
                        rGrabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                        rGrabbedObject.transform.position = nowControl.transform.position;
                        rGrabbedObject.transform.rotation = Quaternion.Euler(new Vector3(nowControl.transform.localRotation.z + zRot, -nowControl.transform.localRotation.x, 0));
                    }
                }
                if(nowControl == lGrabber)
                {
                    if (lGrabbedObject == null && rGrabbedObject != nowControl.GetGrabbedObj())
                    {
                        lGrabbedObject = nowControl.GetGrabbedObj();
                        //lGrabbedObject.GetComponent<OVRGrabbable>().isGrabbed = true;
                    }
                    if(lGrabbedObject != null)
                    {
                        lGrabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                        lGrabbedObject.transform.position = nowControl.transform.position;
                        lGrabbedObject.transform.localRotation = Quaternion.Euler(new Vector3(-nowControl.transform.localRotation.z - zRot, nowControl.transform.localRotation.x, 0));
                    }
                }
            }
        }
        else
        {
            if(nowControl == rGrabber)
            {
                if(rGrabbedObject != null)
                {
                    rGrabbedObject.GetComponent<Rigidbody>().isKinematic = false;
                   // rGrabbedObject.GetComponent<OVRGrabbable>().isGrabbed = false;
                    rGrabbedObject = null;
                }
                if(lGrabbedObject != null)
                {
                    lGrabbedObject.GetComponent<Rigidbody>().isKinematic = false;
                    //lGrabbedObject.GetComponent<OVRGrabbable>().isGrabbed = false;
                    lGrabbedObject = null;
                }
            }
        }
    }
}