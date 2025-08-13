using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerEquip : MonoBehaviour
{
    public EquipmentData currentEquipmentData;
    public GameObject ring;

    //TabŰ�� ���� ��� ������
    public void UnEquip()
    {
        ring.SetActive(false);
        currentEquipmentData.effect.DoItemEffect(false);
        Instantiate(currentEquipmentData.equipmentPrefab, transform.position + Vector3.forward, Quaternion.identity);
        currentEquipmentData = null;
    }

    public void ThrowInputReceive(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started && currentEquipmentData != null)
        {
            UnEquip();
        }
    }

}
