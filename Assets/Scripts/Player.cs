using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    
    private void Awake()
    {
        CharacterManager.Instance.PlayerSet(this);
        controller = GetComponent<PlayerController>();
    }
}
