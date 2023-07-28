using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarderEnemy : MonoBehaviour
{
    private float harderForce = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            // ��Ҫ�����Զ����ˣ�������Ϊ���ˣ��յ�Ϊ���
            Rigidbody playerRigdibody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 direction = (collision.gameObject.transform.position - transform.position).normalized;
            playerRigdibody.AddForce(direction * harderForce, ForceMode.Impulse);
        }
    }
}
