using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralAction : FSMAction
{
    public Animator animator;
    string triggerName;
    string[] finishEvent;
    float rand;


    public NeutralAction(FSMState owner) : base(owner)
    {
    }

    // triggername is the state we transition to, fnishEvent is the next state we transition to, and animtor is just the animator
    public void Init(string triggerName, string[] finishEvent, Animator animator)
    {
        this.triggerName = triggerName;
        this.finishEvent = finishEvent;
        this.animator = animator;
    }

    public override void OnEnter()
    {
        rand = Random.Range(1, 4);
        animator.SetTrigger(triggerName);
        //there needs to be logic here to determine what animation each body 
        //part plays
            //if(StateManager.instance.walking == false)
                //legAnimator.SetTrigger(triggerName);
    }

    public override void OnUpdate()
    {
        if(rand > 0)
        {
            Manager.instance.boss.IdleState();
            rand -= Time.deltaTime;
        }
        else if(rand <= 0)
        {
            Finish(Manager.instance.boss.DecideAttack());
        }
        //if we enter knockback, or if hte attack finishes, then we 
        //set the finishEvent to knockback or whatever and transition there instead
        //and if we are leaving the full animation early, the we change the "attacking variable to 'false' on leaving
        //record the state that existed before it entered this state and set that to finishedEvent
        //if we are exiting into a different state, (knockback), then set finished event to that. 
    }

    public void Finish(int num)
    {
        if(!string.IsNullOrEmpty(finishEvent[num]))
            GetOwner().SendEvent(finishEvent[num]);
    }
}
