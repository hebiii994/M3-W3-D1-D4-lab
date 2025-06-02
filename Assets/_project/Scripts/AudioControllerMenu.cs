using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControllerMenu : MonoBehaviour
{

    [SerializeField] private AudioClip _mainMenuSong;
    private AudioSource _mainAudio;


    void Awake() 
    {
        _mainAudio = GetComponent<AudioSource>();
        if (_mainAudio == null)
        {
            Debug.LogError("AudioSource mancante su AudioController!");
            this.enabled = false;
            return;
        }
    }

    void Start()
    {
        if (_mainMenuSong != null)
        {
            _mainAudio.clip = _mainMenuSong;
            _mainAudio.loop = true;         // La musica del menu di solito è in loop
            _mainAudio.volume = 0.5f;
            _mainAudio.playOnAwake = false; // Controlliamo noi quando parte
            _mainAudio.Play();
            Debug.Log("Riproduzione musica menu principale: " + _mainMenuSong.name);
        }
        else
        {
            Debug.LogError("MainMenuSong non assegnata allo script MainMenuAudio!");
        }

        // Assicurati che il cursore del sistema operativo sia visibile nel menu
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None; // Sblocca il cursore se era bloccato
    
}

    // Update is called once per frame
    void Update()
    {
        
    }
}
