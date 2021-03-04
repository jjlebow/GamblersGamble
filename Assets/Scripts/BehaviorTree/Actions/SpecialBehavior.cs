using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class SpecialBehavior : Action
{

    private BossController boss;
    private Vector2 playerPos;
    private int id;

    public override void OnStart()
    {
        boss = Manager.instance.boss;
        boss.FacePlayer();
        playerPos = Manager.instance.player.ceilingCheck.position;// - boss.transform.position;   //aims at the players head to lessen collisions with the environment this second part is if we dont want to leantween
        playerPos.Normalize();
        id = LeanTween.move(boss.gameObject, new Vector2(playerPos.x * 10, playerPos.y * 10), 0.7f).setEase(LeanTweenType.easeInQuart).id; 
        //boss.enemyRB.velocity = playerPos * boss.attackPlayerSpeed;  //this is our backup option if we dont want to leantween this
        //Debug.Log("we are entering aoe behavior");
        
        //Debug.Log("this is when on start is called");
    }

    public override TaskStatus OnUpdate()
    {
        //Debug.Log(boss.hitInfo.name);
        if(boss.hitInfo != null && boss.bossState == BossController.BossState.SPECIAL)
        {
            if(boss.hitInfo.tag == "Ground") //if we collide with ground
            {
                LeanTween.cancel(id);
                //Debug.Log("this is hitting the ground");
                return TaskStatus.Success;
            }
            if(boss.hitInfo.gameObject.tag == "Player")
            {
                LeanTween.cancel(id);
                //Debug.Log("we are hitting the player");
                return TaskStatus.Success;
            }
            //else if we collide with player weapon
                //return true and move to the next node which will reveal weakpoint
            else if(boss.hitInfo.gameObject.tag == "Weapon")
            {
                LeanTween.cancel(id);
                //Debug.Log("we are hitting a weapon");
                //boss.WallFlip();
                boss.enemyRB.velocity = new Vector2(0,0);
                boss.weakPoint.SetActive(true);
                boss.bossState = BossController.BossState.KNOCKBACK;
                if(boss.transform.position.x < Manager.instance.player.transform.position.x)
                    LeanTween.move(boss.gameObject, new Vector2(boss.transform.position.x - 1, boss.transform.position.y), 0.5f).setEase(LeanTweenType.easeOutSine);
                else
                    LeanTween.move(boss.gameObject, new Vector2(boss.transform.position.x + 1, boss.transform.position.y), 0.5f).setEase(LeanTweenType.easeOutSine);
                return TaskStatus.Running;
                //return TaskStatus.Success;
            }
        }
        if(boss.bossState == BossController.BossState.IDLE)
        {
            boss.weakPoint.SetActive(false);
            LeanTween.cancel(id);
            return TaskStatus.Success;
        }
        boss.hitInfo = null;
        return TaskStatus.Running;
    }


    


}
