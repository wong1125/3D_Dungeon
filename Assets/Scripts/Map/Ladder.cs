using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour, IInvestigatable
{
    public bool CanInteract { get; set; } = false;

    public string GetDataString()
    {
        string str = "[��ٸ�]\n[�����̽� ��]�� ��Ÿ�ؼ� �ö� �� �ֽ��ϴ�.";
        return str;
    }

    public void InteractReaction()
    {
        return;
    }
}
