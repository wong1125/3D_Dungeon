using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapProjectile : MonoBehaviour, IInvestigatable
{
    Trap trap;

    [SerializeField] string objName;
    [SerializeField] string destription;

    private void Awake()
    {
        trap = GetComponentInParent<Trap>();
    }

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

    private void OnCollisionEnter(Collision collision)
    {
        trap.ChildOnCollisionEnter(collision);
    }

}
