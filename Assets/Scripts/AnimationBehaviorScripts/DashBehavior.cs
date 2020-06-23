using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashBehavior : StateMachineBehaviour
{
    private PlayerController player;
    private float length;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(player == null)
            player = animator.GetComponentInParent(typeof(PlayerController)) as PlayerController;
            //player = animator.GetComponent<PlayerController>();
        length = stateInfo.length;
        if(StateManager.instance.faceRight)
            player.m_Rigidbody2D.velocity = new Vector3(20,0,0);
        else
            player.m_Rigidbody2D.velocity = new Vector3(-20,0,0);
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(stateInfo.normalizedTime >= length)
        {
            StateManager.instance.ChangeState(StateManager.PlayerState.IDLE);
        }
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        StateManager.instance.playerStatic = false;
        player.m_Rigidbody2D.gravityScale = 1f;
        //player.m_Rigidbody2D.velocity = new Vector3(0,0,0);

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
