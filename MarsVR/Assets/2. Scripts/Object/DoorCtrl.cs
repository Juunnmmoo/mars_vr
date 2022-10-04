using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCtrl : MonoBehaviour
{
    public bool isOpened = false;
    private Vector3 originRot;
    private bool isPlaying;


    void Start()
    {
        originRot = transform.rotation.eulerAngles;
    }

    [ContextMenu("Open")]
    public void OpenDoor()
    {
        if (!isPlaying && !isOpened)
            StartCoroutine(OpenDoorCor());
    }

    [ContextMenu("Close")]
    public void CloseDoor()
    {
        if (!isPlaying && isOpened)
        {
            StartCoroutine(CloseDoorCor());
        }
    }

    IEnumerator OpenDoorCor()
    {
        isPlaying = true;
        Vector3 rot = originRot;
        rot.x += 90 % 360;
        float i = originRot.x;
        while (i <= rot.x)
        {
            i += (Time.deltaTime / 0.5f) * (rot.x - originRot.x);                                       //0.5????? ???? ??
            transform.localRotation = Quaternion.Euler(i, originRot.y, originRot.z);
            yield return null;
        }
        transform.localRotation = Quaternion.Euler(rot);
        isOpened = true;
        isPlaying = false;
    }

    IEnumerator CloseDoorCor()
    {
        isPlaying = true;
        Vector3 rot = originRot;
        rot.x += 90 % 360;
        float i = rot.x;
        while (i >= originRot.x)
        {
            i -= (Time.deltaTime / 0.5f) * (rot.x - originRot.x);
            transform.localRotation = Quaternion.Euler(i, originRot.y, originRot.z);
            yield return null;
        }
        transform.localRotation = Quaternion.Euler(originRot);
        isOpened = false;
        isPlaying = false;
    }
}