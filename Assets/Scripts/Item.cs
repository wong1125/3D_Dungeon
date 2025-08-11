using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInvestigatable
{
    string GetDataString();
 
}



public class Item : MonoBehaviour, IInvestigatable
{

    [SerializeField] ItemData data;

    public string GetDataString()
    {
        string str = $"[{data.itemName}]\n{data.description}";
        return str;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
            Destroy(this.gameObject);
        }
    }
}
