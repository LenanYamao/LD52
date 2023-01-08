using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheCoccoonPhase1 : StateMachineBehaviour
{
    private int rand;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rand = Random.Range(0, 250);

        if(rand < 50)
        {
            animator.SetTrigger("Idle");
        }
        else if(rand >= 50 && rand < 100)
        {
            animator.SetTrigger("Attack1");
        }
        else if (rand >= 100 && rand < 150)
        {
            animator.SetTrigger("Attack2");
        }
        else if (rand >= 150 && rand < 200)
        {
            animator.SetTrigger("Attack3");
        }
        else if (rand >= 200 && rand < 250)
        {
            animator.SetTrigger("Attack4");
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
