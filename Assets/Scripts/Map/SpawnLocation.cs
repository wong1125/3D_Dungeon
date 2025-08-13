using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLocation : MonoBehaviour, IInvestigatable
{
    [SerializeField] string objName;
    [SerializeField] string destription;

    public bool CanInteract { get; set; } = false;

    public string GetDataString()
    {
        string str = $"[{objName}]\n{destription}";
        return str;
    }

    public void InteractReaction()
    {
        return;
    }
}
