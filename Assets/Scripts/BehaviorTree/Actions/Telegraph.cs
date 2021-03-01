using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class Telegraph : Action
{
    public Animator animator;
    public string telegraphName;
    private float animTime;
    private BossController boss;
    private float timer = 0;

    public override void OnAwake()
    {
        boss = Manager.instance.boss;

        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach(AnimationClip clip in clips)
        {
            if(clip.name == telegraphName)
            {
                animTime = clip.length;
                break;
                
            }

        }
    }

    public override void OnStart()
    {
        timer = 0;
        
        
    }

    public override TaskStatus OnUpdate()
    {
        Debug.Log("animTimer: " + animTime);
        //Debug.Log("timer:  " + timer);
        boss.enemyRB.velocity = new Vector2(0,0);
        timer += Time.deltaTime;
        if(timer >= animTime)
            return TaskStatus.Success;
        else
            return TaskStatus.Running;
        //set the velocity to 0
    }

    public override void OnEnd()
    {

    }
}
