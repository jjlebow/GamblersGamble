using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public int health;

    private bool canDamage = true;
    //public GameObject{?} death animation

    public void TakeCollisionDamage(int damage, string type)
    {
    	if(canDamage)
    	{
    		canDamage = false;
    		StartCoroutine(DamageWait());
    		Debug.Log("The type is: " + type);
    		if(type == "CriticalHitbox")
    		{
    			Debug.Log("we have arrived at critical damage");
    			health -= damage * 2;
    		}
    		else if(type == "NormalHitbox")
    		{
    			health -= damage;
    		}
    		//Debug.Log("damage taken: " + damage);
    		if(health <=0)
    		{	 
    			Debug.Log(this.name + " has died");
    			//destroy the gameobject here the same way we do a dart
    		}
    	}
    }

    public void TakeDamage(int damage)
    {
    	health -= damage;
    	if(health <= 0)
    		Debug.Log(this.name + " has died");
    }
    public IEnumerator DamageWait()
    {
    	yield return new WaitForSeconds(0.02f);
    	canDamage = true;
    }
}
