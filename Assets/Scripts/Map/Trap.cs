using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [Header("Trap Parameter")]
    [SerializeField] float power;
    [SerializeField] float lazerLength;
    [SerializeField] LayerMask playerMask;

    [SerializeField] Transform lazer;
    [SerializeField] Rigidbody projectileRb;

    private Vector3 fowardDirection;
    private float checkRate = 0.1f;
    private float lastTimeChecked;
    private bool active = true;

    private void Start()
    {
        fowardDirection = transform.rotation * Vector3.forward;
        lazer.transform.localScale = new Vector3(0.01f, lazerLength - 0.5f, 0.01f);
    }

    private void Update()
    {
        if (Time.time - lastTimeChecked > checkRate && active)
        {
            lastTimeChecked = Time.time;
            CheckPlayer();
        }

    }

    void CheckPlayer()
    {
        Ray ray = new Ray(transform.position, fowardDirection);

        if (Physics.Raycast(ray, lazerLength * 2, playerMask))
        {
            projectileRb.AddForce(fowardDirection * power, ForceMode.Impulse);
            StartCoroutine(ResetTrap());
        }
    }

    IEnumerator ResetTrap()
    {
        active = false;
        yield return new WaitForSeconds(4f);
        projectileRb.velocity = Vector3.zero;
        projectileRb.angularVelocity = Vector3.zero;
        projectileRb.rotation = Quaternion.identity;
        projectileRb.MovePosition(this.transform.position);
        active = true;
    }

    //플레이어 컨트롤을 잠시 비활성화하지 않으면, 힘을 받아도 바로 초기화됨.
    IEnumerator TurnOffControllBriefly()
    {
        CharacterManager.Instance.Player.controller.ControllerSwitch();
        yield return new WaitForSeconds(2f);
        CharacterManager.Instance.Player.controller.ControllerSwitch();
    }

    //콜라이더가 자식 오브젝트에게 있어서, 자식의 OnCollsionEnter에서 호출됨.
    public void ChildOnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(TurnOffControllBriefly());
            collision.gameObject.GetComponent<Rigidbody>().AddForce(fowardDirection * (power / 10) , ForceMode.Impulse);

        }
    }

}
