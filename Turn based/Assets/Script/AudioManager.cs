using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("---------- Audio Source ----------")]
    [SerializeField] AudioSource musicSoures;
    [SerializeField] AudioSource SFXSoures;

    [Header("---------- Audio Clip ----------")]
    public AudioClip background;
    public AudioClip attack;
    public AudioClip critical;
    public AudioClip parry;
    public AudioClip click;

    private void Start()
    {
        musicSoures.clip = background;
        musicSoures.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSoures.PlayOneShot(clip);
    }
}
