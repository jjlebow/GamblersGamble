using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class CloseDistance : Conditional
{
    public BossController boss;
    private PlayerController player;

    public override void OnAwake()
    {
        player = Manager.instance.player;
    }

    public override TaskStatus OnUpdate()
    {
        if(Vector2.Distance(player.transform.position, boss.transform.position) < 5)
            return TaskStatus.Success;
        else
            return TaskStatus.Failure;
    }
}
