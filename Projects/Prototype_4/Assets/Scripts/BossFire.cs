using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFire : MonoBehaviour
{
    // Fire status
    private float fireInterval = 1f;
    private int fireTimes = 20;

    // fireup indicator
    public GameObject fireupIndicator;

    // bullet Prefab
    public GameObject bulletPrefab;

    // player
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        // set indicator active
        fireupIndicator.SetActive(true);
        // start fire coroutine
        StartCoroutine(FireCoolDown());
    }

    // Update is called once per frame
    void Update()
    {
        fireupIndicator.transform.position = transform.position + new Vector3(0, -0.3f, 0);
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
        for (int i = 0; i < fireTimes; i++)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            GameObject bullet = Instantiate(bulletPrefab, transform.position + 2 * direction, Quaternion.Euler(direction));
            bullet.GetComponent<BossBullet>().SetEnemy(player);

            // fire through an interval
            yield return new WaitForSeconds(fireInterval);

        }
        // set indicator inactive
        fireupIndicator.SetActive(false);
    }
}
