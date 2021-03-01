using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class ShootBehavior : Action
{

    private BossController boss;

    public override void OnStart()
    {
        boss = Manager.instance.boss;
        boss.FacePlayer();
        boss.Shoot();
    }

    public override TaskStatus OnUpdate()
    {
        if(boss.bossState == BossController.BossState.IDLE)
            return TaskStatus.Success;
        else
            return TaskStatus.Running;
    }


}
