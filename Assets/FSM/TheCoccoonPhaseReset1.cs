using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheCoccoonPhaseReset1 : StateMachineBehaviour
{
    public float timer = 2f;
    private float internalTimer = 0f;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        internalTimer = timer;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (internalTimer <= 0)
        {
            animator.SetTrigger("Phase1");
        }
        else
        {
            internalTimer -= Time.deltaTime;
        }
    }
}
