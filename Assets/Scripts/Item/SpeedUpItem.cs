using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemEffectBase : ScriptableObject
{
    public abstract void DoItemEffect();
}

[CreateAssetMenu(menuName = "ItemEffects/SpeedUp")]
public class SpeedUpEffect : ItemEffectBase
{
    public float deltaSpeed;
    
    public override void DoItemEffect()
    {
        CharacterManager.Instance.Player.controller.ChangeSpeed(deltaSpeed);
    }
}
