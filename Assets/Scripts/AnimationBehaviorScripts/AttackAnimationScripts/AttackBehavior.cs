using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehavior : StateMachineBehaviour
{
    private PlayerController player;
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(player == null)
            player = animator.GetComponentInParent(typeof(PlayerController)) as PlayerController;   
        //if(StateManager.instance.playerGrounded && StateManager.instance.stance == false) 
            //StateManager.instance.playerState = StateManager.PlayerStates.HOLD;
        if(player.intendedLayer == 0)
        {
            player.neutralHitbox.SetActive(true);
            AudioManager.instance.audioSource.PlayOneShot(AudioManager.instance.neutralAttackSound, 1.0f);
        }
        else if(player.intendedLayer == 1)
        {
            player.heavyHitbox.SetActive(true);
        }
        else if(player.intendedLayer ==2)
        {
            player.precisionHitbox.SetActive(true);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //if(Input.GetKey(KeyCode.K) && !StateManager.instance.stance)
        //{
            //StateManager.instance.attackContinue = true;
        //}
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.neutralHitbox.SetActive(false);
        player.heavyHitbox.SetActive(false);
        player.precisionHitbox.SetActive(false);
        //StateManager.instance.playerState = StateManager.PlayerStates.IDLE;
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
