using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class MeleeTelegraph : Action
{
    private BossController boss;

    public override void OnAwake()
    {
        boss = Manager.instance.boss;
    }

    public override void OnStart()
    {
        boss.bossState = BossController.BossState.TMELEE;
        boss.enemyRB.velocity = new Vector2(0,0);
        
        
    }

    public override TaskStatus OnUpdate()
    {
        if(boss.bossState == BossController.BossState.MELEE)
            return TaskStatus.Success;
        else
            return TaskStatus.Running;
        //set the velocity to 0
    }
}
