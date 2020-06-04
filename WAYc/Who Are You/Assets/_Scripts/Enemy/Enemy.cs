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

    public CameraShake camShake;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;
    }

    void Update()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        spriteRenderer.color = damageColor;
        health -= damage;
        Invoke("ReleaseDamage", damageTime);
        //if(damage > 1)
        //{
        //CameraShaker.Instance.ShakeOnce(5f, 5f, .1f, 1f);
        //}
        //else
        //{
        //  Debug.Log(damage);
        //CameraShaker.Instance.ShakeOnce(3f, 3f, .1f, .8f);
        //}
        camShake.Shacking();
    }

    void ReleaseDamage()
    {
        spriteRenderer.color = defaultColor;
    }
}
