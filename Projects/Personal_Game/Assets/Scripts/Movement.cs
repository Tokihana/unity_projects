using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed, Space.Self);
    }

    // This method is called to set the speed and the direction
    public void SetMovement(ref Vector3 d, float s)
    {
        speed = s;
    }

    
}
