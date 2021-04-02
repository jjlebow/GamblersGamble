using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    //private Collider2D thisCollider;
    
	public PlayerController player;

    private LayerMask enemyLayer;
    private ContactFilter2D contactFilter = new ContactFilter2D();   //if you want to understand contact filter, make this variable public and look at it from inspector. 
    private Collider2D thisCollider;
    private Collider2D[] hitBuffer = new Collider2D[16];   //16 should be good enough for initialization. dont expect to hold more than 16 at once. we ont actually use this but its for the function (formality)

    void Awake()
    {
        thisCollider = gameObject.GetComponent<Collider2D>();
        enemyLayer = LayerMask.GetMask("Enemy");
        contactFilter.SetLayerMask(enemyLayer);
        contactFilter.useTriggers = true;
        contactFilter.useLayerMask = true;
        
        //Debug.Log(gameObject.GetComponent<Collider2D>());
    }


    public void OnTriggerEnter2D(Collider2D hitInfo)
    {
        
    	Transform hitParent = hitInfo.transform;
    	hitParent = PublicFunctions.FindParent(hitParent);

    	if(hitParent.GetComponent<Damageable>() != null && hitInfo.tag != "Player")
    	{
            int collisionNumber = thisCollider.OverlapCollider(contactFilter, hitBuffer);
            if(collisionNumber == 1)
                hitParent.GetComponent<Damageable>().CalculateSingleDamage(player.damageHolder, hitInfo.name, player.gameObject);
            else if(collisionNumber > 1)
                hitParent.GetComponent<Damageable>().TakeCollisionDamage(player.damageHolder, hitInfo.name, player.gameObject);
    		//Debug.Log("the boss should be taking: " + player.damageHolder + " much damage");
            //Debug.Log("this is the hitbox we are hitting: " + hitInfo.name);
            
    		
    	}
        
    }
}
