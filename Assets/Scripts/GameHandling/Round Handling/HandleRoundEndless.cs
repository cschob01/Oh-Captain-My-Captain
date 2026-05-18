using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.ParticleSystem;

public class HandleRoundEndless : MonoBehaviour
{
    [SerializeField] private int MaxEnemies = 10;
    [SerializeField] private GameObject EnemyPrefab;
    [SerializeField] private GameObject Captain;

    public int EnemiesLeft = 0;
    public int Round = 0;

    private Vector3[] SpawnPoints;
    public int EnemiesInPlay = 0;
    private float SpawnSpeed = 5; // Seconds per enemy
    public Coroutine SpawnCoroutine;
    public bool MidRound = false;

    private void OnEnable()
    {
        EventHandler.Instance.OnRoundStart += StartRound;
        EventHandler.Instance.OnRoundEnd += EndRound;
        EventHandler.Instance.OnEnemyDied += OnEnemyDied;
    }

    private void OnDisable()
    {
        EventHandler.Instance.OnRoundStart += StartRound;
        EventHandler.Instance.OnRoundEnd += EndRound;
        EventHandler.Instance.OnEnemyDied -= OnEnemyDied;
    }

    private void Awake()
    {
        List<Vector3> positions = new();

        foreach (Transform child in transform)
        {
            if (child.name.StartsWith("SpawnPoint"))
            {
                positions.Add(child.position);
            }
        }

        SpawnPoints = positions.ToArray();
    }

    private void OnEnemyDied()
    {
        EnemiesInPlay--;
        EnemiesLeft--;
    }

    private void StartRound()
    {
        Round++;
        EventHandler.Instance.RoundChange(Round);

        EnemiesLeft = Round * 5;
        SpawnSpeed = SpawnSpeed / 1.3f;
        MidRound = true;

        EndSpawnRoutine();
        SpawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(SpawnSpeed);
            if (EnemiesInPlay < MaxEnemies && EnemiesLeft - EnemiesInPlay > 0)
            {
                SpawnEnemy();
            }
            if (EnemiesLeft == EnemiesInPlay)
            {
                EndSpawnRoutine();
            }
        }
    }

    private void EndSpawnRoutine()
    {
        if (SpawnCoroutine != null) {
            StopCoroutine(SpawnCoroutine);
            SpawnCoroutine = null;
        }
    }

    private void SpawnEnemy()
    {
        int ClosestIndex = GetClosestSpawn();
        int SpawnIndex = ClosestIndex;

        while (SpawnIndex == ClosestIndex)
        {
            SpawnIndex = Random.Range(0, SpawnPoints.Length);
        }

        GameObject enemy = Instantiate(EnemyPrefab, SpawnPoints[SpawnIndex], Quaternion.Euler(0f, 0f, 0f));
        OnBoard onBoard = enemy.GetComponent<OnBoard>();
        if (onBoard != null)
        {
            onBoard.momentum = Ship.Instance.vel;
        }
        EnemiesInPlay++;
    }

    private int GetClosestSpawn()
    {
        int ClosestIndex = 0;
        float ClosestDist = Mathf.Infinity;

        for (int i = 0; i < SpawnPoints.Length; i++)
        {
            float dist = Vector3.Distance(Captain.transform.position, SpawnPoints[i]);

            if (dist < ClosestDist)
            {
                ClosestDist = dist;
                ClosestIndex = i;
            }
        }

        return ClosestIndex;
    }

    private void EndRound()
    {
        MidRound = false;
        EndSpawnRoutine();
    }

    void FixedUpdate()
    {
        if (EnemiesLeft == 0 && MidRound)
        {
            EventHandler.Instance.RoundEnd();
        }
    }


}
