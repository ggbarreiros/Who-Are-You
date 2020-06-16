using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ImproveKnockback : MonoBehaviour
{
    public GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void KnockbackActive(float thrust, float time)
    {
        Rigidbody2D enemyRb = GetComponent<Rigidbody2D>();
        if (enemyRb != null)
        {
            Vector3 difference = transform.position - player.transform.position;
            difference = difference.normalized * thrust;
            enemyRb.DOMove(enemyRb.transform.position + difference, time);
        }
    }
}
