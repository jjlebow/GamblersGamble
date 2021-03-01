using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class AOETelegraph : Action
{
    private BossController boss;

    public override void OnAwake()
    {
        boss = Manager.instance.boss;
    }

    public override void OnStart()
    {
        boss.bossState = BossController.BossState.TAOE;
        boss.enemyRB.velocity = new Vector2(0,0);
        
        
    }

    public override TaskStatus OnUpdate()
    {
        if(boss.bossState == BossController.BossState.AOE)
            return TaskStatus.Success;
        else
            return TaskStatus.Running;
        //set the velocity to 0
    }
}
