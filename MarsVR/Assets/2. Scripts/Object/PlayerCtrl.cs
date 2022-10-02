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

    private Vector3 initialPos;
    private OVRGrabber nowControl;
    private Scene scene;

    // Start is called before the first frame update

    void Start()
    {
        scene = SceneManager.GetActiveScene();
        switch (scene.name.ToUpper())
        {
            case "TUTORIAL":
                initialPos = new Vector3(0, 1.5f, -6);
                break;
            case "BARTENDER":
                initialPos = new Vector3(16.0f, 2.35f, -46.8f);
                break;
            case "BAKER":
                initialPos = new Vector3(0.2f, 1.4f, -1.4f);
                break;
            default:
                initialPos = Vector3.zero;
                break;
        }

        score = 100;
         
        nowControl = rGrabber;
        transform.position = initialPos;
    }

    // Update is called once per frame
    void Update()
    {
        ClickObject(rGrabber);
        ClickObject(lGrabber);
        //ControllerAction();
/*        if(isDebug)
            KeyboardAction();*/
    }

    private void ClickObject(OVRGrabber hand)
    {
        RaycastHit hit;
        Ray ray = new Ray(hand.transform.position, hand.transform.forward);
        if(Physics.Raycast(ray, out hit, 10f, 1 << 6))
        {
            if((OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) || OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
                && hit.transform.gameObject.CompareTag("Receipt"))
            {
                //받아올 때 이부분 수정
                switch (scene.name.ToUpper())
                {
                    case "TUTORIAL":
                        hit.transform.gameObject.GetComponent<ReceiptCtrl>().ShowReceipt("Tutorial");
                        break;
                    case "BARTENDER":
                        if (PlayerPrefs.GetInt("OncePlayed") == 0)
                            hit.transform.gameObject.GetComponent<ReceiptCtrl>().ShowReceipt("PeachTree");
                        else if (PlayerPrefs.GetInt("OncePlayed") == 1)
                            hit.transform.gameObject.GetComponent<ReceiptCtrl>().ShowReceipt("PinaColada");
                        break;
                    case "BAKER":
                        if (PlayerPrefs.GetInt("OncePlayed") == 0)
                            hit.transform.gameObject.GetComponent<ReceiptCtrl>().ShowReceipt("Baguette");
                        else if (PlayerPrefs.GetInt("OncePlayed") == 1)
                            hit.transform.gameObject.GetComponent<ReceiptCtrl>().ShowReceipt("Croissant");
                        break;
                }
            }
            else if ((OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) || OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
                && hit.transform.gameObject.CompareTag("Door"))
            {
                if (!hit.transform.gameObject.GetComponent<DoorCtrl>().isOpened)
                {
                    hit.transform.gameObject.GetComponent<DoorCtrl>().OpenDoor();
                }
                else
                {
                    hit.transform.gameObject.GetComponent<DoorCtrl>().CloseDoor();
                }
            }
        }
    }
}