using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InformationText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI interactionText;
    
    private void Awake()
    {
        UIManager.Instance.InformationText = this.GetComponent<TextMeshProUGUI>();
        UIManager.Instance.InteractionText = interactionText;
    }
}
