using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerCondition condition;
    public PlayerInvestigation investigation;
    public PlayerEquip equip;
    
    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
        investigation = GetComponent<PlayerInvestigation>();
        equip = GetComponent<PlayerEquip>();
    }
}
