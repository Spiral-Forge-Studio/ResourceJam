using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            for (int j = 0; j < spawnGroups[i].GroundPathGroups.Count; j++)
            {
                for (int k = 0; k < spawnGroups[i].GroundPathGroups[j].pathUnits.Count; k++)
                {
                    totalEnemies += spawnGroups[i].GroundPathGroups[j].pathUnits[k].amount;
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
    [SerializeField] public List<PathGroup> GroundPathGroups;
    [SerializeField] public List<PathGroup> FlyingPathGroups;

    public int GetGroundPathGroupCount() => GroundPathGroups.Count;
    public int GetFlyingPathGroupCount() => FlyingPathGroups.Count;
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
    [SerializeField] public GameState gameState;
    
    [Header("Attributes")]
    [SerializeField] private float enemiesPerSecond = 0.5f; //enemies spawning per second
    [SerializeField] private float timeBetweenWaves = 5f; //Time to prepare before next wave

    [Header("Events")]
    public static UnityEvent onEnemyDestroy  = new UnityEvent(); 
    
    [Header("[DEBUG]")]
    [SerializeField] private Path[] groundPaths;
    [SerializeField] private Path[] flyingPaths;
    [SerializeField] private int groundPathAmount;
    [SerializeField] private int flyingPathAmount;
    [SerializeField] private int currentWave = 0;
    [SerializeField] private int currentSpawnGroup = 0;
    [SerializeField] private int currentSpawnAmount = 0;
    public List<bool> allPGSpawned;
    [SerializeField] private bool canSpawnGroup;

    //[SerializeField] private float timeSinceLastSpawn;
    [SerializeField] private int enemiesAlive;
    [SerializeField] private int enemiesLeftToSpawn;
    [SerializeField] private bool isSpawning = false;
    [SerializeField] private int flpgOffset;

    private void Awake()
    {
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }

    private void Start()
    {
        currentWave = 0;
        currentSpawnGroup = 0;

        groundPaths = pathStats.GetGroundPaths();
        groundPathAmount = pathStats.GetNumberOfGroundPaths();
        
        flyingPaths = pathStats.GetFlyingPaths();
        flyingPathAmount = pathStats.GetNumberOfFlyingPaths();

        flpgOffset = groundPathAmount;

        allPGSpawned = Enumerable.Repeat(true, groundPathAmount + flyingPathAmount).ToList();

        Debug.Log("Total enemies: " + waves[currentWave].GetTotalEnemies());

        StartCoroutine(StartWave());
    }

    void Update()
    {
        if (!isSpawning) { return; }

        //if(enemiesLeftToSpawn > 0 && allPGSpawned.All(x => x))
        if(allPGSpawned.All(x => x))
        {
            Debug.Log("started initialize spawn group function");
            InitializeSpawnGroupAsync();
        }

        //if (enemiesAlive == 0 && enemiesLeftToSpawn == 0) {
        if (enemiesLeftToSpawn == 0) {
            EndWave();
        }
    }

    private void InitializeSpawnGroupAsync()
    {
        Debug.Log("Spawngroup count: " + waves[currentWave].GetSpawnGroupCount());
        Debug.Log("Spawngroup number: " + currentSpawnGroup);
        Debug.Log("Current wave: " + currentWave);
        
        if (currentSpawnGroup > waves[currentWave].GetSpawnGroupCount())
        {
            return;
        }

        SpawnGroup _spawnGroup = waves[currentWave].spawnGroups[currentSpawnGroup];

        canSpawnGroup = false;
        StartCoroutine(SGTimerCoroutine(_spawnGroup.spawnGroupDelay));  // Await the delay

        if(_spawnGroup.GroundPathGroups.Any())
        {
            for (int i = 0; i < _spawnGroup.GetGroundPathGroupCount(); i++)
            {
                allPGSpawned[i] = false;
                StartCoroutine(PathGroupCoroutine(_spawnGroup.GroundPathGroups[i], i, false));
            }
        }        
        
        if(_spawnGroup.FlyingPathGroups.Any())
        {
            for (int i = 0; i < _spawnGroup.GetFlyingPathGroupCount(); i++)
            {
                allPGSpawned[i+flpgOffset] = false;
                StartCoroutine(PathGroupCoroutine(_spawnGroup.FlyingPathGroups[i], i, true));
            }
        }

        Debug.Log("Current spawngroup (end): " + currentSpawnGroup);
        currentSpawnGroup++;
    }

    private IEnumerator PathGroupCoroutine(PathGroup _pathGroup, int _pathGroupNumber, bool isFlying)
    {
        //Debug.Log("Starting Pathgroup coroutine");
        for (int i = 0; i < _pathGroup.GetPathUnitCount(); i++)
        {
            for (int j = 0; j < _pathGroup.pathUnits.Count; j++)
            {
                int _enemyType = _pathGroup.pathUnits[j].enemyType;
                int _amount = _pathGroup.pathUnits[j].amount;

                for (int k = 0; k < _amount; k++)
                {
                    while (gameState.IsPaused()) yield return null;

                    if (!isFlying)
                    {
                        InstantiateEnemy(_enemyType, groundPaths[_pathGroupNumber].pointList[0], _pathGroupNumber);
                    }
                    else
                    {
                        InstantiateEnemy(_enemyType, flyingPaths[_pathGroupNumber].pointList[0], _pathGroupNumber);
                        
                    }
                    yield return new WaitForSecondsRealtime(enemiesPerSecond);
                }
            }
        }

        allPGSpawned[_pathGroupNumber] = true;
    }

    private IEnumerator SGTimerCoroutine(float delay)
    {
        while (gameState.IsPaused()) yield return null;

        yield return new WaitForSecondsRealtime(delay);
        canSpawnGroup = true;
    }

    private void InstantiateEnemy(int _enemyType, Transform _transform, int _pathGroupNumber)
    {
        GameObject prefabToSpawn = enemyPrefabs[_enemyType];

        GameObject spawnedEnemyObject = Instantiate(prefabToSpawn, _transform.position, Quaternion.identity);

        Enemy enemyScript = spawnedEnemyObject.GetComponent<Enemy>();
       
        enemyScript.GetComponentInChildren<Enemy>().pathAssignment = _pathGroupNumber;

        enemiesAlive++;
        enemiesLeftToSpawn--;
    }

    private void EnemyDestroyed()
    {
        enemiesAlive--;
    }

    private IEnumerator StartWave()
    {
        while (gameState.IsPaused()) yield return null;
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
}
