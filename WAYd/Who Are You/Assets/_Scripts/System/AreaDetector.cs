using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDetector : MonoBehaviour
{
    public string areaID;
    public FootSteps footStepScript;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Feet")
        {
            footStepScript = other.GetComponent<FootSteps>();
            footStepScript.actualArea = areaID;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "Feet")
        {
            footStepScript = other.GetComponent<FootSteps>();
            footStepScript.actualArea = null;
        }
    }

}
