﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStandingHitbox : MonoBehaviour
{
	public int damage;
	public PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter2D(Collider2D hitInfo)
    {
    	Transform hitParent = hitInfo.transform;
    	hitParent = PublicFunctions.FindParent(hitParent);
    	//Debug.Log("we are colliding with " + tempName);
    	if(hitParent.GetComponent<Damageable>() != null && hitParent.name == "Player")
    	{
    		Debug.Log("the boss should be taking: " + player.damageHolder + " much damage");
    		hitParent.GetComponent<Damageable>().PlayerCollisionDamage(damage, this.transform.position, hitInfo.transform.position);
    	}
    }
}