using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class MeleeBehavior : Action
{

    private BossController boss;

    public override void OnStart()
    {
        boss = Manager.instance.boss;
        boss.enemyRB.velocity = new Vector2(0,0);
        boss.meleeHitbox.SetActive(true);
        //boss.FacePlayer();
        //Debug.Log("we are entering aoe behavior");
        
        //Debug.Log("this is when on start is called");
    }

    public override TaskStatus OnUpdate()
    {
        if(boss.bossState == BossController.BossState.IDLE)
            return TaskStatus.Success;
        else
            return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        boss.meleeHitbox.SetActive(false);
    }
}
