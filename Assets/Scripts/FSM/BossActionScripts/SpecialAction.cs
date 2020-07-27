﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAction : FSMAction
{
    public Animator animator;
    string triggerName;
    string[] finishEvent;
    private Vector2 playerPosition;


    public SpecialAction(FSMState owner) : base(owner)
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
    	animator.SetTrigger(triggerName);
    	playerPosition = Manager.instance.player.transform.position - Manager.instance.boss.transform.position;
        //there needs to be logic here to determine what animation each body 
        //part plays
            //if(StateManager.instance.walking == false)
                //legAnimator.SetTrigger(triggerName);
    }

    public override void OnUpdate()
    {
        //if boss hp falls below 0, trigger death
        //if a cancel occurs, move to idle, 
        //else, Finish(Manager.instance.boss.DecideAttack())
    	Manager.instance.boss.SpecialState(playerPosition);
    	if(Manager.instance.boss.isTouchingUp || Manager.instance.boss.isTouchingDown || Manager.instance.boss.isTouchingWall)
    	{
    		Finish(0);  //this will finish at neutral once the animation has run its course
    	}
    }

    public void Finish(int num)
    {
        if(!string.IsNullOrEmpty(finishEvent[num]))
            GetOwner().SendEvent(finishEvent[num]);
    }
}
