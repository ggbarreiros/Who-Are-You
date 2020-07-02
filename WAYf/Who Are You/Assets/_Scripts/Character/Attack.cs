using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Attack : MonoBehaviour
{
    private int damage;
    private bool cameraShake;
    private AudioClip hitSound;


    public float knockbackForce;
    public float knockbackTime;

    public GameObject atkHolder;

    public GameObject right, left;

    public void SetAttack(Hit hit)
    {       
        damage = hit.damage;
        //hitSound = hit.hitSound;
    }

    

    private void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if(enemy != null)
        {
            enemy.TakeDamage(damage);
            other.GetComponent<ImproveKnockback>().KnockbackActive(knockbackForce, knockbackTime);
        }
    }

    public void FlipAtk(bool facing)
    {
        transform.position = left.transform.position; 
        /*
        Debug.Log(facing);
        if (facing)
        {
            //transform.position = new Vector3(transform.position.x - transform.position.x,transform.position.y, transform.position.z);
            transform.position = right.transform.position;
        }
        else
        {
            //transform.position = new Vector3(transform.position.x - 0.45f, transform.position.y, transform.position.z);
            transform.position = left.transform.position;
        }
        */
    }
}
