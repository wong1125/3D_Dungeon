using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour, IInvestigatable
{
    [SerializeField] private float jumpPadPower;
    [SerializeField] string objName;
    [SerializeField] string destription;

    public bool CanInteract { get; set; } = false;

    public string GetDataString()
    {
        string str = $"[{objName}]\n{destription}";
        return str;
    }

    public void InteractReaction()
    {
        return;
    }


    //player 접촉하면 player 점프
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.rigidbody.AddForce(Vector3.up * jumpPadPower, ForceMode.Impulse);
        }
    }
}
