using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUIManager : MonoBehaviour
{

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _scoreTextElement;
    [SerializeField] private TextMeshProUGUI _roundTextElement;

    private void Awake()
    {
        if (_scoreTextElement == null)
        {
            Debug.LogError("Elemento ScoreText non assegnato a GameUIManager!");
        }
        if (_roundTextElement == null)
        {
            Debug.LogError("Elemento RoundText non assegnato a GameUIManager!");
        }
    }

    private void OnEnable()
    {
        
        RoundManager.OnScoreChanged += UpdateScoreDisplay;
        RoundManager.OnRoundChanged += UpdateRoundDisplay;
    }

    private void OnDisable()
    {
        
        RoundManager.OnScoreChanged -= UpdateScoreDisplay;
        RoundManager.OnRoundChanged -= UpdateRoundDisplay;
    }

    private void UpdateScoreDisplay(int newScore)
    {
        if (_scoreTextElement != null)
        {
            _scoreTextElement.text = "Score: " + newScore;
        }
    }

    private void UpdateRoundDisplay(int currentRound, int totalRounds)
    {
        if (_roundTextElement != null)
        {
            if (totalRounds == int.MaxValue) 
            {
                _roundTextElement.text = "Round: " + currentRound + " (∞)";
            }
            else
            {
                _roundTextElement.text = "Round: " + currentRound + "/" + totalRounds;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
