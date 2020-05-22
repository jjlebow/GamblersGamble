using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dart : MonoBehaviour
{
	public float speed = 5f;
	public Rigidbody2D rb;
    [HideInInspector] public int damage;
	//public GameObject impactEffect; // a prefab in here to make an impact effect or destroy effect


    // Start is called before the first frame update
    void Start()
    {
    	rb.velocity = -transform.right * speed;

    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        
        Debug.Log("WE COLLIDED WITH SOMETHING");
        if(hitInfo.gameObject.CompareTag("Weapon") == false && hitInfo.gameObject.CompareTag("Player") == false)
        {

            Debug.Log("We collided with something that isnt a weapon or the player");
            if(hitInfo.GetComponent<Damageable>() != null)
            {
                hitInfo.GetComponent<Damageable>().TakeDamage(damage);
            }
            Destroy(gameObject);
            
        }
        else
        {
            /*
            Debug.Log("we are destroying the dart now");
            if(hitInfo.GetComponent<Damageable>() != null)
            {
                Debug.Log("should have taken damage");
                hitInfo.GetComponent<Damageable>().TakeDamage(5);  //5 should be taken from the card...maybe? look into this later
            }
            Destroy(gameObject);  //do this by playy8ing a destruction animation
            */
        }
    	//play animation for bullet falling out (impact effect);
    	//Destroy(gameObject); destroy this while we are not touching the player
    	//if the thing is damagable, then it takes damage and dies if necessary, playing
    	//its apporpirate death animation and then destorying itself
    	//Instantiate(impactEffect, transform.position, transform.rotation);
    }

}
