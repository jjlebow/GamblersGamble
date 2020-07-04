using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackAction : FSMAction
{
    public Animator animator;
    public Animator legAnimator;
    string triggerName;
    string triggerName2;
    string[] finishEvent;
    PlayerController player;


    public KnockbackAction(FSMState owner) : base(owner)
    {
    }

    // triggername is the state we transition to, fnishEvent is the next state we transition to, and animtor is just the animator
    public void Init(string triggerName, string[] finishEvent, Animator animator, Animator legAnimator, PlayerController player)
    {
        this.triggerName = triggerName;
        this.finishEvent = finishEvent;
        this.animator = animator;
        this.legAnimator = legAnimator;
        this.player = player;
    }

    public override void OnEnter()
    {
        //there needs to be logic here to determine what animation each body 
        //part plays
        animator.SetTrigger(triggerName);
    }

    public override void OnUpdate()
    {
        //if we enter knockback, or if hte attack finishes, then we 
        //set the finishEvent to knockback or whatever and transition there instead,
        //and if we are leaving the full animation early, the we change the "attacking variable to 'false' on leaving
        if(StateManager.instance.currentState == StateManager.PlayerState.CANCEL)
        {
            player.GetComponent<Damageable>().StopKnockback();  //this line still needs to be tested
            Finish(0);
        }
        if(StateManager.instance.currentState != StateManager.PlayerState.KNOCKBACK)
        {
        	if(StateManager.instance.currentState == StateManager.PlayerState.MELEE)
            	Finish(1);
            if(StateManager.instance.currentState == StateManager.PlayerState.DASH)
            	Finish(6);
            if (StateManager.instance.currentState == StateManager.PlayerState.SHOOT)
            	Finish(4);
        	if(StateManager.instance.grounded == false)
            	Finish(3);  //go to air state
        	if(StateManager.instance.walking == true)
            	Finish(2);  //go to walk state
        	if(StateManager.instance.walking == false)
            	Finish(0);   //go to idle state
            
            
            
        //record the state that existed before it entered this state and set that to finishedEvent
        //if we are exiting into a different state, (knockback), then set finished event to that. 
        }
    }

    public void Finish(int num)
    {
        if(!string.IsNullOrEmpty(finishEvent[num]))
            GetOwner().SendEvent(finishEvent[num]);
    }
}

/*legs animator is always in real time with the state updates. 
There is no exit time on leg animations. Upper body animations will 
(almost) always have to finish their animation before moving onto
the next animation, whereas leg stuff will be ahead. (unless its a top priority
transition like knockback).
NO COOLDOWN. the next action can be initiated immediatlely after the last animation ends
*/
