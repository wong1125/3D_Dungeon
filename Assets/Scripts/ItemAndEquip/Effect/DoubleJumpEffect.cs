using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffects/DoubleJump")]
public class DoubleJumpEffect : EffectBase
{
    private int additonalJump = 1;

    public override void DoItemEffect(bool ItemOn)
    {
        int jumpSwitch = ItemOn ? 1 + additonalJump : 1;
        CharacterManager.Instance.Player.controller.MultipleJump(jumpSwitch);
    }
}
