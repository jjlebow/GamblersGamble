using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [Range(0,100)] public int health;
    //this is to prevent multiple hitboxes from being hit at once
    private bool collisionDamageEventRaised = false;
    private float timer = 2.0f;  //timer before player can take damage again after being hit
    //public GameObject{?} death animation
    public bool isDrainable;
    int oldHealth;
    public IEnumerator temp = null;
    private Dictionary<string, int> hitboxesHit = null;   //dictionary is not really necessary...
    private bool canDamage = true;

    private const float WAITEDSECONDS = 0.03f;


    

    //this is the damage function for any collision that isn't the player taking damage (boss collisions)  
    public void TakeCollisionDamage(int baseDamage, string type, GameObject attacker)
    {
        if(!collisionDamageEventRaised)
        {
            hitboxesHit = new Dictionary<string, int>();
            collisionDamageEventRaised = true;
            StartCoroutine(CalculateDamage(attacker));
        }
        //canDamage = false;
        //StartCoroutine(DamageWait());
        //Debug.Log("The type is: " + type);
        hitboxesHit[type] = baseDamage;
    }

    public void CalculateSingleDamage(int baseDamage, string type, GameObject attacker)
    {
        if(type == "CriticalHitbox")
        {
            Debug.Log("critical damage: " + baseDamage * 2);
            oldHealth = health;
            health -= baseDamage * 2;
            if(isDrainable)
                attacker.GetComponent<Damageable>().health += ((oldHealth - health) * 2);
        }
        else
        {
            Manager.instance.boss.audioSource.PlayOneShot(Manager.instance.boss.hitNoise, 1f);
            Debug.Log("Normal Damage: " + baseDamage);
            oldHealth = health;
            health -= baseDamage;
            if(isDrainable)
                attacker.GetComponent<Damageable>().health += (oldHealth - health);
        }
        if(health <=0)
        {	 
            Debug.Log(this.name + " has died");
            //destroy the gameobject here the same way we do a dart
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
            
            //Debug.Log(health);
            //Debug.Log(oldHealth);
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
        //Debug.Log("routine ended");

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



    //this groups up the normalhitbox hit with the criticalEnabler hitbox. The critical enabler 
    //then activates the critical hitbox so that it can be collected in a separate event. This 
    //makes the critical collision its own event at first, so it doesnt register as a critical hit, 
    //but then groups it with the normal hitbox in any subsequent hits which will allow critical damage to happen. 
    //any group that does not contain normalhitbox is not counted as a hit. 

    //Used for enabling a critical hitbox upon hitting a
    //normal hitbox AND could also be used for hitboxes that overlap with normal hitboxes. Generally for melee.
    public IEnumerator CalculateDamage(GameObject attacker)
    {
        //Debug.Log("calling calculate damage");
    	yield return new WaitForSeconds(WAITEDSECONDS);  //this is waiting on top to collect all hitboxes that were hit by the player during the attack (some computers might run into a bug where they dont process frames fast enough given this short time)
        if(!hitboxesHit.ContainsKey("NormalHitbox"))
        {
            collisionDamageEventRaised = false;  //this handles scenarios where subsequent hitboxes that were activated are hit in the same action
            //Debug.Log("thorwing away calculate damage");
            yield break;
        }
        if(hitboxesHit.ContainsKey("CriticalHitbox"))
        {
            Debug.Log("critical damage: " + hitboxesHit["CriticalHitbox"] * 2);
            oldHealth = health;
            health -= hitboxesHit["CriticalHitbox"] * 2;
            if(isDrainable)
                attacker.GetComponent<Damageable>().health += ((oldHealth - health) * 2);
        }
        else if(hitboxesHit.ContainsKey("CriticalEnablerHitbox"))
        {
            //Debug.Log("enabling critical hitbox");
            Manager.instance.boss.weakPoint.SetActive(true);
            Manager.instance.boss.audioSource.PlayOneShot(Manager.instance.boss.hitNoise, 1f);
            Debug.Log("Normal (Enabler) Damage: " + hitboxesHit["NormalHitbox"]);
            oldHealth = health;
            health -= hitboxesHit["NormalHitbox"];
            if(isDrainable)
                attacker.GetComponent<Damageable>().health += (oldHealth - health);
        }
        else
        {
            Manager.instance.boss.audioSource.PlayOneShot(Manager.instance.boss.hitNoise, 1f);
            Debug.Log("Normal Damage: " + hitboxesHit["NormalHitbox"]);
            oldHealth = health;
            health -= hitboxesHit["NormalHitbox"];
            if(isDrainable)
                attacker.GetComponent<Damageable>().health += (oldHealth - health);
        }
        //Debug.Log("damage taken: " + damage);
        if(health <=0)
        {	 
            Debug.Log(this.name + " has died");
            //destroy the gameobject here the same way we do a dart
        }
        collisionDamageEventRaised = false;
    }

    public IEnumerator KnockbackRoutine(Vector3 offender, Vector3 player)
    {
        var temp = timer;
        Vector3 knockbackDir = new Vector3(0,0,0);
        Vector3 moveDir = (offender - player).normalized;
        //Debug.Log("routine started");

        this.GetComponent<PlayerController>().m_Rigidbody2D.velocity = new Vector3(0,0,0); //setting current velocity to 0
        this.GetComponent<PlayerController>().m_Rigidbody2D.angularVelocity = 0f;
        //Debug.Log(this.GetComponent<PlayerController>().m_Rigidbody2D.velocity);
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
                //Debug.Log("running");
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
