using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireup : MonoBehaviour
{
    // Fire status
    private float fireInterval = 0.5f;
    private int fireTimes = 10;

    // fireup indicator
    public GameObject fireupIndicator;

    // bullet Prefab
    public GameObject bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // set indicator position follow players position
        fireupIndicator.transform.position = transform.position + new Vector3(0f, -0.4f, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("FireIcon"))
        {
            // Destory pickup
            Destroy(other.gameObject);
            // set indicator active
            fireupIndicator.SetActive(true);
            // start fire coroutine
            StartCoroutine(FireCoolDown());
        }
    }

    IEnumerator FireCoolDown()
    {
        for(int i = 0; i < fireTimes; i++)
        {
            // Find all enemys in scene
            GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
            // Debug.Log(enemys.Length + " enemy in scene");
            // fire bullet to each enemy
            for(int j = 0; j < enemys.Length; j++)
            {
                // Debug.Log(i + "th fire to enemy" + j);
                Vector3 direction = (enemys[j].transform.position - transform.position).normalized;
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(direction));
                bullet.GetComponent<BulletMove>().SetEnemy(enemys[j]);
            }
            // fire through an interval
            yield return new WaitForSeconds(fireInterval);
            
        }
        // set indicator inactive
        fireupIndicator.SetActive(false);
    }
}
