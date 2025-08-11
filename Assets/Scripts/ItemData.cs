using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Item Data", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public string description;
    public float duration;
    public ItemEffect effect;
}
