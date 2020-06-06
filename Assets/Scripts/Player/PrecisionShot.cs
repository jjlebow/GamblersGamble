using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrecisionShot : MonoBehaviour
{
	public float speed = 5f;
	public Rigidbody2D rb;
	[HideInInspector] public int damage;
	private bool canHit;
	private float hitTimer;
	public PlayerController player;
    // Start is called before the first frame update
    void Awake()
    {
    	
        rb.velocity = -transform.right * speed;
        //player = PublicFunctions.FindParent(this.transform).gameObject.GetComponent<PlayerController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        hitTimer = player.recordedTime;
    }

    void Update()
    {
    	if(player.charging == false)
    	{
    		hitTimer -= Time.deltaTime;
    	}
    	if(hitTimer <= 0)
    	{
    		canHit = true;
    		StartCoroutine(HitboxActive);
    	}

    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if(hitInfo.gameObject.CompareTag("Weapon") == false && hitInfo.gameObject.CompareTag("Player") == false)
        {
        	Transform hitParent = hitInfo.transform;

        	hitParent = PublicFunctions.FindParent(hitParent);
        	if(hitParent.GetComponent<Damageable>() != null && canHit == true)
        	{
        		hitParent.GetComponent<Damageable>().TakeCollisionDamage(damage, hitInfo.name, PublicFunctions.FindParent(this.transform).gameObject);
        		Destroy(gameObject);
        	}
        }
    }

    //this ensures that after the object is activated for 0.2 seconds it always gets destroyed after, if it didnt hit anything
    private IEnumerator HitboxActive()
    {
    	yield return WaitForSeconds(0.2f);
    	Destroy(gameObject);
    }
}
