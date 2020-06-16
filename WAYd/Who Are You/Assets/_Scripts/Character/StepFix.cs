using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepFix : MonoBehaviour
{
    public FootSteps footStepsScript;

    public void FixStep()
    {
        footStepsScript.Step();
    }
}
