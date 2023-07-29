using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    // target enemy
    public GameObject targetEnemy;

    // move status
    private float moveSpeed = 15f;
    private float bulletForce = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(targetEnemy != null)
        {
            // Use LookAt method to modify direction
            transform.LookAt(targetEnemy.transform);
            // move forward
            transform.Translate(transform.forward * moveSpeed * Time.deltaTime);
        }
    }
    public void SetEnemy(GameObject enemy)
    {
        targetEnemy = enemy;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Debug.Log("Hit Enemy");
            Rigidbody enemyRb = other.GetComponent<Rigidbody>();
            Vector3 direction = (other.transform.position - transform.position).normalized;
            enemyRb.AddForce(direction * bulletForce, ForceMode.Impulse);
            Destroy(gameObject);
        }
    }
}
