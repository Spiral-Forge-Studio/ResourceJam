using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] public List<PathUnit> pathUnits;

    public int GetPathUnitCount() => pathUnits.Count;
}

[Serializable]
public class PathUnit
{
    [SerializeField] public int enemyType = -1;
    [SerializeField] public int amount = -1;
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
    [SerializeField] private int pathAmount;
    [SerializeField] private int currentWave = 0;
    [SerializeField] private int currentSpawnGroup = 0;
    [SerializeField] private int currentSpawnAmount = 0;
    public List<bool> allPGSpawned;
    [SerializeField] private bool canSpawnGroup;

    //[SerializeField] private float timeSinceLastSpawn;
    [SerializeField] private int enemiesAlive;
    [SerializeField] private int enemiesLeftToSpawn;
    [SerializeField] private bool isSpawning = false;

    private void Awake()
    {
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }

    private void Start()
    {
        currentWave = 0;
        paths = pathStats.GetPaths();
        pathAmount = pathStats.GetNumberOfPaths();

        allPGSpawned = Enumerable.Repeat(true, pathAmount).ToList();

        StartCoroutine(StartWave());
    }

    void Update()
    {
        if (!isSpawning) return;

        if(enemiesLeftToSpawn > 0 && allPGSpawned.All(x => x))
        {
            InitializeSpawnGroup();
        }

        if (enemiesAlive == 0 && enemiesLeftToSpawn == 0) {
            EndWave();
        }
    }

    private void InitializeSpawnGroup()
    {

        SpawnGroup _spawnGroup = waves[currentWave].spawnGroups[currentSpawnGroup];

        for (int i = 0; i < _spawnGroup.GetPathGroupCount(); i++)
        {
            allPGSpawned[i] = false;
        }

        canSpawnGroup = false;
        StartCoroutine(SGTimer(_spawnGroup.spawnGroupDelay));

        if (_spawnGroup.GetPathGroupCount() != pathStats.GetNumberOfPaths())
        {
            Debug.Log("Spawngroup " + currentSpawnGroup + 
                ": invalid pathgroup count of " + _spawnGroup.pathGroups.Count);
            return;
        }

        for (int i = 0; i < _spawnGroup.GetPathGroupCount(); i++)
        {
            allPGSpawned[i] = false;
            StartCoroutine(PathGroupCoroutine(_spawnGroup.pathGroups[i], i));
        }
    }

    private IEnumerator PathGroupCoroutine(PathGroup _pathGroup, int _pathGroupNumber)
    {
        for (int i = 0; i < _pathGroup.GetPathUnitCount(); i++)
        {
            for (int j = 0; j < _pathGroup.pathUnits.Count; j++)
            {
                int _enemyType = _pathGroup.pathUnits[j].enemyType;
                int _amount = _pathGroup.pathUnits[j].amount;

                for (int k = 0; k < _amount; k++)
                {
                    InstantiateEnemy(_enemyType, paths[_pathGroupNumber].pointList[0], _pathGroupNumber);
                    yield return new WaitForSecondsRealtime(enemiesPerSecond);
                }
            }
        }

        allPGSpawned[_pathGroupNumber] = true;
    }

    private void InstantiateEnemy(int _enemyType, Transform _transform, int _pathGroupNumber)
    {
        GameObject prefabToSpawn = enemyPrefabs[_enemyType];

        GameObject spawnedEnemyObject = Instantiate(prefabToSpawn, _transform.position, Quaternion.identity);

        Enemy enemyScript = spawnedEnemyObject.GetComponent<Enemy>();
        
        if (enemyScript.isFlying == false)
        {
            enemyScript.GetComponentInChildren<GroundEnemy>().pathAssignment = _pathGroupNumber;
        }

        enemiesAlive++;
        enemiesLeftToSpawn--;
    }

    private void EnemyDestroyed()
    {
        enemiesAlive--;
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
        currentWave++;

        if (currentWave < waves.Length)
        {
            StartCoroutine(StartWave());
        }
    }

    private IEnumerator SGTimer(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        canSpawnGroup = true;
    }
}
