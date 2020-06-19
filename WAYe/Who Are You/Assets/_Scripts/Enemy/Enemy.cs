using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class Enemy : MonoBehaviour
{
    public int health;


    public float damageTime = 0.1f;
    public Color damageColor;
    private SpriteRenderer spriteRenderer;
    private Color defaultColor;
    public FlipEnemy enemyBehaviour;

    public CameraShake camShake;

    public AudioClip[] dmgSounds;
    public AudioSource audioSrc;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;
        if (enemyBehaviour == null)
            enemyBehaviour = GetComponent<FlipEnemy>();
    }

    void Update()
    {
        if(health <= 0)
        {
            enemyBehaviour.StopSounds();
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        if (enemyBehaviour != null)
        {
           StartCoroutine(DebuggingCD());
        }
        spriteRenderer.color = damageColor;
        health -= damage;
        Invoke("ReleaseDamage", damageTime);

        camShake.Shacking();
        DamageSound();
    }

    public IEnumerator DebuggingCD()
    {
        enemyBehaviour.enabled = false;
        enemyBehaviour.ableToPursuit = false;
        enemyBehaviour.GetComponent<Animator>().SetBool("isFollowing", false);
        yield return new WaitForSeconds(0.09f);
        enemyBehaviour.enabled = true;
        enemyBehaviour.ableToPursuit = true;
    }

    void ReleaseDamage()
    {
        spriteRenderer.color = defaultColor;
    }



    public void DamageSound()
    {
        AudioClip clip = SelectSound();
        audioSrc.PlayOneShot(clip);

    }

    private AudioClip SelectSound()
    {
        AudioClip clip;
        return clip = dmgSounds[UnityEngine.Random.Range(0, dmgSounds.Length)];
    }
}
