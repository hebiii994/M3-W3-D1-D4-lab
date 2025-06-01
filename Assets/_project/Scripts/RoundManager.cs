using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public static RoundManager instance;

    public int currentRound = 0;
    public int maxRounds = 3;
    public int enemiesPerRound = 15;
    public int totalEnemiesSpawnedInRound = 0;
    public int totalEnemiesEliminatedInRound = 0;

    [SerializeField] private float spawnInterval = 5.0f;
    [SerializeField]private List<SpawnController> _spawnPoints;

    private float timer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        StartNewRound();
    }

    private void Update()
    {
        if ( totalEnemiesEliminatedInRound >= enemiesPerRound )
        {
            if (currentRound < maxRounds)
            {
                StartNewRound();
            }
            else
            {
                Debug.Log("HAI VINTO!");
                this.enabled = false;
            }
            return;
        }
        timer += Time.deltaTime;

        if ( timer >= spawnInterval && totalEnemiesSpawnedInRound < enemiesPerRound)
        {
            SpawnOneEnemy();
            timer = 0f;
        }
    }
    public void StartNewRound()
    {
        currentRound++;
        enemiesPerRound += 5; 
        totalEnemiesSpawnedInRound = 0;
        totalEnemiesEliminatedInRound = 0;
        timer = 0;
        Debug.Log($"Inizio del Round {currentRound}!");
    }

    private void SpawnOneEnemy()
    {
        if (_spawnPoints == null || _spawnPoints.Count == 0)
        {
            Debug.LogError("Nessun spawn point assegnato al RoundManager!");
            return;
        }

        int randomIndex = Random.Range(0, _spawnPoints.Count );
        SpawnController randomSpawnPoint = _spawnPoints[randomIndex];
        Debug.Log($"Spawning da {randomSpawnPoint.gameObject.name}");
        Enemy newEnemy = randomSpawnPoint.SpawnEnemy();

        if ( newEnemy != null )
        {
            float healthMultiplier = 1f + (currentRound * 0.4f);
            float speedMultiplier = 1f + (currentRound * 0.2f);
            float scaleMultiplier = 1f + (currentRound * 0.1f);

            float finalHealth = 10f * healthMultiplier;
            float finalSpeed = 2.0f * speedMultiplier;
            float finalScale = 1f * scaleMultiplier;

            newEnemy.Setup(finalHealth, finalSpeed, finalScale);
            Debug.Log($"Spawning da {randomSpawnPoint.gameObject.name}. Round: {currentRound}, Vita: {finalHealth}, Vel: {finalSpeed}");
        }
        else
        {
            Debug.LogError($"Spawn fallito da {randomSpawnPoint.gameObject.name}");
        }
        totalEnemiesSpawnedInRound++;
    }
}
