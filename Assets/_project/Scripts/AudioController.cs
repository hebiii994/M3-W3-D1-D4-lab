using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    
   // private bool victoryMusicHasPlayed = false;

    [SerializeField] AudioClip ambientSound;
    [SerializeField] AudioClip victorySound;
    
    private AudioSource _mainAudio;

    private bool _victoryMusicPlayed = false; 

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
        PlayAmbientSound();
    }

    private void OnEnable()
    {
        // Iscriviti all'evento OnGameWon
        RoundManager.OnGameWon += PlayVictoryMusicAndStopAmbient; // <-- QUESTA RIGA ASCOLTA L'EVENTO
    }

    private void OnDisable()
    {
        // Annulla l'iscrizione per evitare problemi
        RoundManager.OnGameWon -= PlayVictoryMusicAndStopAmbient; // <-- QUESTA RIGA SMETTE DI ASCOLTARE
    }


    void PlayAmbientSound()
    {
        if (_mainAudio.isPlaying && _mainAudio.clip == ambientSound) return; // Già in riproduzione

        _mainAudio.clip = ambientSound;
        _mainAudio.loop = true;
        _mainAudio.volume = 0.15f; // O il volume che preferisci
        _mainAudio.Play();
    }

    void PlayVictoryMusicAndStopAmbient()
    {
        if (_victoryMusicPlayed) return; // Assicura che venga suonata una sola volta

        Debug.Log("AudioController ha ricevuto OnGameWon. Avvio musica vittoria.");
        _victoryMusicPlayed = true;

        _mainAudio.Stop(); // Ferma la musica ambientale
        _mainAudio.clip = victorySound;
        _mainAudio.loop = false; // La musica della vittoria di solito non è in loop
        _mainAudio.volume = 0.6f; // O il volume che preferisci
        _mainAudio.Play();
    }
}
