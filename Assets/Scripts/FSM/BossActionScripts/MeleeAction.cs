using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAction : FSMAction
{
    public Animator animator;
    string triggerName;
    string[] finishEvent;


    public MeleeAction(FSMState owner) : base(owner)
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
        //Debug.Log("Melee Action");
        animator.SetTrigger(triggerName);
        Manager.instance.boss.enemyRB.velocity = Vector2.zero;
        Manager.instance.boss.bossActive = true;
        //there needs to be logic here to determine what animation each body 
        //part plays
            //if(StateManager.instance.walking == false)
                //legAnimator.SetTrigger(triggerName);
    }

    public override void OnUpdate()
    {
        if(Manager.instance.boss.bossActive == false)
        {
            Manager.instance.boss.meleeHitbox.SetActive(false);
            Finish(0);  //this will finish at neutral once the animation has run its course
        } 
    }

    public void Finish(int num)
    {
        if(!string.IsNullOrEmpty(finishEvent[num]))
            GetOwner().SendEvent(finishEvent[num]);
    }
}
