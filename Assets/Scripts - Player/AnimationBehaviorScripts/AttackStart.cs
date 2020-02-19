﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStart : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //StateManager.instance.attackInitiate = false;
        //Debug.Log("we have entered this part");
        //StateManager.instance.isAttacking = true;
        StateManager.instance.attackContinue = false;
        if(StateManager.instance.playerGrounded && StateManager.instance.stance == false) 
            StateManager.instance.playerState = StateManager.PlayerStates.HOLD;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        StateManager.instance.playerState = StateManager.PlayerStates.IDLE;
        StateManager.instance.attackInitiate = false;    
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
