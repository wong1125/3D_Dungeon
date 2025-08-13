using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConditionBar : MonoBehaviour
{
    private float initialCondition = 100;
    private float maxContidion = 100;
    private float currentCondtion = 100;

    [SerializeField] Image uiBar;
    
    // Update is called once per frame
    void Update()
    {
        uiBar.fillAmount = GetImageRatio();
    }


    float GetImageRatio()
    {

        return currentCondtion / maxContidion;
    }

    public void ChangeCondition(float value)
    {
        currentCondtion += value;
        if (currentCondtion >= maxContidion)
            currentCondtion = maxContidion;
        else if (currentCondtion <= 0)
            currentCondtion = 0;
    }

    public void SyncCondtion(float init, float max)
    {
        initialCondition = init;
        maxContidion = max;
        currentCondtion = initialCondition;
    }
}
