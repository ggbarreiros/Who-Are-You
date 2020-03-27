using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public GameObject player;
 
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        
    }

    public void KnockbackActive(float thrust, float time)
    {
        Rigidbody2D enemyRb = GetComponent<Rigidbody2D>();
        if(enemyRb != null)
        {
            enemyRb.isKinematic = false;
            Vector2 difference = transform.position - player.transform.position;
            difference = difference.normalized * thrust;
            enemyRb.AddForce(difference, ForceMode2D.Impulse);
            StartCoroutine(KnockCo(enemyRb, time));
            Debug.Log("KNOCKBACK BITCHH!!!");
        }
    }

    private IEnumerator KnockCo(Rigidbody2D enemy, float knockTime)
    {
        if (enemy != null)
        {
            yield return new WaitForSeconds(knockTime);
            enemy.velocity = Vector2.zero;
            enemy.isKinematic = true;
        }
    }
}
