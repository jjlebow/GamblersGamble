using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class ShootTelegraph : Action
{
    private BossController boss;

    public override void OnAwake()
    {
        boss = Manager.instance.boss;
    }

    public override void OnStart()
    {
        boss.bossState = BossController.BossState.TSHOOT;
        boss.enemyRB.velocity = new Vector2(0,0);
        
        
    }

    public override TaskStatus OnUpdate()
    {
        if(boss.bossState == BossController.BossState.SHOOT)
            return TaskStatus.Success;
        else
            return TaskStatus.Running;
        //set the velocity to 0
    }
}
