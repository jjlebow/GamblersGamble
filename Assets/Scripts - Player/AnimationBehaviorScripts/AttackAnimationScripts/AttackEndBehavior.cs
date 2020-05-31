using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEndBehavior : StateMachineBehaviour
{
    private float length;
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        length = stateInfo.length;
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(stateInfo.normalizedTime >= length)
        {
            //StateManager.instance.isActive = false;
            StateManager.instance.ChangeState(StateManager.PlayerState.IDLE);
            StateManager.instance.playerStatic = false;
        }

        //Debug.Log(StateManager.instance.isActive);
        //if(Input.GetKey(KeyCode.K))
        //{
            //StateManager.instance.attackContinue = true;
        //look into state info here and disable the isActive once the animation stops playing. This way you can transition to the next animation
        //before exiting this state
        //}
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        //if(StateManager.instance.attackContinue == false)
            //StateManager.instance.attackCooldown = false;
        //Debug.Log(StateManager.instance.isActive);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
