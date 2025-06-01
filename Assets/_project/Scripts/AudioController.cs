using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private PlayerShooterController _shooterController;
    private bool victoryMusicHasPlayed = false;

    [SerializeField] AudioClip ambientSound;
    [SerializeField] AudioClip victorySound;
    private AudioSource _mainAudio;
    private float _time = 0f;
    // Start is called before the first frame update
    void Start()
    {
        _mainAudio = GetComponent<AudioSource>();

        if (_shooterController == null)
        {
            Debug.LogError("Riferimento al PlayerShooterController non assegnato nell'AudioController!");
        }

        _mainAudio.clip = ambientSound;
        _mainAudio.Play();
        _mainAudio.loop = true;
        _mainAudio.volume = 0.15f;
        
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;
        // 2. Controlla se il riferimento esiste, se la lista dei nemici è vuota e se la musica della vittoria non è già partita
        if (_shooterController != null && _shooterController._enemiesList.Count == 0 && !victoryMusicHasPlayed && _time > 6.0f)
        {
            PlayVictorySound();
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
