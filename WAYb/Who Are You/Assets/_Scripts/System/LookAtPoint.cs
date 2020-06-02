using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPoint : MonoBehaviour
{
    public GameObject other;
    public int direction;

    void Update()
    {
        Vector3 dir = other.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        if(angle < 0)
        {
            angle = angle + 360.5f;
        }

        direction = Mathf.RoundToInt(angle / 51.428f);
    }
}
