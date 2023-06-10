using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // enemies prefabs
    public GameObject[] enemies;

    // pickup prefabs
    public GameObject powerup;


    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("RandomSpawn", 2f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Spawn an enemy at random place
    void RandomSpawn()
    {
        int enemyID = Random.Range(0, enemies.Length);
        Instantiate(enemies[enemyID], new Vector3(Random.Range(-15, 15), 0, Random.Range(0, 20)), 
            Quaternion.Euler(0,180,0));
    }

    // generate 
    public void SpawnPowerup(Vector3 position)
    {
        Instantiate(powerup, position, Quaternion.Euler(0, 180, 0));
    }
}
