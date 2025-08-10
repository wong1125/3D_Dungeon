using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConditionBar : MonoBehaviour
{
    public float initialCondition;
    public float maxContidion;
    private float currnetCondtion;

    
    [SerializeField] Image uiBar;
    
    // Start is called before the first frame update
    void Start()
    {
        currnetCondtion = initialCondition;
    }

    // Update is called once per frame
    void Update()
    {
        uiBar.fillAmount = GetImageRation();
    }


    float GetImageRation()
    {
        return currnetCondtion / maxContidion;
    }

    void ChangeCondition(float value)
    {
        currnetCondtion += value;
        if (currnetCondtion >= maxContidion)
            currnetCondtion = maxContidion;
        else if (currnetCondtion <= 0)
            currnetCondtion = 0;
    }
}
