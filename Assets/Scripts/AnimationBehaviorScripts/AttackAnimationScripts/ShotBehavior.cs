using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotBehavior : StateMachineBehaviour
{
    private PlayerController player;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(player == null)
        {
            player = animator.GetComponentInParent(typeof(PlayerController)) as PlayerController;
        }
        if(player.intendedLayer == 0)
        {
            AudioManager.instance.audioSource.PlayOneShot(AudioManager.instance.knifeThrowSound, 1.0f);
            GameObject dart = Instantiate(player.bulletPrefab, player.firePoint.position, player.firePoint.rotation);
            dart.transform.parent = player.transform;  //this connects it to the player for damage reasons
            dart.GetComponent<Dart>().damage = player.damageHolder;
            //dart.GetComponent<Dart>().shooter = player.gameObject;
        }
        else if(player.intendedLayer == 1)
        {
            player.StartHeavyCoroutineShot();
        }
        else if(player.intendedLayer == 2)
        {
            //player.charging = true;
            //Debug.Log("instantiating");
            GameObject card = Instantiate(player.cardPrefab, player.firePoint.position, player.firePoint.rotation);
            //card.transform.parent = player.transform;
            card.GetComponent<PrecisionShot>().damage = player.damageHolder;
            card.GetComponent<PrecisionShot>().shooter = player.gameObject;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {    
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
