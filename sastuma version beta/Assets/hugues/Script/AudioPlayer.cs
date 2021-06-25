using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviourPunCallbacks
{
    public AudioClip[] shootSounds;
    public AudioClip reloadSound;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void ShootAudio()
    {
        audioSource.PlayOneShot(shootSounds[0]);
    }
}
