using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVR;
using OculusSampleFramework;
using UnityEngine.SceneManagement;

public class PlayerCtrl : MonoBehaviour
{

    public float score;
    //�޼�, ������ �׷���
    public OVRGrabber lGrabber;
    public OVRGrabber rGrabber;

    //�� ���� ���� �ִ� ������Ʈ
    public GameObject lGrabbedObject;
    public GameObject rGrabbedObject;

    [Header("Ű���� ����� ����")]
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
    private Scene scene;

    // Start is called before the first frame update

    void Start()
    {
        scene = SceneManager.GetActiveScene();
        if (scene.name == "GameTitle")
        {
            InitialPos = new Vector3(297, 550, -2945);
        }
        else if (scene.name == "GameSelect")
        {
            InitialPos = new Vector3(353, 166, -229);
        }
        else if (scene.name == "Tutorial") {
            InitialPos = new Vector3(0, 1.5f, -6);
        }
        else if (scene.name == "bar")
        {
            // InitialPos = new Vector3(15.5f, 2.7f, -46.8f);
            InitialPos = new Vector3(16.0f, 2.7f, -46.8f);
        }
         else if (scene.name == "baker")
        {
            InitialPos = new Vector3(-0.057f, 1.38f, -1.74f);
        }
        else if (scene.name == "Score")
        {
            InitialPos = new Vector3(413, 170, -935);
        }

        score = 100;
         
        nowControl = rGrabber;
        transform.position = InitialPos;
        lGrabber.transform.position = InitialPos;
        rGrabber.transform.position = InitialPos;
    }

    // Update is called once per frame
    void Update()
    {
        ClickReceipt(rGrabber);
        ClickReceipt(lGrabber);
        //ControllerAction();
/*        if(isDebug)
            KeyboardAction();*/
    }

    private void ClickReceipt(OVRGrabber hand)
    {
        RaycastHit hit;
        Ray ray = new Ray(hand.transform.position, hand.transform.forward);
        Debug.Log(hand.GetController().ToString());
        if(Physics.Raycast(ray, out hit, 10f, 1 << 6))
        {
            if((OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) || OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
                && hit.transform.gameObject.CompareTag("Receipt"))
            {
                //받아올 때 이부분 수정
                switch (scene.name)
                {
                    case "Tutorial":
                        hit.transform.gameObject.GetComponent<ReceiptCtrl>().ShowReceipt("Tutorial");
                        break;
                    case "bar":
                        if (PlayerPrefs.GetInt("OncePlayed") == 0)
                            hit.transform.gameObject.GetComponent<ReceiptCtrl>().ShowReceipt("PeachTree");
                        else if (PlayerPrefs.GetInt("OncePlayed") == 1)
                            hit.transform.gameObject.GetComponent<ReceiptCtrl>().ShowReceipt("PinaColada");
                        break;
                }
            }
        }
    }

    private void ControllerAction()
    {
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
            //���� ��Ŭ��
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
            //������ ��Ŭ��
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