using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;

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
            for (int j = 0; j < spawnGroups[i].FlyingPathGroups.Count; j++)
            {
                for (int k = 0; k < spawnGroups[i].FlyingPathGroups[j].pathUnits.Count; k++)
                {
                    totalEnemies += spawnGroups[i].FlyingPathGroups[j].pathUnits[k].amount;
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
    [SerializeField] public float delayTillNextSpawnGroup;
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
    [SerializeField] public GameObject waveClearText;
    

    [Header("Attributes")]
    [SerializeField] private float enemiesPerSecond = 0.5f; //enemies spawning per second
    [SerializeField] private float timeBetweenWaves = 5f; //Time to prepare before next wave

    [Header("Events")]
    //public static UnityEvent onEnemyDestroy  = new UnityEvent(); 
    
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
    [SerializeField] private Coroutine sgTimer;

    private void Awake()
    {
        
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

        //Debug.Log("offest: " + flpgOffset);

        //Debug.Log("Total enemies: " + waves[currentWave].GetTotalEnemies());

        enemiesLeftToSpawn = waves[currentWave].GetTotalEnemies();
        canSpawnGroup = true;
        StartCoroutine(StartWave());
    }

    void Update()
    {
        if (!isSpawning) { return; }

        //if(enemiesLeftToSpawn > 0 && allPGSpawned.All(x => x))
        if(allPGSpawned.All(x => x))
        {
            InitializeSpawnGroupAsync();
        }

        //if (enemiesAlive == 0 && enemiesLeftToSpawn == 0) {
        if (enemiesLeftToSpawn == 0 && enemiesAlive <= 0) {
            EndWave();
        }
    }

    private void InitializeSpawnGroupAsync()
    {

        if (currentSpawnGroup > waves[currentWave].GetSpawnGroupCount() - 1)
        {
            return;
        }

        SpawnGroup _spawnGroup = waves[currentWave].spawnGroups[currentSpawnGroup];

        if (canSpawnGroup == false)
        {
            return;
        }

        if (canSpawnGroup)
        {
            sgTimer = StartCoroutine(SGTimerCoroutine(_spawnGroup.delayTillNextSpawnGroup));  // Await the delay
        }

        if (_spawnGroup.GroundPathGroups.Any())
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
                allPGSpawned[i + flpgOffset] = false;
                StartCoroutine(PathGroupCoroutine(_spawnGroup.FlyingPathGroups[i], i, true));
            }
        }

        //Debug.Log("Current spawngroup (end): " + currentSpawnGroup);
        currentSpawnGroup++;


    }

    private IEnumerator PathGroupCoroutine(PathGroup _pathGroup, int _pathGroupNumber, bool isFlying)
    {
        //Debug.Log("Starting Pathgroup coroutine " + _pathGroupNumber + ", isflying: " + isFlying);
        for (int i = 0; i < _pathGroup.GetPathUnitCount(); i++)
        {
            for (int j = 0; j < _pathGroup.pathUnits[i].amount; j++)
            {
                while (gameState.IsPaused()) yield return null;

                if (!isFlying)
                {
                    //Debug.Log("spawned ground: " + _enemyType + " , order: " + k);
                    InstantiateEnemy(_pathGroup.pathUnits[i].enemyType, groundPaths[_pathGroupNumber].pointList[0], _pathGroupNumber);
                }
                else
                {
                    InstantiateEnemy(_pathGroup.pathUnits[i].enemyType, flyingPaths[_pathGroupNumber].pointList[0], _pathGroupNumber);
                        
                }
                yield return new WaitForSeconds(enemiesPerSecond);
            }
        }

        if (!isFlying)
        {
            allPGSpawned[_pathGroupNumber] = true;
        }
        else if (isFlying)
        {
            //Debug.Log("Point at pg " + _pathGroupNumber + ": " + flyingPaths[_pathGroupNumber].pointList[0].name);
            allPGSpawned[_pathGroupNumber + flpgOffset] = true;
        }
    }

    private IEnumerator SGTimerCoroutine(float delay)
    {
        canSpawnGroup = false;
        float prevTime = Time.time;

        while (gameState.IsPaused()) yield return null;

        yield return new WaitForSeconds(delay);
        Debug.Log("ended at duration of: " + (Time.time - prevTime));
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

    public void EnemyDestroyed()
    {
        //Debug.Log("Subtracting Enemies");
        enemiesAlive--;
        gameState._totalEnemiesThisWave--;
    }

    private IEnumerator StartWave()
    {
        gameState.BuildPhase = true;
        gameState._currentWave = currentWave + 1;
        gameState._totalWaves = waves.Length;
        gameState._totalEnemiesThisWave = waves[currentWave].GetTotalEnemies(); ;

        while (gameState.IsPaused() || gameState.BuildPhase) yield return null;

        enemiesLeftToSpawn = waves[currentWave].GetTotalEnemies();
        

        yield return new WaitForSeconds(timeBetweenWaves);
        currentSpawnGroup = 0;
        isSpawning = true;
    }

    private void EndWave()
    {
        Instantiate(waveClearText);
        isSpawning = false;
        currentWave++;

        if (currentWave < waves.Length)
        {
            StartCoroutine(StartWave());
        }
        else
        {
            gameState.EndLevel = true;
        }
    }

}
