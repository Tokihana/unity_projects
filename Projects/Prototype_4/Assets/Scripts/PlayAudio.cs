using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    public AudioClip audioStart;
    public AudioClip audioClipPositive;
    public AudioClip audioClipNegative;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioStart, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WavePositive() { audioSource.PlayOneShot(audioClipPositive); }

    public void WaveNegative() { audioSource.PlayOneShot(audioClipNegative); }
}
