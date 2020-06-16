using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipEnemy : MonoBehaviour
{
    public bool aware = false;
    public GameObject player;
    public bool facingRight = true;
    public float distance;
    private Animator anim;
    public float speed;
    //private Enemy enemy;
    public bool ableToPursuit = true;


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        distance = Vector2.Distance(player.transform.position, transform.position);

        if(distance < 14.4f)
        {
            
            if (distance > 2f && ableToPursuit)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed);
                aware = true;
                anim.SetBool("isFollowing", true);
            }
        }

        if (aware)
        {
            if (player.transform.position.x > transform.position.x && !facingRight)
            {
                Flip();
            }
            else if (player.transform.position.x < transform.position.x && facingRight)
            {
                Flip();
            }
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
