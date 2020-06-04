using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAnimation : MonoBehaviour
{
    public float timer = 0;
    public float animationTime;
    private bool animationPlaying = false;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer > animationTime && !animationPlaying)
        {
            //Fazer animação de Idle
            //anim.Play("WaitAnim");
            animationPlaying = true;
        }

        if (Input.anyKey)
        {
            timer = 0;
            animationPlaying = false;
        }
    }
}
