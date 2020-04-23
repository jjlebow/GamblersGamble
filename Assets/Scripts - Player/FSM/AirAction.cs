using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirAction : FSMAction
{
    public Animator animator;
    public Animator legAnimator;

    string triggerName;
    string triggerName2;
    string[] finishEvent;
    PlayerController player;

    public AirAction(FSMState owner) : base(owner)
    {

    }

    public void Init(string triggerName, string triggerName2, string[] finishEvent, Animator animator, Animator legAnimator, PlayerController player)
    {
        this.triggerName = triggerName;
        this.finishEvent = finishEvent;
        this.animator = animator;
        this.legAnimator = legAnimator;
        this.player = player;
        this.triggerName2 = triggerName2;
    }

    public override void OnEnter()
    {
        Debug.Log("engertingAirAction");
        //Debug.Log("shoule print once");
        if(player.m_Rigidbody2D.velocity.y <= 0)
        {
            animator.SetTrigger(triggerName2);
            Debug.Log("the trigger has been set");
            //legAnimator.SetTrigger(triggerName2);
        }
        else
        {
            animator.SetTrigger(triggerName);
            //legAnimator.SetTrigger(triggerName);
        }
        //no trigger, we use a bool for the airfall and rise
        //we need a trigger for attacks in the air though 
    }

    public override void OnUpdate()
    {
        if(StateManager.instance.grounded == true)
        {
            //set trigger for landing animation here if you want to add that
            Finish(0);
            Debug.Log("grounded");
        }
        else if(StateManager.instance.isActive == true)
        {
            Debug.Log("We are leaving air and going into attack");
            Finish(1);
        }

    }

    public void Finish(int num)
    {
        if(!string.IsNullOrEmpty(finishEvent[num]))
            GetOwner().SendEvent(finishEvent[num]);
    }


}
