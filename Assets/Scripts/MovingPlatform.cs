using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] float moveDistance = 5f;
    [SerializeField] float duration = 5f;
    
    private Rigidbody rb;
    private WaitForSeconds waitDurationSeconds;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        waitDurationSeconds = new WaitForSeconds(duration + 1f);
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(moveFowardBack());
    }


    IEnumerator moveFowardBack()
    {
        while (true)
        {
            StartCoroutine(LerpMove(transform.position + Vector3.forward * moveDistance, 5f));
            yield return waitDurationSeconds;
            StartCoroutine(LerpMove(transform.position + Vector3.back * moveDistance, 5f));
            yield return waitDurationSeconds;
        }
    }


    //lerp로 MovePosition을 이용한 움직임 구현
    IEnumerator LerpMove(Vector3 targetPos, float duration)
    {
        Vector3 startPos = transform.position;
        float currentTime = 0;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float lerpRatio = (currentTime / duration);
            rb.MovePosition(Vector3.Lerp(startPos, targetPos, lerpRatio));
            yield return null;
        }
        rb.MovePosition(targetPos);
    }
}
