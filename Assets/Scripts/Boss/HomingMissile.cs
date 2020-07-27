using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HomingMissile : MonoBehaviour
{
	private Rigidbody2D rb;
	public float speed = 5f;
	private Transform target;
	public float rotateSpeed = 200f;
	public GameObject destroyEffect;

    // Start is called before the first frame update
    void Awake()
    {
  		rb = GetComponent<Rigidbody2D>();
  		target = Manager.instance.player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate() 
    {
    	Vector2 direction = (Vector2)target.position - rb.position;

    	direction.Normalize();

    	float rotateAmount = Vector3.Cross(direction, transform.up).z;

    	rb.angularVelocity = -rotateAmount * rotateSpeed;

    	rb.velocity = transform.up * speed;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
    	//Instantiate(destroyEffect, transform.position, transform.rotation);
    	if(hitInfo.gameObject.CompareTag("Boss") == false)
    		Destroy(gameObject);
    }
}
