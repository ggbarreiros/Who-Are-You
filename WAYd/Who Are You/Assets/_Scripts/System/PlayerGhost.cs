using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGhost : MonoBehaviour
{
    public SpriteRenderer sprite;
    float timer = 0.25f;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        GameObject spriteGO = GameObject.Find("New Sprite");

        transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
        //transform.localScale = GameObject.FindGameObjectWithTag("Player").transform.localScale;

        //transform.position = spriteGO.transform.position;
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().facingRight)
        {
            transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
        else
        {
            transform.localScale = new Vector3(-1.5f, 1.5f, 1.5f);
        }

        sprite.sprite = spriteGO.GetComponent<SpriteRenderer>().sprite;
        sprite.color = new Vector4(50, 250, 50, 0.2f);

    }

    void Update()
    {
        timer -= Time.deltaTime;
        //Color alphaColor = GetComponent<SpriteRenderer>().color;
        //alphaColor.a = alphaColor.a - 0.1f;
        //sprite.color = new Vector4(sprite.color.r, sprite.color.g, sprite.color.b, alphaColor.a);
        if (timer <=0)
        {
            Destroy(gameObject);
        }
    }
}
