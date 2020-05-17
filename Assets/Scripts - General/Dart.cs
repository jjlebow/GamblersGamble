using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dart : MonoBehaviour
{
	public float speed = 5f;
	public Rigidbody2D rb;
	//public GameObject impactEffect; // a prefab in here to make an impact effect or destroy effect


    // Start is called before the first frame update
    void Start()
    {
    	rb.velocity = -transform.right * speed;

    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
    	//play animation for bullet falling out (impact effect);
    	//Destroy(gameObject); destroy this while we are not touching the player
    	//if the thing is damagable, then it takes damage and dies if necessary, playing
    	//its apporpirate death animation and then destorying itself
    	//Instantiate(impactEffect, transform.position, transform.rotation);
    }

}
