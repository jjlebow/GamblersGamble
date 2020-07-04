using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [Range(0,100)] public int health;
    //this is to prevent multiple hitboxes from being hit at once
    private bool canDamage = true;
    private float timer = 2.0f;  //timer before player can take damage again after being hit
    //public GameObject{?} death animation
    public bool isDrainable;
    int oldHealth;
    public IEnumerator temp = null;

    //this is the damage function for any collision that isn't the player taking damage
    public void TakeCollisionDamage(int damage, string type, GameObject attacker)
    {
    	if(canDamage)
    	{

    		canDamage = false;
    		StartCoroutine(DamageWait());
    		Debug.Log("The type is: " + type);
    		if(type == "CriticalHitbox")
    		{
    			Debug.Log("we have arrived at critical damage");
                oldHealth = health;
    			health -= damage * 2;
                if(isDrainable)
                    attacker.GetComponent<Damageable>().health += ((oldHealth - health) * 2);
    		}
    		else if(type == "NormalHitbox")
    		{
                oldHealth = health;
    			health -= damage;
                if(isDrainable)
                    attacker.GetComponent<Damageable>().health += (oldHealth - health);
    		}
    		//Debug.Log("damage taken: " + damage);
    		if(health <=0)
    		{	 
    			Debug.Log(this.name + " has died");
    			//destroy the gameobject here the same way we do a dart
    		}
    	}
    }

    //this is the damage function if the player takes collision damage
    public void PlayerCollisionDamage(int damage, GameObject offender, GameObject player)
    {
        //Debug.Log(offender.name);
        //Debug.Log(player.name);
        if(canDamage)
        {
            oldHealth = health;
            health -= damage;
            
            Debug.Log(health);
            Debug.Log(oldHealth);
            if(isDrainable)
                offender.GetComponent<Damageable>().health += (oldHealth - health);
            canDamage = false;
            temp = KnockbackRoutine(offender.transform.position, player.transform.position);
            StartCoroutine(temp);
            if(health <= 0)
            {
                StateManager.instance.ChangeState(StateManager.PlayerState.DEAD);
                Debug.Log(this.name + " has died");
            }
        }
    }

    public void StopKnockback()
    {
        StopCoroutine(temp);
    }

    public void TakeDamage(int damage)
    {
    	health -= damage;
    	if(health <= 0)
    		Debug.Log(this.name + " has died");
    }

    public void Heal(int amount)
    {
        if(health + amount < 100)
            health += amount;
        else 
            health = 100;
    }




    public IEnumerator DamageWait()
    {
    	yield return new WaitForSeconds(0.02f);
    	canDamage = true;
    }

    public IEnumerator KnockbackRoutine(Vector3 offender, Vector3 player)
    {
        var temp = timer;
        Vector3 knockbackDir = new Vector3(0,0,0);
        Vector3 moveDir = (offender - player).normalized;

        this.GetComponent<PlayerController>().m_Rigidbody2D.velocity = new Vector3(0,0,0); //setting current velocity to 0
        
        if(moveDir.x <= 0)
            knockbackDir = new Vector3(2,4, 0);
        else
            knockbackDir = new Vector3(-2,4, 0);
        
        StateManager.instance.playerStatic = true;
        StateManager.instance.ChangeState(StateManager.PlayerState.KNOCKBACK);
        //try to find a knockback equation that works here the rest of the physics here should work
        
        this.GetComponent<PlayerController>().m_Rigidbody2D.velocity = knockbackDir;  //this sets the knockback direction

        //this is the waiting between the time that we are immobilizzed from knockback and the time 
        //that we are invincible
        while(temp > 0)
        {
            while(temp > timer * 0.6)
            {
                temp -= Time.deltaTime;
                //this.GetComponent<PlayerController>().m_Rigidbody2D.velocity = knockbackDir;
                //this.GetComponent<PlayerController>().m_Rigidbody2D.AddForce(knockbackDir);
                yield return null;
            }
            StateManager.instance.playerStatic = false;
            StateManager.instance.ChangeState(StateManager.PlayerState.IDLE);
            temp -= Time.deltaTime;
            //play invincible animation during this time
            yield return null;
        }

        canDamage = true;
        
    }
}
