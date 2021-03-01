using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class AOEBehavior : Action
{

    private BossController boss;

    public override void OnStart()
    {
        boss = Manager.instance.boss;
        boss.FacePlayer();
        //Debug.Log("we are entering aoe behavior");
        
        //Debug.Log("this is when on start is called");
    }

    public override TaskStatus OnUpdate()
    {
        AOEState();
        if(boss.bossState == BossController.BossState.IDLE)
            return TaskStatus.Success;
        else
            return TaskStatus.Running;
    }

    public void AOEState()
    {
    	if(boss.isTouchingUp && boss.goingUp)
    	{
    		ChangeDirection();
    	}
    	else if(boss.isTouchingDown && !boss.goingUp)
    	{
    		ChangeDirection();
    	}

    	if(boss.isTouchingWall)
    	{
    		if(boss.facingLeft)
    		{
    			boss.WallFlip();
    		}
    		else if(!boss.facingLeft)
    		{
    			boss.WallFlip();
    		}
    	}
        //Debug.Log("we are in the aoe state");
    	boss.enemyRB.velocity = boss.attackMoveSpeed * boss.attackMoveDirection;
    }

    private void ChangeDirection()
    {
    	boss.goingUp = !boss.goingUp;
    	boss.idleMoveDirection.y *= -1;
		//put some kind of pause and squishing effect here. 
    	boss.attackMoveDirection.y *= -1;
		if(boss.idleMoveDirection.x > 0 && boss.facingLeft)
		{
			boss.CeilingFlip();
			//facingLeft = !facingLeft;
			//transform.Rotate(0,180,0);
		}
		else if(boss.idleMoveDirection.x < 0 && !boss.facingLeft)
		{
			boss.CeilingFlip();
			//facingLeft = !facingLeft;
			//transform.Rotate(0,180,0);
		}
    }


}
