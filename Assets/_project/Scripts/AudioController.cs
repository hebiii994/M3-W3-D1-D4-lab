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
        
        RoundManager.OnGameWon += PlayVictoryMusicAndStopAmbient; 
    }

    private void OnDisable()
    {
        
        RoundManager.OnGameWon -= PlayVictoryMusicAndStopAmbient; 
    }


    void PlayAmbientSound()
    {
        if (_mainAudio.isPlaying && _mainAudio.clip == ambientSound) return; 

        _mainAudio.clip = ambientSound;
        _mainAudio.loop = true;
        _mainAudio.volume = 0.15f; 
        _mainAudio.Play();
    }

    void PlayVictoryMusicAndStopAmbient()
    {
        if (_victoryMusicPlayed) return; 

        Debug.Log("AudioController ha ricevuto OnGameWon. Avvio musica vittoria.");
        _victoryMusicPlayed = true;

        _mainAudio.Stop(); 
        _mainAudio.clip = victorySound;
        _mainAudio.loop = false; 
        _mainAudio.volume = 0.6f; 
        _mainAudio.Play();
    }
}
