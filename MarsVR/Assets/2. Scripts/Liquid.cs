using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liquid : MonoBehaviour
{
    private LineRenderer line = null;

    private Coroutine pourCor = null;
    private Vector3 targetPos = Vector3.zero;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        MoveToPos(0, transform.position);
        MoveToPos(1, transform.position);
    }

    public void Begin()
    {
        pourCor = StartCoroutine(BeginPour());
    }

    public void End()
    {
        StopCoroutine(pourCor);
        pourCor = StartCoroutine(EndPour());
    }



    private Vector3 FindEndPoint()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);
        Physics.Raycast(ray, out hit, 10.0f);
        return hit.collider ? hit.point : ray.GetPoint(10.0f);
    }

    private void MoveToPos(int idx, Vector3 targetPos)
    {
        line.SetPosition(idx, targetPos);
    }

    private void AnimateToPosition(int idx, Vector3 targetPos)
    {
        Vector3 curPos = line.GetPosition(idx);
        Vector3 newPos = Vector3.MoveTowards(curPos, targetPos, Time.deltaTime * 2.25f);
        line.SetPosition(idx, newPos);
    }

    private bool HasReachedPosition(int idx, Vector3 targetPos)
    {
        Vector3 curPos = line.GetPosition(idx);
        return curPos == targetPos;
    }
    private IEnumerator BeginPour()
    {
        while (gameObject.activeSelf)
        {
            targetPos = FindEndPoint();

            MoveToPos(0, transform.position);
            AnimateToPosition(1, targetPos);
            yield return null;
        }
    }

    private IEnumerator EndPour()
    {
        while(!HasReachedPosition(0, targetPos))
        {
            AnimateToPosition(0, targetPos);
            AnimateToPosition(1, targetPos);
            yield return null;
        }
        Debug.Log("!");
        Destroy(gameObject);
    }

    //private IEnumerator UpdateParticle()
    //{
    //    while (gameObject.activeSelf)
    //    {
    //        splashParticle.gameObject.transform.position = targetPos;

    //        bool isHitting = HasReachedPosition(1, targetPos);
    //        splashParticle.gameObject.SetActive(isHitting);
    //        yield return null;
    //    }
    //}
}
