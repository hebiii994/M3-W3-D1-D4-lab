using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro; 

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField _roundsInputField;
    [SerializeField] private Toggle _infiniteToggle;
    [SerializeField] private Button _startButton;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        _startButton.onClick.AddListener(OnStartButtonPressed);
        _infiniteToggle.onValueChanged.AddListener(OnToggleValueChanged);

        _roundsInputField.text = "10";
        OnToggleValueChanged(false);
    }

    private void OnToggleValueChanged(bool isInfinite)
    {
        
        _roundsInputField.interactable = !isInfinite;
    }

    private void OnStartButtonPressed()
    {
        bool infiniteMode = _infiniteToggle.isOn;
        int rounds = 10;
        if (!infiniteMode)
        {
            if (!int.TryParse(_roundsInputField.text, out rounds) || rounds <= 0)
            {
                
                rounds = 10;
                Debug.LogWarning("Input dei round non valido, impostato a 10 di default.");
            }
        }
        GameSettings.instance.isInfiniteMode = infiniteMode;
        GameSettings.instance.numberOfRounds = rounds;

        // Avvia il gioco!
        GameSettings.instance.StartGame();
    }
}
