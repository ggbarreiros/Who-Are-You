using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSystem : MonoBehaviour
{
    public Rigidbody2D rb;
    public float impulseForce;
    public PlayerMovement playerMove;
    public float testTime;

    public LookAtPoint lookScript;
    public LayerMask enemyLayer;

    [Header("ATTACK DETAILS", order = -1)]
    public int damage;
    public float knockbackForce;
    public float knockbackTime;
    [Space]

    [Header("Attack Directions", order = 0)]
    [Space]

    [Header("Right Attack", order = 1)]
    public Vector3 rightAttackPosition;
    public Vector2 rightAttackRange;

    [Header("Up Attack")]
    public Vector3 upAttackPosition;
    public Vector2 upAttackRange;

    [Header("Left Attack")]
    public Vector3 leftAttackPosition;
    public Vector2 leftAttackRange;

    [Header("Bottom Attack")]
    public Vector3 bottomAttackPosition;
    public Vector2 bottomAttackRange;





    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var mouseDir = mousePos - gameObject.transform.position;
        mouseDir.z = 0.0f;
        mouseDir = mouseDir.normalized;

        if (Input.GetMouseButtonDown(0))
        {
            //playerMove.enabled = false;
            //rb.AddForce(mouseDir * impulseForce);
            StartCoroutine(AttackingTime(mouseDir));
            StartCoroutine(EnableMovement(testTime));
            //Attack(lookScript.direction);
            #region Flip
            if (mouseDir.x > 0)
            {
                playerMove.side = 1;
                playerMove.Flip(playerMove.side);
            }
            if(mouseDir.x < 0)
            {
                playerMove.side = -1;
                playerMove.Flip(playerMove.side);
            }
            #endregion
        }
    }

    IEnumerator AttackingTime(Vector2 mouse)
    {
        playerMove.enabled = false;
        rb.AddForce(mouse * impulseForce);
        yield return new WaitForSeconds(0.02f);
        Attack(lookScript.direction);
    }



    IEnumerator EnableMovement(float time)
    {
        yield return new WaitForSeconds(time);
        playerMove.enabled = true;
    }


    void Attack(int direction)
    {
        Collider2D[] hitEnemies;
        int attackNumber = 0;
        if (direction == 0 || direction == 7)
        {
            //Ataque para direita
            hitEnemies = Physics2D.OverlapBoxAll(transform.position + rightAttackPosition, rightAttackRange, 0, enemyLayer);

            for(int i = 0; i < hitEnemies.Length; i++)
            {
                hitEnemies[i].GetComponent<Enemy>().TakeDamage(damage);
                hitEnemies[i].GetComponent<ImproveKnockback>().KnockbackActive(knockbackForce, knockbackTime);
            }
        }

        else if(direction == 1 || direction == 2)
        {
            //Ataque para Cima
            hitEnemies = Physics2D.OverlapBoxAll(transform.position + upAttackPosition, upAttackRange, 0, enemyLayer);

            for (int i = 0; i < hitEnemies.Length; i++)
            {
                hitEnemies[i].GetComponent<Enemy>().TakeDamage(damage);
                hitEnemies[i].GetComponent<ImproveKnockback>().KnockbackActive(knockbackForce, knockbackTime);
            }
        }
        else if (direction == 3 || direction == 4)
        {
            //Ataque para Esquerda
            hitEnemies = Physics2D.OverlapBoxAll(transform.position + leftAttackPosition, leftAttackRange, 0, enemyLayer);

            for (int i = 0; i < hitEnemies.Length; i++)
            {
                hitEnemies[i].GetComponent<Enemy>().TakeDamage(damage);
                hitEnemies[i].GetComponent<ImproveKnockback>().KnockbackActive(knockbackForce, knockbackTime);
            }
        }
        else
        {
            //Ataque para Baixo
            hitEnemies = Physics2D.OverlapBoxAll(transform.position + bottomAttackPosition, bottomAttackRange, 0, enemyLayer);

            for (int i = 0; i < hitEnemies.Length; i++)
            {
                hitEnemies[i].GetComponent<Enemy>().TakeDamage(damage);
                hitEnemies[i].GetComponent<ImproveKnockback>().KnockbackActive(knockbackForce, knockbackTime);
            }
        }

        foreach(Collider2D enemy in hitEnemies)
        {
            Debug.Log("We hit " + enemy.name + " " + attackNumber);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + rightAttackPosition, rightAttackRange);
        Gizmos.DrawWireCube(transform.position + upAttackPosition, upAttackRange);
        Gizmos.DrawWireCube(transform.position + leftAttackPosition, leftAttackRange);
        Gizmos.DrawWireCube(transform.position + bottomAttackPosition, bottomAttackRange);
    }
}
