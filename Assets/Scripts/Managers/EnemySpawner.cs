using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Wave
{
    [SerializeField] private string waveName;

    [SerializeField] private List<SpawnGroup> spawnGroups;
}

[Serializable]
public class SpawnGroup
{
    [SerializeField] private int enemyType;
    [SerializeField] private int amount;
}

public class EnemySpawner : MonoBehaviour
{
    [Header("Wave Settings")]
    [SerializeField] public Wave[] waves;

    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;
    //[SerializeField] private List<List<List<int>>> spawnList = new List<List<List<int>>>();

    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 8;//amount of enemies to spawn
    [SerializeField] private float enemiesPerSecond = 0.5f;//enemies spawning per second
    [SerializeField] private float timeBetweenWaves = 5f; //Time to prepare before next wave
    [SerializeField] private float difficultyScalingFactor = 0.75f; //doubles the enemies spawning per wave

    [Header("Events")]
    public static UnityEvent onEnemyDestroy  = new UnityEvent();

    private int currentWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesLeftToSpawn;
    private bool isSpawning = false;

    // [enum, amount]
    // waveArray1 = [[0, 2], [1, 2] , [0, 3], [1, 3]]
    
    private enum enemyTypes
    {
        Basic,
        Heavy
        //SimpleFlyer,
        //GunshipFlyer,
        //Speedyboi
    }

    private void Awake()
    {
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }

    private void Start()
    {
        StartCoroutine(StartWave());
    }

    void Update()
    {
        if (!isSpawning) return;

        timeSinceLastSpawn += Time.deltaTime;

        if(timeSinceLastSpawn >= 1f / enemiesPerSecond && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();
            enemiesLeftToSpawn--;
            enemiesAlive++;
            timeSinceLastSpawn = 0f;
        }

        if (enemiesAlive == 0 && enemiesLeftToSpawn == 0) {
            EndWave();
        }

    }


    private void SpawnEnemies(int currentWave)
    {
        if (currentWave == 1)
        {
            // spawn specific enemies

            // List<Lists<enemyType, enemyAmount>> spawnList; [["Basic", 3], ["Heavy", 2]] <- wave 1
            // List<Lists<enemyType, enemyAmount>> spawnList; [["Basic", 5], ["Heavy", 3]] <- wave 2


            // enemyType
            // enemyAmount
        }
        if (currentWave == 2)
        {
            // spawn specific enemies
        }
        if (currentWave == 3)
        {
            // spawn specific enemies
        }
        if (currentWave == 4)
        {
            // spawn specific enemies
        }
    }


    private void EnemyDestroyed()
    {
        enemiesAlive--;
    }

    private void SpawnEnemy()
    {
        GameObject prefabToSpawn = enemyPrefabs[0];
        Instantiate(prefabToSpawn, LevelManage.main.startPoint.position, Quaternion.identity);
    }
    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        isSpawning = true;
        enemiesLeftToSpawn = EnemiesPerWave();
    }

    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor));
    }

    private void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++;
        StartCoroutine(StartWave());
    }
}
