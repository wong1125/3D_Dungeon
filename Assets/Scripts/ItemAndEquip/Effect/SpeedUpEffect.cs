using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffectBase : ScriptableObject
{
    public abstract void DoItemEffect(bool itemOn);
}

[CreateAssetMenu(menuName = "ItemEffects/SpeedUp")]
public class SpeedUpEffect : EffectBase
{
    public float speedBoost;
    
    public override void DoItemEffect(bool ItemOn)
    {
        float speed = ItemOn ? speedBoost : -speedBoost;
        CharacterManager.Instance.Player.controller.ChangeSpeed(speed);
    }

}
