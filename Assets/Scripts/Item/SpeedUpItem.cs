using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemEffectBase : ScriptableObject
{
    public abstract void DoItemEffect(bool itemOn);
}

[CreateAssetMenu(menuName = "ItemEffects/SpeedUp")]
public class SpeedUpEffect : ItemEffectBase
{
    public float deltaSpeed;
    
    public override void DoItemEffect(bool ItemOn)
    {
        float speed = ItemOn ? deltaSpeed : -deltaSpeed;
        CharacterManager.Instance.Player.controller.ChangeSpeed(speed);
    }

}
