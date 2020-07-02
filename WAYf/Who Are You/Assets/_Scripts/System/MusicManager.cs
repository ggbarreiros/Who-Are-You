using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public float slider = 0.1f;

    [Header("Songs")]
    public AudioClip bgSong;
    public AudioClip normalSong;
    public AudioClip awareSong;
    public AudioClip fightSong;

    public AudioSource audioSRCBG;
    public AudioSource audioSRCNormal;
    public AudioSource audioSRCAware;
    public AudioSource audioSRCFight;

    //[HideInInspector]
    public bool awarePlay, fightPlay; 

    void Start()
    {
        audioSRCBG.clip = bgSong;
        audioSRCNormal.clip = normalSong;
        audioSRCAware.clip = awareSong;
        audioSRCFight.clip = fightSong;

        audioSRCAware.volume = 0;
        audioSRCFight.volume = 0;

        audioSRCBG.Play();
        audioSRCNormal.Play();
        audioSRCAware.Play();
        audioSRCFight.Play();
    }

    
    void Update()
    {
        //GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");


        if (awarePlay == true)
        {
            audioSRCAware.volume += slider;
        }
        else
        {
            audioSRCAware.volume -= slider;
        }

        if (fightPlay == true)
        {
            audioSRCFight.volume += slider;
        }
        else
        {
            audioSRCFight.volume -= slider;
        }
    }
}
