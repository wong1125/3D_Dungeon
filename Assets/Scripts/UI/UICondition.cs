using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class UICondition : MonoBehaviour
{
    public ConditionBar healthBar;
    public ConditionBar staminaBar;
    PlayerCondition playerCondition;
    
    private void Awake()
    {
        UIManager.Instance.UICondition = this;
    }

    private void Start()
    {
        playerCondition = CharacterManager.Instance.Player.condition;
        if (playerCondition == null )
            Debug.LogWarning("PlayerCondition이 null 입니다");
        healthBar.SyncCondtion(playerCondition.InitialHealth, playerCondition.MaxHealth);
        staminaBar.SyncCondtion(playerCondition.InitialStamina, playerCondition.MaxStamina);
        playerCondition.HealthChanger += healthBar.ChangeCondition;
        playerCondition.StaminaChanger += staminaBar.ChangeCondition;
    }
}
