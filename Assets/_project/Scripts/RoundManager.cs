using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public static RoundManager instance;

    public int currentRound = 1;
    public int maxRounds = 3;
    public int enemiesPerRound = 15;
    public int totalEnemiesSpawnedInRound = 0;
    public int totalEnemiesEliminatedInRound = 0;

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
    public void StartNewRound()
    {
        currentRound++;
        enemiesPerRound += 5; 
        totalEnemiesSpawnedInRound = 0;
        totalEnemiesEliminatedInRound = 0;
    }

}
