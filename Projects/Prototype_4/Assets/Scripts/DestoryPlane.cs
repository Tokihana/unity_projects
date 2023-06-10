using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryPlane : MonoBehaviour
{
    public GameObject spawnManager;
    public GameObject audioPlayer;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // this method is called when a Prefab collide with plane
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            // Debug.Log("Destory Enemy");
            Destroy(other.gameObject);
            spawnManager.GetComponent<SpawnManager>().SubSpawnCount(1);
        }
        if(other.gameObject.CompareTag("Player"))
        {
            audioPlayer.GetComponent<PlayAudio>().WaveNegative();
            Destroy(other.gameObject);
            spawnManager.GetComponent <SpawnManager>().SubSpawnCount(-10);
        }
    }
}