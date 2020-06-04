using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;

    public bool isDashing;
    public bool hasDashed;
    public float dashSpeed = 20;
    public float dashTime = .3f;

    public Animator anim;
    public SpriteRenderer sr;
    public int side = 1;

    public AttackSystem attackSystem;

    public UnityEvent OnStartAttack, OnFinishAttack;

    public Attack atkScript;
    public bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");
        bool xCheck = Input.GetButton("Horizontal");
        bool yCheck = Input.GetButton("Vertical");

        Vector2 dir = new Vector2(x, y);
        Walk(dir);

        if (Input.GetKeyDown(KeyCode.Space) && !hasDashed)
        {
            if (xRaw != 0 || yRaw != 0)
                Dash(xRaw, yRaw);
        }

        if(xCheck || yCheck)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
        #region Flip
        /*
        if (x > 0)
        {
            side = 1;
            Flip(side);
            atkScript.FlipAtk();
        }
        if (x < 0)
        {
            side = -1;
            Flip(side);
            atkScript.FlipAtk();
        }
        */

        if(facingRight && x < 0)
        {
            Flip();  
        }
        else if(!facingRight && x > 0)
        {
            Flip();
        }
        #endregion
    } //FIM DO METODO UPDATE


    private void Walk(Vector2 dir)
    {
        if(!isDashing)
            rb.velocity = new Vector2(dir.x * moveSpeed, dir.y * moveSpeed);
    }

    private void Dash(float x, float y)
    {
        OnStartAttack.Invoke();
        hasDashed = true;

        //anim.SetTrigger("dash");

        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2(x, y);

        rb.velocity += dir.normalized * dashSpeed;
        StartCoroutine(DashWait());
    }


    IEnumerator DashWait()
    {
        //FindObjectOfType<GhostTrail>().ShowGhost();
        StartCoroutine(EndDash());

        isDashing = true;

        yield return new WaitForSeconds(dashTime);

        OnFinishAttack.Invoke();
        isDashing = false;
    }

    IEnumerator EndDash()
    {
        yield return new WaitForSeconds(.15f);
        hasDashed = false;
        //Detectar colisão com inimigo aqui.
    }

    public void Flip() //int side
    {
        /* if (side == -1 && sr.flipX)
             return;

         if (side == 1 && !sr.flipX)
         {
             return;
         }

     bool state = (side == 1) ? false : true;
     sr.flipX = state;
     */

        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        atkScript.FlipAtk(facingRight);
    }
}
