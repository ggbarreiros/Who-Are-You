using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{   
    public AudioSource audioSrc;
    public string actualArea;
    [Space]
    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip[] grassClips;
    [SerializeField]
    private AudioClip[] woodClips;
    [SerializeField]
    private AudioClip[] stoneClips;
    [SerializeField]
    private AudioClip[] mudClips;
    [SerializeField]
    private AudioClip[] defaultClip;

    void Awake()
    {

    }

    public void Step()
    {
        AudioClip clip = GetRandomClip();
        audioSrc.PlayOneShot(clip);
    }

    private AudioClip GetRandomClip()
    {
        AudioClip choosenClip = null;
        switch (actualArea)
        {
            case "grama":
                choosenClip = grassClips[UnityEngine.Random.Range(0, grassClips.Length)];
                break;
            case "madeira":
                choosenClip = woodClips[UnityEngine.Random.Range(0, woodClips.Length)];
                break;
            case "pedra":
                choosenClip = stoneClips[UnityEngine.Random.Range(0, stoneClips.Length)];
                break;
            case "lama":
                choosenClip = mudClips[UnityEngine.Random.Range(0, mudClips.Length)];
                break;
            default:
                choosenClip = defaultClip[UnityEngine.Random.Range(0, defaultClip.Length)];
                break;
        }
        return choosenClip;
    }
}
