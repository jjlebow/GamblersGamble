using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class IdleBehavior : Action
{
    private BossController boss;
	private float rand;
	public float min;
	public float max;


    public override TaskStatus OnUpdate()
    {

        return IdleState();
    }

	public override void OnStart()
	{
		boss = Manager.instance.boss;
		boss.bossState = BossController.BossState.IDLE;
		
		rand = Random.Range(min, max);
	}

    private TaskStatus IdleState()
    {
        ///Debug.Log("boss controller we should be movig");
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
				//ChangeIdleDirection();
    		}
    		else if(!boss.facingLeft)
    		{
				//ChangeIdleDirection();
    			boss.WallFlip();
    		}
    	}
    	boss.enemyRB.velocity = boss.idleMoveSpeed * boss.idleMoveDirection;
		rand -= Time.deltaTime;
		if(rand > 0)
			return TaskStatus.Running;
		else
			return TaskStatus.Success;

    }

    private void ChangeDirection()
	{
		boss.goingUp = !boss.goingUp;
		float p = Random.Range(-1.0f, 1.0f);
		//idleMoveDirection = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
		boss.idleMoveDirection = new Vector2(p, boss.idleMoveDirection.y * -1);
		boss.attackMoveDirection.y *= -1;

		if(p > 0 && boss.facingLeft)
		{
			boss.CeilingFlip();
			//facingLeft = !facingLeft;
			//transform.Rotate(0,180,0);
		}
		else if(p < 0 && !boss.facingLeft)
		{
			boss.CeilingFlip();
			//facingLeft = !facingLeft;
			//transform.Rotate(0,180,0);
		}

		
	}
}
