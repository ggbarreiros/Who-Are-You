using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class AttackSystemRequiem : MonoBehaviour
{
    public Combo[] combos;

    public Attack attack;

    public List<string> currentCombo; 

    private Animator anim;

    private bool startCombo;

    private Hit currentHit, nextHit;

    private float comboTimer;

    private bool canHit = true;
    private bool resetCombo;

    public UnityEvent OnStartCombo, OnFinishCombo;


    public GameObject mouse;
    public float dashForce;
    public float dashTime;

    public PlayerMovement playerMove;
    public Attack atkScript;

    public LookAtPoint directionScript;

    public AudioClip[] slashSounds;
    public AudioSource audioSrc;

    public ParticleSystem dashParticles;

    private void Awake()
    {
        anim = GetComponent<Animator>();    
    }

    void Start()
    {
        
    }

    void Update()
    {
        CheckInput();

        #region Flip

        //var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //var mouseDir = mousePos - gameObject.transform.position;
        //mouseDir.z = 0.0f;
        //mouseDir = mouseDir.normalized;

        /*
        if (Input.GetMouseButtonDown(0))
        {
            if (mouse.transform.position.x > transform.position.x)
            {
                playerMove.side = 1;
                playerMove.Flip(playerMove.side);
            }
            if (mouse.transform.position.x < transform.position.x)
            {
                playerMove.side = -1;
                playerMove.Flip(playerMove.side);
            }
        }
        */
        if (Input.GetMouseButtonDown(0))
        {
            StartParticles();
            if (mouse.transform.position.x > transform.position.x && playerMove.facingRight == false)
            {
                playerMove.Flip();
                atkScript.FlipAtk(true);
            }
            if (mouse.transform.position.x < transform.position.x && playerMove.facingRight == true)
            {
                playerMove.Flip();
                atkScript.FlipAtk(false);
            }
        }
        #endregion
    }

    void CheckInput()
    {
        for (int i = 0; i < combos.Length; i++)
        {
            if(combos[i].hits.Length > currentCombo.Count)
            {
                if (Input.GetButtonDown(combos[i].hits[currentCombo.Count].inputButton))
                {
                    if (currentCombo.Count == 0)
                    {
                        OnStartCombo.Invoke();
                        Debug.Log("Primeiro Hit foi adicionado");
                        PlayHit(combos[i].hits[currentCombo.Count]);
                        break;
                    }
                    else
                    {
                        bool comboMatch = false;
                        for (int y = 0; y < currentCombo.Count; y++)
                        {
                            if (currentCombo[y] != combos[i].hits[y].inputButton)
                            {
                                Debug.Log("Input não pertence ao hit atual");
                                comboMatch = false;
                                break;
                            }
                            else
                            {
                                comboMatch = true;
                            }
                        }
                        if (comboMatch && canHit)
                        {
                            Debug.Log("Hit Adicionado ao combo");
                            canHit = false;
                            nextHit = combos[i].hits[currentCombo.Count];
                            break;
                        }
                    }
                }
            }

            
        }

        if (startCombo)
        {
            comboTimer += Time.deltaTime;
            if(comboTimer >= currentHit.animationTime && !canHit)
            {
                PlayHit(nextHit);
                playerMove.isAttacking = true;
            }

            if(comboTimer >= currentHit.resetTime)
            {
                ResetCombo();
            }
        }
    }

    void PlayHit(Hit hit)
    {
        playerMove.isAttacking = true;
        comboTimer = 0;
        attack.SetAttack(hit);
        anim.Play(hit.animation[directionScript.directionId]);      
        AttackDash(dashForce, dashTime);
        SlashSound();     
        startCombo = true;
        currentCombo.Add(hit.inputButton);
        currentHit = hit;       
        canHit = true;
        //playerMove.isAttacking = false;
        StartCoroutine(StartParticles());
    }

    void ResetCombo()
    {
        OnFinishCombo.Invoke();
        startCombo = false;
        comboTimer = 0;
        currentCombo.Clear();
        anim.Rebind(); //LEMBRAR DESSA FUNÇÃO
        canHit = true;
        playerMove.isAttacking = false;
    }


    public void AttackDash(float thrust, float time)
    {
        Vector3 difference = transform.position - mouse.transform.position;
        difference = difference.normalized * thrust;
        GetComponent<Rigidbody2D>().DOMove(transform.position - difference, time);
        StartParticles();
    }



    public void SlashSound()
    {
        AudioClip clip = SelectSound();
        audioSrc.PlayOneShot(clip);

    }

    private AudioClip SelectSound()
    {
        AudioClip clip;
        return clip = slashSounds[UnityEngine.Random.Range(0, slashSounds.Length)];
    } 

    private IEnumerator StartParticles()
    {
        yield return new WaitForSeconds(0.2f);
        //ParticleSystem.EmissionModule em = dashParticles.emission;
        //em.enabled = true;
        //dashParticles.Play();
    }
}
