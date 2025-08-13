using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ItemEffects/JumpUp")]
public class JumpUpEffect : EffectBase
{
    public float jumpBoost;

    public override void DoItemEffect(bool ItemOn)
    {
        float jump = ItemOn ? jumpBoost : -jumpBoost;
        CharacterManager.Instance.Player.controller.ChangeJumpPower(jump);
    }
}
