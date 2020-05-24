using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrecisionAttack : MonoBehaviour
{
	public PlayerController player;

	public bool hilt = false;
	public bool tip = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DamageCheck(Collider2D col)
    {
    	if(hilt)  //if both are active it should calculate with hilt damage and skip the tipper damage
    	{
    		Transform hitParent = col.transform;
    		hitParent = PublicFunctions.FindParent(hitParent);
    		//Debug.Log("we are colliding with " + tempName);
    		if(hitParent.GetComponent<Damageable>() != null)
    		{
    			//Debug.Log("the boss should be taking: " + player.damageHolder + " much damage");
    			hitParent.GetComponent<Damageable>().TakeCollisionDamage(player.damageHolder,  col.name);
    			hilt = false;
    		}
    	}
    	else if(tip && !hilt)
    	{
    		Transform hitParent = col.transform;
    		hitParent = PublicFunctions.FindParent(hitParent);
    		//Debug.Log("we are colliding with " + tempName);
    		if(hitParent.GetComponent<Damageable>() != null)
    		{
    			//Debug.Log("the boss should be taking: " + player.damageHolder + " much damage");
    			hitParent.GetComponent<Damageable>().TakeCollisionDamage(player.damageHolder * 2,  col.name);
    			tip = false;
    		}
    	}
    }
}
