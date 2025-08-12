using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInvestigatable
{
    string GetDataString();

    bool CanInteract();

    void InteractReaction();
 
}


public class Item : MonoBehaviour, IInvestigatable
{
    private Collider col;
    private MeshRenderer mr;
    
    private void Awake()
    {
        col = GetComponent<Collider>();
        mr = GetComponent<MeshRenderer>();
    }


    [SerializeField] ItemData data;

    public string GetDataString()
    {
        string str = $"[{data.itemName}]\n{data.description}";
        return str;
    }

    public bool CanInteract()
    {
        return false;
    }

    public void InteractReaction()
    {
        return;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (data.effect != null)
            {
                StartCoroutine(itemTimer());
            }
            else
                Debug.Log("������ ȿ���� �����ϴ�!");
        }
    }

    IEnumerator itemTimer()
    {
        data.effect.DoItemEffect(true);
        col.enabled = false;
        mr.enabled = false;
        yield return new WaitForSeconds(data.duration);
        data.effect.DoItemEffect(false);
        Destroy(this.gameObject);
    }
}
