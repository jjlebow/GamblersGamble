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

    private bool rising = true;
    private bool falling = false;

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
        //Debug.Log("engertingAirAction");
        //Debug.Log("shoule print once");
        rising = false;
        falling = false;
        if(player.m_Rigidbody2D.velocity.y <= 0)
        {
            animator.SetTrigger(triggerName2);
            //Debug.Log("the trigger has been set");
            falling = true;
            //legAnimator.SetTrigger(triggerName2);
        }
        else
        {
            rising = true;
            animator.SetTrigger(triggerName);
            //legAnimator.SetTrigger(triggerName);
        }
        //no trigger, we use a bool for the airfall and rise
        //we need a trigger for attacks in the air though 
    }

    public override void OnUpdate()
    {
        if(StateManager.instance.grounded == true && StateManager.instance.walking == false)
        {
            //set trigger for landing animation here if you want to add that
            Finish(0);
            //Debug.Log("grounded");
        }
        else if(StateManager.instance.walking == true && StateManager.instance.grounded == true)
        {

            Finish(2);
        }
        else if(StateManager.instance.currentState == StateManager.PlayerState.MELEE)
        {
            //Debug.Log("We are leaving air and going into attack");
            Finish(1);
        }

        else if(StateManager.instance.currentState == StateManager.PlayerState.SHOOT)
        {
            Finish(4);
        }
        //this will reverse the jump while still in midair if we sheft to a positive or negative velocity while still in midair. 
        if((rising == true && player.m_Rigidbody2D.velocity.y <= 0) || (falling == true && player.m_Rigidbody2D.velocity.y > 0))
        {
            Finish(3);
        }

    }

    public void Finish(int num)
    {
        if(!string.IsNullOrEmpty(finishEvent[num]))
            GetOwner().SendEvent(finishEvent[num]);
    }


}
