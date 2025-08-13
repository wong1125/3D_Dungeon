using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour, IInvestigatable
{
    public bool CanInteract { get; set; } = false;

    public string GetDataString()
    {
        string str = "[사다리]\n[스페이스 바]를 연타해서 올라갈 수 있습니다.";
        return str;
    }

    public void InteractReaction()
    {
        return;
    }
}
