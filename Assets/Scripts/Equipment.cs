using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Equipment : MonoBehaviour, IInvestigatable
{
    PlayerEquip playerEquip;
    [SerializeField] EquipmentData data;

    public bool CanInteract { get; set; } = true;

    private void Start()
    {
        playerEquip = CharacterManager.Instance.Player.equip;
    }

    public string GetDataString()
    {
        string str = $"[¿Â∫Ò: {data.equipmentName}]\n{data.description}";
        return str;
    }

    public void InteractReaction()
    {
        if (playerEquip.currentEquipmentData != null)
        {
            playerEquip.UnEquip();
        }

        data.effect.DoItemEffect(true);
        playerEquip.currentEquipmentData = this.data;
        playerEquip.ring.SetActive(true);
        var renderers = playerEquip.ring.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            Material material = renderers[i].material;
            material.color = data.color;
        }
        Destroy(this.gameObject);
    }
}
