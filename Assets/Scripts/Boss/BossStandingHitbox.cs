using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStandingHitbox : MonoBehaviour
{
	public int damage;
	public PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        player = Manager.instance.player;
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
    	//if(hitParent.GetComponent<Damageable>() != null && hitParent.name == "Player")
    	if(hitInfo.gameObject.name == "Torso" || hitInfo.gameObject.name == "Legs")
    	{
    		Debug.Log("the boss should be taking: " + player.damageHolder + " much damage");
    		hitParent.GetComponent<Damageable>().PlayerCollisionDamage(damage, PublicFunctions.FindParent(this.transform).gameObject, hitParent.gameObject);
    	}
        else if(hitInfo.gameObject.tag == "Weapon")
        {
            Debug.Log("HIT WITH A WEAPON");
        }
    }
}
