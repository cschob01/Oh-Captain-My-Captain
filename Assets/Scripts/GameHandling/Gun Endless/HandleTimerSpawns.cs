using System.Collections;
using UnityEngine;

[System.Serializable]
public class EnemyTimerInfo
{
    public float StartTime;
    public float EndTime;
    public float MaxProportion; /// From 0-1, Takes EndTime - StartTime to reach it
    public GameObject EnemyPrefab;
}

public class HandleTimerSpawns : MonoBehaviour
{
    [SerializeField] private int MaxEnemies = 20;
    [SerializeField] private EnemyTimerInfo[] Enemies; // First element will be treated as DEAFAULT enemy
    [SerializeField] private SpawnInfo[] SpawnPoints;
    private bool[] ActiveSpawns;

    [SerializeField] private Timer timer;
    [SerializeField] private float StartSpawnRate = 5f;
    [SerializeField] private float Divider = 1.3f;
    [SerializeField] private float DivideTime = 30f;

    [Header("Enemy diffuclty control")]
    [Tooltip("Base health multipler increments every HealthIncreaseRate seconds")]
    [SerializeField] private float HealthIncreaseRate = 60;
    private float HealthMultiplier = 1f;


    private int EnemiesInPlay = 0;
    private float SpawnRate;

    [System.Serializable]
    public class SpawnInfo
    {
        public Transform SpawnPoint;

        [Tooltip("Use \"\" to start active")]
        public string UnlockBeat;
    }

    private void Awake()
    {
        ActiveSpawns = new bool[SpawnPoints.Length];
        for (int i = 0; i < SpawnPoints.Length; i++)
        {
            if (SpawnPoints[i].UnlockBeat == "")
            {
                ActiveSpawns[i] = true;
            }
        }

        SpawnRate = StartSpawnRate;
        StartCoroutine(SpawnRoutine());
    }

    private void FixedUpdate()
    {
        SpawnRate *= Mathf.Pow(1f / Divider, Time.fixedDeltaTime / DivideTime);
        HealthMultiplier += Time.fixedDeltaTime / HealthIncreaseRate;
    }

    private void OnEnable()
    {
        EventHandler.Instance.OnBeatChange += ActivateSpawn;
        EventHandler.Instance.OnEnemyDied += OnEnemyDied;
    }

    private void OnDisable()
    {
        EventHandler.Instance.OnBeatChange -= ActivateSpawn;
        EventHandler.Instance.OnEnemyDied -= OnEnemyDied;
    }

    private void ActivateSpawn(string beat)
    {
        for (int i = 0; i < SpawnPoints.Length; i++)
        {
            if (beat == SpawnPoints[i].UnlockBeat)
            {
                ActiveSpawns[i] = true;
            }
        }
    }

    private void OnEnemyDied()
    {
        EnemiesInPlay--;
    }

    IEnumerator SpawnRoutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(SpawnRate);
            if (EnemiesInPlay < MaxEnemies)
            {
                SpawnEnemy();
            }
        }
    }

    private void SpawnEnemy()
    {
        int ClosestIndex = GetClosestSpawn();
        int SpawnIndex = ClosestIndex;

        while (SpawnIndex == ClosestIndex || !ActiveSpawns[SpawnIndex])
        {
            SpawnIndex = Random.Range(0, SpawnPoints.Length);
        }

        GameObject enemy = Instantiate(ChooseEnemy(), SpawnPoints[SpawnIndex].SpawnPoint.position, Quaternion.Euler(0f, 0f, 0f));
        OnBoard onBoard = enemy.GetComponent<OnBoard>();
        Health health = enemy.GetComponentInChildren<Health>();

        if (onBoard == null || health == null)
        {
            Debug.Log("Enemy prefab not set up correctly for HandleTimerSpawns");
            return;
        }

        onBoard.momentum = Ship.Instance.vel;
        health.health *= HealthMultiplier;

        EnemiesInPlay++;
    }

    private int GetClosestSpawn()
    {
        int ClosestIndex = 0;
        float ClosestDist = Mathf.Infinity;

        for (int i = 0; i < SpawnPoints.Length; i++)
        {
            float dist = Vector3.Distance(CaptainHandler.Instance.transform.position, SpawnPoints[i].SpawnPoint.position);

            if (dist < ClosestDist)
            {
                ClosestDist = dist;
                ClosestIndex = i;
            }
        }

        return ClosestIndex;
    }

    private GameObject ChooseEnemy()
    {
        float choice = Random.Range(0f, 1f);

        for (int i = 1; i < Enemies.Length; i++)
        {
            choice -= Enemies[i].MaxProportion * Mathf.Clamp((timer.TimeProg - Enemies[i].StartTime) / (Enemies[i].EndTime - Enemies[i].StartTime) 
                                                                * Enemies[i].MaxProportion, 
                                                                0f, 
                                                                Enemies[i].MaxProportion);
            if (choice <= 0) return Enemies[i].EnemyPrefab;
        }
        return Enemies[0].EnemyPrefab;
    }
}
