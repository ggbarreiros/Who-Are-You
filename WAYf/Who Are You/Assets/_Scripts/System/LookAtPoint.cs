using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPoint : MonoBehaviour
{
    public GameObject other;
    public int direction;
    public int directionId;

    /// <summary>
    /// 0 - Cima (1, 2)
    /// 1 - Meio (3, 4, 7, 0)
    /// 2 - Baixo (5, 6)
    /// </summary>

    void Update()
    {
        Vector3 dir = other.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        if(angle < 0)
        {
            angle = angle + 360.5f;
        }

        direction = Mathf.RoundToInt(angle / 51.428f);

        #region aqui a besteira começa
        if (direction == 1 || direction == 2)
        {
            directionId = 0;
        }
        else if(direction == 5 || direction == 6)
        {
            directionId = 2;
        }
        else
        {
            directionId = 1;
        }
        #endregion
    }
}
