using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject [] enemyPrefabs;
    public GameObject [] pickupPrefabs;
    public GameObject audioPlayer;
    private float spawnRange = 9.0f;
    private int spawnCount = 0;
    private int waveHard = 1;
    private bool nextwave = false;

    public GameObject bossPrefab;

    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemyWave(waveHard);
        SpawnPowerupWave(waveHard);
    }

    // Update is called once per frame
    void Update()
    {
        if(!nextwave && spawnCount == 0)
        {
            ++waveHard;
            nextwave = true;
            audioPlayer.GetComponent<PlayAudio>().WavePositive();
            StartCoroutine(WaveBreak());
        }
    }

    private Vector3 GenerateSpawnPosition()
    {
        float spawnX = Random.Range(-spawnRange, spawnRange);
        float spawnZ = Random.Range(-spawnRange, spawnRange);

        Vector3 spawnPosition = new Vector3(spawnX, 0, spawnZ);
        return spawnPosition;
    }

    void SpawnEnemyWave(int wave)
    {
        spawnCount = wave;
        for (int i = 0; i < wave; ++i)
        {
            int spawnId = Random.Range(0, enemyPrefabs.Length);
            Instantiate(enemyPrefabs[spawnId], GenerateSpawnPosition(), enemyPrefabs[spawnId].transform.rotation);
        }
    }

    public void SubSpawnCount(int count) {  spawnCount -= count; }

    void SpawnPowerupWave(int wave)
    {
        for(int i = 0; i < wave; ++i)
        {
            int spawnId = Random.Range(0, pickupPrefabs.Length);
            Instantiate(pickupPrefabs[spawnId], GenerateSpawnPosition(), pickupPrefabs[spawnId].transform.rotation);
        }
    }

    IEnumerator WaveBreak()
    {
        yield return new WaitForSeconds(3);
        if (waveHard % 5 == 0)
        {
            spawnCount++;
            Instantiate(bossPrefab, GenerateSpawnPosition(), bossPrefab.transform.rotation);
        }
        SpawnEnemyWave(waveHard);
        SpawnPowerupWave(waveHard);
        nextwave = false;
    }
}
