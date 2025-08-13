using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    //인스펙터에서 이동방향 설정 가능
    [SerializeField] Vector3 direction = Vector3.forward;
    [SerializeField] float moveDistance = 5f;
    [SerializeField] float duration = 5f;
    
    private Rigidbody rb;
    private WaitForSeconds waitDurationSeconds;
    private Vector3 previusPos;
    private Vector3 currentPos;
    private Vector3 velocity;

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

    private void FixedUpdate()
    {
        //속도 구하기(rb.Velocity를 kinematic에 사용할 수 없기 때문)
        currentPos = rb.position;
        velocity = (currentPos - previusPos) / Time.fixedDeltaTime;
        previusPos = currentPos;
    }


    IEnumerator moveFowardBack()
    {
        while (true)
        {
            StartCoroutine(LerpMove(transform.position + direction * moveDistance, 5f));
            yield return waitDurationSeconds;
            StartCoroutine(LerpMove(transform.position + -direction * moveDistance, 5f));
            yield return waitDurationSeconds;
        }
    }


    //시간 기반 lerp로 MovePosition을 이용한 움직임 구현
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

    //접촉시 플랫폼과 같이 움직이기
    private void OnCollisionStay(Collision collision)
    {
        collision.rigidbody.MovePosition(collision.rigidbody.position + velocity * Time.fixedDeltaTime);
    }
}
