using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class AOEBehavior : Action
{

    private BossController boss;
	private int id;

    public override void OnStart()
    {
        boss = Manager.instance.boss;
        boss.FacePlayer();
		Debug.Log(boss.attackMoveDirection.x * boss.attackMoveSpeed);
		id = LeanTween.move(boss.gameObject, boss.transform.TransformPoint(new Vector3(boss.attackMoveDirection.x * boss.attackMoveSpeed, boss.attackMoveDirection.y * boss.attackMoveSpeed, 0)), 0.7f).setEase(LeanTweenType.easeInQuart).id;
		AOEState();
		
        //Debug.Log("we are entering aoe behavior");
        
        //Debug.Log("this is when on start is called");
    }

    public override TaskStatus OnUpdate()
    {
        AOEState();
		
		
        if(boss.bossState == BossController.BossState.IDLE)
		{
			LeanTween.cancel(id);
            return TaskStatus.Success;
		}
        else
            return TaskStatus.Running;
    }

	public void AOEState()
	{

		if(boss.isTouchingWall)
		{
			//Debug.Log(boss.attackMoveDirection.x * boss.attackMoveSpeed);
			LeanTween.cancel(id);
			boss.WallFlip();
			id = LeanTween.move(boss.gameObject, boss.transform.TransformPoint(new Vector3(boss.attackMoveDirection.x * boss.attackMoveSpeed, boss.attackMoveDirection.y * boss.attackMoveSpeed, 0)), 0.7f).setEase(LeanTweenType.easeInQuart).id;
		}
		if(boss.isTouchingUp && boss.goingUp)
		{
			Debug.Log("we are touching up");
			LeanTween.cancel(id);
			ChangeDirection();
			id = LeanTween.move(boss.gameObject, boss.transform.TransformPoint(new Vector3(boss.attackMoveDirection.x * boss.attackMoveSpeed, boss.attackMoveDirection.y * boss.attackMoveSpeed, 0)), 0.7f).setEase(LeanTweenType.easeInQuart).id;

		}
		else if(boss.isTouchingDown && !boss.goingUp)
		{
			Debug.Log("boss is touching down");
			LeanTween.cancel(id);
			ChangeDirection();
			id = LeanTween.move(boss.gameObject, boss.transform.TransformPoint(new Vector3(boss.attackMoveDirection.x * boss.attackMoveSpeed, boss.attackMoveDirection.y * boss.attackMoveSpeed, 0)), 0.7f).setEase(LeanTweenType.easeInQuart).id;

		}
		
		
		
	}

    public void AOEStated()
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
    	//boss.enemyRB.velocity = boss.attackMoveSpeed * boss.attackMoveDirection;
		
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
