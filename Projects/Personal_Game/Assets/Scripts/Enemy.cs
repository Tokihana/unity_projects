using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // enemy health
    private int health = 3;
    // enemy collide damage
    private int collideDamage = 1;

    // bool varible to prevent the enemy spawn two or more pickups
    // when it was hitted by multi bullets
    bool alive = true;

    // SpawnManager
    SpawnManager spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // make damage
    public void ChangeHealth(int damage)
    {
        health -= damage;
        // Debug.Log("Enemy health: " + health);
        if(health <= 0 && alive)
        {
            Destroy(gameObject);
            spawnManager.SpawnPowerup(transform.position);
            // Debug.Log("Enemy destroied!");
            alive = false;
        }
    }
    
    // return collide damage
    public int CollideDamage() { return collideDamage; }

    // handle collide with projectile
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Projectile"))
        {
            ChangeHealth(other.gameObject.GetComponent<Projectile>().Damage());
            Destroy(other.gameObject);
        }
    }
}
