using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI interactionText;
    [SerializeField] TextMeshProUGUI informationText;
    [SerializeField] TextMeshProUGUI clearText;

    private void Awake()
    {
        UIManager.Instance.InformationText = informationText;
        UIManager.Instance.InteractionText = interactionText;
        UIManager.Instance.clearText = clearText;
    }
}
