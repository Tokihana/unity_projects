using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Destory : MonoBehaviour
{
    private float xRange = 15f;
    private float zRange = 20f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckBounding();
    }

    // This method is called to check whether the gameObject should be destoried or not
    void CheckBounding()
    {
        if (transform.position.z < -zRange || transform.position.z > zRange ||
            transform.position.x < -xRange || transform.position.x > xRange)
        {
            // Debug.Log("Object " + name + " out of bound");
            Destroy(gameObject);
        }
    }
}
