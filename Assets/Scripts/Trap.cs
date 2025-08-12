using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [Header("Trap Parameter")]
    [SerializeField] float power = 5f;
    [SerializeField] float lazerLength = 2.5f;
    [SerializeField] LayerMask playerMask;

    [SerializeField] Transform lazer;
    [SerializeField] Rigidbody projectileRb;

    private float checkRate = 0.1f;
    private float lastTimeChecked;
    private bool active = true;

    private void Start()
    {
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
        Ray ray = new Ray(transform.position, Vector3.forward);

        if (Physics.Raycast(ray, lazerLength * 2, playerMask))
        {
            projectileRb.AddForce(Vector3.forward * power * 50, ForceMode.Impulse);
            StartCoroutine(ResetTrap());
        }
    }

    IEnumerator ResetTrap()
    {
        active = false;
        yield return new WaitForSeconds(4f);
        projectileRb.velocity = Vector3.zero;
        projectileRb.rotation = Quaternion.identity;
        projectileRb.MovePosition(this.transform.position);
        active = true;
    }



}
