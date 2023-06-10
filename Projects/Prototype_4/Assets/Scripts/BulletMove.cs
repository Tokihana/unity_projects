using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    // target enemy
    public GameObject targetEnemy;

    // move status
    private float moveSpeed = 15f;
    private float bulletForce = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if target enemy is destroyed, change target
        // Debug.Log(targetEnemy);
        if (targetEnemy == null)
        {
            GameObject[] currentEnemys = GameObject.FindGameObjectsWithTag("Enemy");
            if(currentEnemys.Length > 0) { targetEnemy = currentEnemys[0]; }
            else { Destroy(gameObject); }
        }
        else
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
        if(other.CompareTag("Enemy"))
        {
            // Debug.Log("Hit Enemy");
            Rigidbody enemyRb = other.GetComponent<Rigidbody>();
            Vector3 direction = (other.transform.position - transform.position).normalized;
            enemyRb.AddForce(direction * bulletForce, ForceMode.Impulse);
            Destroy(gameObject);
        }
    }
}
