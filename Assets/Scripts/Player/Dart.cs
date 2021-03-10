using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dart : MonoBehaviour
{
	public float speed;
	public Rigidbody2D rb;         
    [HideInInspector] public int damage;
    public GameObject shooter;
    private Animator animator;
	//public GameObject impactEffect; // a prefab in here to make an impact effect or destroy effect


    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody2D>();
    	rb.velocity = transform.right * speed;
        animator = GetComponent<Animator>();

    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        
        if(hitInfo.gameObject.CompareTag("Weapon") == false && hitInfo.gameObject.CompareTag("Player") == false)
        {
            Transform hitParent = hitInfo.transform;
            /*
            while(true)
            {
                if(hitParent.parent != null)
                    hitParent = hitParent.parent;
                else
                    break;
            }
            */
            rb.velocity = new Vector3(0,0,0);
            hitParent = PublicFunctions.FindParent(hitParent);
            if(hitParent.GetComponent<Damageable>() != null)
            {
                hitParent.GetComponent<Damageable>().TakeCollisionDamage(damage, hitInfo.name, shooter);  //this uses the player and the collision in the damage formula
            }
            animator.SetTrigger("Despawn");
        }
        
    	//play animation for bullet falling out (impact effect);
    	//Destroy(gameObject); destroy this while we are not touching the player
    	//if the thing is damagable, then it takes damage and dies if necessary, playing
    	//its apporpirate death animation and then destorying itself
    	//Instantiate(impactEffect, transform.position, transform.rotation);
    }

}
