using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Wave
{
    [SerializeField] public string WaveNumber;

    private int totalEnemies;

    [SerializeField] public List<SpawnGroup> spawnGroups;

    public int GetTotalEnemies()
    {
        totalEnemies = 0;

        for (int i = 0; i < spawnGroups.Count; i++)
        {
            for (int j = 0; j < spawnGroups[i].pathGroups.Count; j++)
            {
                for (int k = 0; k < spawnGroups[i].pathGroups[j].pathUnits.Count; k++)
                {
                    totalEnemies += spawnGroups[i].pathGroups[j].pathUnits[k].amount;
                }
            }
        }

        return totalEnemies;
    }

    public int GetSpawnGroupCount() => spawnGroups.Count;
}

[Serializable]
public class SpawnGroup
{
    private int spawnGroupNumber;
    [SerializeField] public float spawnGroupDelay;
    [SerializeField] public List<PathGroup> pathGroups;

    public int GetPathGroupCount() => pathGroups.Count;
}

[Serializable]
public class PathGroup
{
    [SerializeField] public int pathNumber;
    [SerializeField] public float pathGroupDelay = 0.5f;
    [SerializeField] public List<PathUnit> pathUnits;

    public int GetPathUnitCount() => pathUnits.Count;
}

[Serializable]
public class PathUnit
{
    private int pathUnitNumber;
    [SerializeField] public int enemyType;
    [SerializeField] public int amount;
    // null = 0
    // basic ground enemy = 1
    // speedy ground enemy = 2
    // heavy ground enemy = 3
    // basic flying enemy = 4
    // heavy flying enemy = 5
}

public class EnemySpawner : MonoBehaviour
{
    [Header("Wave Settings")]
    [SerializeField] public Wave[] waves;

    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] public PathStats pathStats;
    
    [Header("Attributes")]
    [SerializeField] private float enemiesPerSecond = 0.5f; //enemies spawning per second
    [SerializeField] private float timeBetweenWaves = 5f; //Time to prepare before next wave

    [Header("Events")]
    public static UnityEvent onEnemyDestroy  = new UnityEvent(); 
    
    [Header("[DEBUG]")]
    [SerializeField] private Path[] paths;
    [SerializeField] private int currentWave = 1;
    [SerializeField] private int currentSpawnGroup = 0;
    [SerializeField] private int currentSpawnAmount = 0;
    [SerializeField] private float timeSinceLastSpawn;
    [SerializeField] private int enemiesAlive;
    [SerializeField] private int enemiesLeftToSpawn;
    [SerializeField] private bool isSpawning = false;

    private void Awake()
    {
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }

    private void Start()
    {
        paths = pathStats.GetPaths();
        StartCoroutine(StartWave());
    }

    void Update()
    {
        if (!isSpawning) return;

        timeSinceLastSpawn += Time.deltaTime;

        if(timeSinceLastSpawn >= 1f / enemiesPerSecond && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();
            enemiesLeftToSpawn--; // gonna need to place this inside the function and make it scale with the number of enemies (consider nulls!)
            enemiesAlive++; // gonna need to place this inside the function and make it scale with the number of enemies (consider nulls!)
            timeSinceLastSpawn = 0f;
        }

        if (enemiesAlive == 0 && enemiesLeftToSpawn == 0) {
            EndWave();
        }
    }

    private void SpawnEnemy()
    {
        Wave _currentWave = waves[currentWave - 1];

        int _amount = 0;//_currentWave.spawnGroups[currentSpawnGroup].amount;
        int _enemyType = 0;//_currentWave.spawnGroups[currentSpawnGroup].enemyType;
        int _spawnPoint = UnityEngine.Random.Range(0, paths.Length-1);

        if (_enemyType != 0)
        {
            InstantiateEnemy(_enemyType, paths[_spawnPoint].pointList[0]);
        }

        currentSpawnAmount++;

        if (currentSpawnAmount > _amount)
        {
            currentSpawnAmount = 0;
            currentSpawnGroup++;
        }
    }

    private void InstantiateEnemy(int _enemyType, Transform _transform)
    {
        GameObject prefabToSpawn = enemyPrefabs[_enemyType];

        Instantiate(prefabToSpawn, _transform.position, Quaternion.identity);
    }

    private void EnemyDestroyed()
    {
        enemiesAlive--;
    }

    private void InitializeSpawnGroup()
    {
        // iterate over pathgroups based on spawngroup
        // startpathgroup coroutine
        //check if all coroutines are done
        // wait for spawngroupdelay
        // start next spawngroup
    }

    private IEnumerator StartPathGroup(PathGroup _pathGroup)
    {
        for (int i = 0; i < _pathGroup.GetPathUnitCount(); i++)
        {
            for (int j = 0; j < _pathGroup.pathUnits.Count; j++)
            {
                int _enemyType = _pathGroup.pathUnits[j].enemyType;
                int _amount = _pathGroup.pathUnits[j].amount;
                int _pathNumber = _pathGroup.pathNumber;

                for (int k = 0; k < _amount; k++)
                {
                    InstantiateEnemy(_enemyType, paths[_pathNumber].pointList[0]);
                }
            }
        }
        yield return new WaitForSeconds(1);
    }

    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        isSpawning = true;
        enemiesLeftToSpawn = waves[currentWave].GetTotalEnemies();
    }

    private void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++;
        StartCoroutine(StartWave());
    }
}
