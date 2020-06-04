using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] clips;
    public AudioSource audioSrc;

    void Awake()
    {

    }

    private void Step()
    {
        AudioClip clip = GetRandomClip();
        audioSrc.PlayOneShot(clip);
    }

    private AudioClip GetRandomClip()
    {
        return clips[UnityEngine.Random.Range(0, clips.Length)];
    }
}
