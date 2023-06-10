using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryOverTime : MonoBehaviour
{
    float durance = 2f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Counter());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // this coroutine is called to destory gameObject after a few seconds
    IEnumerator Counter()
    {
        yield return new WaitForSeconds(durance);
        Debug.Log("Destory " + name + " after " + durance + " seconds.");
        Destroy(gameObject);
    }
}
