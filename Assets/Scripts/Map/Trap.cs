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

    IEnumerator TurnOffControllBriefly()
    {
        CharacterManager.Instance.Player.controller.ControllerSwitch();
        yield return new WaitForSeconds(2f);
        CharacterManager.Instance.Player.controller.ControllerSwitch();
    }

    public void ChildOnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(TurnOffControllBriefly());
            collision.gameObject.GetComponent<Rigidbody>().AddForce(fowardDirection * (power / 10) , ForceMode.Impulse);

        }
    }




}
