using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrecisionShot : MonoBehaviour
{
	public float speed = 5f;
	public Rigidbody2D rb;
	[HideInInspector] public int damage;
	private bool canHit;
	public bool collision = false;
	public GameObject shooter;
    // Start is called before the first frame update
    void Awake()
    {
    	
        rb.velocity = -transform.right * speed;
        //player = PublicFunctions.FindParent(this.transform).gameObject.GetComponent<PlayerController>();
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
    
    	if(StateManager.instance.charging == false && collision == false)
    	{
    		Destroy(gameObject);
    	}
    }

    // Update is called once per frame
    void OnTriggerStay2D(Collider2D hitInfo)
    {
    	collision = true;
        if(hitInfo.gameObject.CompareTag("Weapon") == false && hitInfo.gameObject.CompareTag("Player") == false && StateManager.instance.charging == false)
        {
        	Transform hitParent = hitInfo.transform;
        	Debug.Log("we are here");
        	hitParent = PublicFunctions.FindParent(hitParent);
        	if(hitParent.GetComponent<Damageable>() != null && StateManager.instance.charging == false)
        	{
        		hitParent.GetComponent<Damageable>().TakeCollisionDamage(damage, hitInfo.name, shooter);
        		Destroy(gameObject);
        	}
        	else
        		Destroy(gameObject);
        }
    }
}
