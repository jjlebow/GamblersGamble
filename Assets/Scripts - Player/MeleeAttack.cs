using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
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
    	Debug.Log("we are colliding with " + hitInfo);
    	if(hitInfo.GetComponent<Damageable>() != null)
    	{
    		Debug.Log("the boss should be taking: " + player.damageHolder + " much damage");
    		hitInfo.GetComponent<Damageable>().TakeDamage(player.damageHolder);
    	}
    }
}
