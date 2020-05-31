using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAction : FSMAction
{
    public Animator animator;
    public Animator legAnimator;
    string triggerName;
    string[] finishEvent;


    public IdleAction(FSMState owner) : base(owner)
    {
    }

    // triggername is the state we transition to, fnishEvent is the next state we transition to, and animtor is just the animator
    public void Init(string triggerName, string[] finishEvent, Animator animator, Animator legAnimator)
    {
        this.triggerName = triggerName;
        this.finishEvent = finishEvent;
        this.animator = animator;
        this.legAnimator = legAnimator;
    }

    public override void OnEnter()
    {
        //animator.Play(triggerName);
        animator.SetTrigger(triggerName);
        //legAnimator.SetTrigger(triggerName);
    }

    public override void OnUpdate()
    {
        //if we enter knockback, or if hte attack finishes, then we 
        //set the finishEvent to knockback or whatever and transition there instead,
        //and if we are leaving the full animation early, the we change the "attacking variable to 'false' on leaving
        if(StateManager.instance.grounded == false)
        {
            Finish(3);
        }
        if(StateManager.instance.currentState == StateManager.PlayerState.MELEE)
            Finish(1);
        else if(StateManager.instance.walking == true)
            Finish(2);
        else if(StateManager.instance.currentState == StateManager.PlayerState.SHOOT)
            Finish(4);
        //record the state that existed before it entered this state and set that to finishedEvent
        //if we are exiting into a different state, (knockback), then set finished event to that. 
    }

    public void Finish(int num)
    {
        //StateManager.instance.
        if(!string.IsNullOrEmpty(finishEvent[num]))
            GetOwner().SendEvent(finishEvent[num]);
    }



    /*
    public IEnumerator AttackCoroutine()
    {
        //anim.Play("NeutralWarmUp", 0);set trigger to play here
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        Debug.Log("activated hitbox");
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        Debug.Log("deactivated hitbox");
    }
    */


    //use Animator.play. Store the animations in states in the animator
    //window which allows us to use Animator.Play isntead of animation.play
    //which allows easy access to the appropriate animation as well as 
    //giving us the play time of that animation(hence allowing us to wait before the next one plays)
    //may have to do different layers for each of the different weapon types? but they 
    //all follow the same path. (Maybe only connect the attackstart, attack, attackcool so that we dont have to put the actual waits in the code and can just wait in the animator)
}
