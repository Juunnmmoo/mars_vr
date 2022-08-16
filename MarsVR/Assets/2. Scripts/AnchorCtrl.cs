using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorCtrl : MonoBehaviour
{
    public Vector3 originPos;
    private GameObject player;
    private Coroutine currentCor = null;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        BeginAnchor();
    }

    private void Update()
    {

    }

    public void BeginAnchor()
    {
         currentCor = StartCoroutine(AnchorMove());
    }
    public void EndAnchor()
    {
        StopCoroutine(currentCor);
        Destroy(gameObject);
    }
    private IEnumerator AnchorMove()
    {
        while (gameObject.activeSelf)
        {
            Vector3 nowPos = (Vector3.up * Mathf.PingPong(Time.time * 0.05f, 0.1f));
            transform.position = originPos + nowPos;
            transform.LookAt(player.transform);
            yield return null;
        }
    }
}
