using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] AudioClip ambientSound;
    [SerializeField] AudioClip victorySound;
    private AudioSource mainAudio;
    // Start is called before the first frame update
    void Start()
    {
        mainAudio = GetComponent<AudioSource>();

        mainAudio.clip = ambientSound;
        mainAudio.Play();
        mainAudio.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
