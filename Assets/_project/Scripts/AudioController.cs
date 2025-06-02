using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    
    private bool victoryMusicHasPlayed = false;

    [SerializeField] AudioClip ambientSound;
    [SerializeField] AudioClip victorySound;
    private AudioSource _mainAudio;
    
    void Start()
    {
        _mainAudio = GetComponent<AudioSource>();
        _mainAudio.clip = ambientSound;
        _mainAudio.Play();
        _mainAudio.loop = true;
        _mainAudio.volume = 0.1f;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!victoryMusicHasPlayed) 
        {
            if (RoundManager.instance != null && RoundManager.instance.currentRound > RoundManager.instance.maxRounds)
            {
                PlayVictorySound();
            }
        }

        
    }

    void PlayVictorySound()
    {
        victoryMusicHasPlayed = true;
        _mainAudio.Stop();
        _mainAudio.loop = false;
        _mainAudio.clip = victorySound;
        _mainAudio.Play();
        _mainAudio.volume = 0.6f;
    }
}
