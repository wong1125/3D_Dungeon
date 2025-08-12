using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] float launchPower = 10f;
    private Rigidbody playerRb;
    private PlayerController playerController;

    private void Start()
    {
        playerController = CharacterManager.Instance.Player.controller;
        //�׽�Ʈ��
        playerRb = CharacterManager.Instance.Player.GetComponent<Rigidbody>();
    }

    private void Update()
    {

    }

    void LaunchPlayer()
    {
        if (playerRb != null)
        {
            Quaternion q = Quaternion.Euler(new Vector3(30, playerRb.rotation.eulerAngles.y, 0));
            Vector3 directionVector = q * Vector3.forward;
            StartCoroutine(TurnOffControllBriefly());
            playerRb.AddForce(directionVector * launchPower * 10f, ForceMode.Impulse);
        }
        else
            Debug.Log("Player�� ã�� �� �����ϴ�.");
    }

    private void OnCollisionEnter(Collision collision)
    {
        playerRb = collision.gameObject.GetComponent<Rigidbody>();
        LaunchPlayer();
    }

    IEnumerator TurnOffControllBriefly()
    {
        playerController.ControllerSwitch();
        yield return new WaitForSeconds(3f);
        playerController.ControllerSwitch();
    }

}
