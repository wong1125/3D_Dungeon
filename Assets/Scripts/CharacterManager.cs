using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : SingletonWithMono<CharacterManager>
{
    public Player Player { get; private set; }

    public void PlayerSet(Player player)
    {
        Player = player;
    }

}
