using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public GameObject target;
    public float moveSpeed;

    void Update()
    {
        transform.position = Vector2.Lerp(transform.position, target.transform.position, moveSpeed);
    }
}
