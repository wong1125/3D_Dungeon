using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIState 
{ 
    Idle,
    Following
}


public class Enemy : MonoBehaviour
{
    private AIState aistate;

    [SerializeField] float playerFollowDistance;
    private float distancePlayer;
    
    // Start is called before the first frame update
    void Start()
    {
        aistate = AIState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        distancePlayer = (transform.position - CharacterManager.Instance.Player.transform.position).sqrMagnitude;
        if (distancePlayer < playerFollowDistance)
        {
            aistate = AIState.Following;
        }
        else 
            aistate = AIState.Idle;
    }
}
