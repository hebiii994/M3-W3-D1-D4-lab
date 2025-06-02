using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour
{
    public static RoundManager instance;

    public static event System.Action OnGameWon;
    public static event System.Action<int> OnScoreChanged;
    public static event System.Action<int, int> OnRoundChanged; 

    public int currentRound = 0;
    public int maxRounds = 3;
    public int enemiesPerRound = 10;


   
    public int currentScore { get; private set; } = 0; 
    [SerializeField] private int scorePerEnemy = 100;

    [SerializeField] private float spawnInterval = 3.0f;
    [SerializeField] private List<SpawnController> _spawnPoints;
    public int totalEnemiesSpawnedInRound = 0;
    public int totalEnemiesEliminatedInRound = 0;


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
            return;
        }
        currentScore = 0;

    }

    private void Start()
    {
        ApplyGameSettings();
        //StartNewRound();
    }

    private void ApplyGameSettings()
    {
        
        if (GameSettings.instance != null)
        {
            if (GameSettings.instance.isInfiniteMode)
            {
                this.maxRounds = int.MaxValue; 
                Debug.Log("Modalità Infinita ATTIVATA");
            }
            else
            {
                this.maxRounds = GameSettings.instance.numberOfRounds;
                Debug.Log($"Numero di round impostato a: {this.maxRounds}");
            }
        }
        else
        {
            
            Debug.LogWarning("GameSettings non trovato. Uso valori di default.");
            this.maxRounds = 5;
        }
        currentRound = 0; 
        currentScore = 0;
        OnScoreChanged?.Invoke(currentScore); 
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

                if (OnGameWon != null)
                {
                    OnGameWon.Invoke(); 
                }

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

    private void OnEnable()
    {
        PlayerLifeController.OnPlayerDied += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        PlayerLifeController.OnPlayerDied -= HandlePlayerDeath;
    }

    private void HandlePlayerDeath()
    {
        Debug.Log("Il giocatore è morto! Ritorno al menu principale...");     
        ReturnToMenu();
    }

    private void ReturnToMenu()
    {
        
        if (GameSettings.instance != null)
        {
            Destroy(GameSettings.instance.gameObject);
        }

        
        SceneManager.LoadScene("MainMenu");
    }
    public void StartNewRound()
    {
        currentRound++;
        enemiesPerRound += 5; 
        totalEnemiesSpawnedInRound = 0;
        totalEnemiesEliminatedInRound = 0;
        timer = 0;
        Debug.Log($"Inizio del Round {currentRound}!");

        OnRoundChanged?.Invoke(currentRound, this.maxRounds);
    }

    public void ReportEnemyKilled()
    {
        totalEnemiesEliminatedInRound++;
        currentScore += scorePerEnemy;

        // Notifica il cambio di punteggio
        OnScoreChanged?.Invoke(currentScore);
        Debug.Log($"Nemico eliminato. Punteggio: {currentScore}");
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

            float baseHealth = newEnemy.BaseHealthForScaling;
            float baseSpeed = newEnemy.BaseSpeedForScaling;
            float baseScale = newEnemy.BaseScaleForScaling;

            float finalHealth = baseHealth * healthMultiplier;
            float finalSpeed = baseSpeed * speedMultiplier;
            float finalScale = baseScale * scaleMultiplier;

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
