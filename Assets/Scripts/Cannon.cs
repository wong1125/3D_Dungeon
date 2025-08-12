using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour, IInvestigatable
{
    [SerializeField] float launchPower = 10f;
    private Rigidbody playerRb;
    private PlayerController playerController;

    private void Start()
    {
        playerController = CharacterManager.Instance.Player.controller;
        playerRb = CharacterManager.Instance.Player.GetComponent<Rigidbody>();
    }


    IEnumerator LaunchPlayer()
    {
        playerRb.MovePosition(transform.position + Vector3.up * 0.25f);
        yield return new WaitForSeconds(3f);
        Quaternion q = Quaternion.Euler(new Vector3(20, playerRb.rotation.eulerAngles.y, 0));
        Vector3 directionVector = q * Vector3.forward;
        StartCoroutine(TurnOffControllBriefly());
        playerRb.AddForce(directionVector * launchPower * 10f, ForceMode.Impulse);
    }


    IEnumerator TurnOffControllBriefly()
    {
        playerController.ControllerSwitch();
        yield return new WaitForSeconds(3f);
        playerController.ControllerSwitch();
    }

    public string GetDataString()
    {
        string str = "[대포]\n원하는 방향으로 발사할 수 있습니다.";
        return str;
    }

    public bool CanInteract()
    {
        return true;
    }

    public void InteractReaction()
    {
        if (playerRb != null)
        {
            StartCoroutine(LaunchPlayer());
        }
        else
            Debug.Log("Player를 찾을 수 없습니다.");

    }
}
