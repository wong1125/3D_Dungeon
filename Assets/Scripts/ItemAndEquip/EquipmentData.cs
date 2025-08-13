using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Equipment Data", menuName = "New Eqipment")]
public class EquipmentData : ScriptableObject
{
    public string equipmentName;
    public string description;
    public GameObject equipmentPrefab;
    public Color color;
    public EffectBase effect;
}

