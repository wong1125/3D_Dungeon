using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConditionBar : MonoBehaviour
{
    private float initialCondition ;
    private float maxContidion;
    private float currentCondtion;

    [SerializeField] Image uiBar;
    
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

    //외부(플레이어) 정보 받아오기
    public void SyncCondtion(float init, float max)
    {
        initialCondition = init;
        maxContidion = max;
        currentCondtion = initialCondition;
    }
}
