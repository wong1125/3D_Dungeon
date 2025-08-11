using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Rigidbody rb;
    private readonly WaitForSeconds waitOneSecond = new WaitForSeconds(1f);

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(moveFowardBack());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator moveFowardBack()
    {
        while (true)
        {
            rb.velocity = Vector3.forward;
            yield return waitOneSecond;
            rb.velocity = Vector3.back;
            yield return waitOneSecond;
        }
    }
}
