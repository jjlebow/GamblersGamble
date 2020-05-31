using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkAction : FSMAction
{

    public Animator animator;
    public Animator legAnimator;
    string triggerName;
    string[] finishEvent;

    // Start is called before the first frame update
    public WalkAction(FSMState owner) : base(owner)
    {
    }

    public void Init(string triggerName, string[] finishEvent, Animator animator, Animator legAnimator)
    {
        this.triggerName = triggerName;
        this.finishEvent = finishEvent;
        this.animator = animator;
        this.legAnimator = legAnimator;
    }

    public override void OnEnter()
    {
        //since this is an indefinite repeated animation, we have a bool
        //that when true, makes the animator play the walking animation until
        //the bool turns false again. No trigger here. See StateController's update
        animator.SetTrigger(triggerName);
        //legAnimator.SetTrigger(triggerName);
    }

    public override void OnUpdate()
    {
        if(StateManager.instance.grounded == false)
            Finish(3);
        if(StateManager.instance.walking == false)
            Finish(0);
        if(StateManager.instance.currentState == StateManager.PlayerState.MELEE)
            Finish(1);
        if(StateManager.instance.currentState == StateManager.PlayerState.SHOOT)
            Finish(4);
    }

    public void Finish(int num)
    {
        if(!string.IsNullOrEmpty(finishEvent[num]))
            GetOwner().SendEvent(finishEvent[num]);
    }
}
