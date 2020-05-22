using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public int health;
    //public GameObject{?} death animation

    public void TakeDamage(int damage)
    {
    	Debug.Log("damage taken: " + damage);
    	health -= damage;
    	if(health <=0)
    	{
    		//destroy the gameobject here the same way we do a dart
    	}
    }
}
