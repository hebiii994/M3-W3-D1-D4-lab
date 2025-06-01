using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private float spawnInterval = 0f;

    //private int enemiesToSpawnThisRound;
    //private int enemiesSpawned;
    private float timer;


    // Start is called before the first frame update
    void Start()
    {
        if (_enemyPrefab == null)
        {
            Debug.LogError("Prefab del nemico non assegnato nello SpawnController!");
        }

        if (RoundManager.instance == null)
        
        {
            Debug.Log("Round Manager assente.");
        }
        
        timer = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if (RoundManager.instance.totalEnemiesSpawnedInRound >= RoundManager.instance.enemiesPerRound && RoundManager.instance.totalEnemiesEliminatedInRound == RoundManager.instance.enemiesPerRound)
        {
            if (RoundManager.instance.currentRound < RoundManager.instance.maxRounds)
            {
                RoundManager.instance.StartNewRound();
            }
            
            return;
        }

        timer += Time.deltaTime;

        if (timer > spawnInterval && RoundManager.instance.totalEnemiesSpawnedInRound < RoundManager.instance.enemiesPerRound)
        {
            if (RoundManager.instance.totalEnemiesSpawnedInRound > 0 && spawnInterval == 0)
            {
                spawnInterval = 5f;
            }

            Instantiate(_enemyPrefab, transform.position, Quaternion.identity);
            RoundManager.instance.totalEnemiesSpawnedInRound++;
            timer = 0;

        }
        
    }
}