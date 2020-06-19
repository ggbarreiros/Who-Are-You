using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAnimation : MonoBehaviour
{
    public float timer = 0;
    public float animationTime;
    private bool animationPlaying = false;
    private Animator anim;
    private PlayerMovement playerMove;
    private AttackSystemRequiem atkSystem;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMove = GetComponent<PlayerMovement>();
        atkSystem = GetComponent<AttackSystemRequiem>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer > animationTime && !animationPlaying)
        {
            //Fazer animação de Idle
            anim.SetFloat("direction", 1f);
            anim.Play("WaitAnimStart");
            animationPlaying = true;
            DeactivateScripts();
        }

        if (Input.anyKey && animationPlaying)
        {
            timer = 0;
            animationPlaying = false;
            anim.SetFloat("direction", -1f);
        }
        else if(Input.anyKey && !animationPlaying)
        {
            timer = 0;
        }
        
    }

    public void DeactivateScripts()
    {
        playerMove.enabled = false;
        atkSystem.enabled = false;
    }

    public void ActivateScripts()
    {
        playerMove.enabled = true;
        atkSystem.enabled = true;
    }
}
