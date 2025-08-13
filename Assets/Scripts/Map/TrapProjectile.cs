using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapProjectile : MonoBehaviour
{
    Trap trap;
    private void Awake()
    {
        trap = GetComponentInParent<Trap>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        trap.ChildOnCollisionEnter(collision);
    }
}
