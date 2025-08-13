using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : SingletonWithMono<UIManager>
{
    public UICondition UICondition;
    public TextMeshProUGUI InformationText;
    public TextMeshProUGUI InteractionText;
    public TextMeshProUGUI clearText;
}
