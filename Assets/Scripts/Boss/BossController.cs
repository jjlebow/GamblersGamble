using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{

	[Header("Idle")]
	public float idleMoveSpeed;
	public Vector2 idleMoveDirection;

	[Header("AttackUpAndDown")]
	public float attackMoveSpeed;
	public Vector2 attackMoveDirection;

	//[Header("AttackPlayer")]
	//public float attackPlayerSpeed;
	private PlayerController player;

	[Header("Other")]
	[SerializeField] Transform groundCheckUp;
	[SerializeField] Transform groundCheckDown;
	[SerializeField] Transform groundCheckWall;
	[SerializeField] float groundCheckRadius;
	[SerializeField] LayerMask groundLayer;
	public bool isTouchingUp;
	public bool isTouchingDown;
	public bool isTouchingWall;
	public bool goingUp = true;
	public Rigidbody2D enemyRB;
	public bool facingLeft = false;
	[HideInInspector] public bool bossActive = false;
	public Transform firePoint;
	public GameObject shotPrefab;
	public int idleAngle;
	private Animator anim;

	private bool deflected = false;

	public GameObject meleeHitbox;
	public GameObject weakPoint;
	public GameObject weakPointEnabler;

	[HideInInspector] public Collider2D hitInfo;

	
	public enum BossState{IDLE, TAOE, AOE, TSHOOT, SHOOT, TMELEE, MELEE, TSPECIAL, SPECIAL, KNOCKBACK}
	public BossState bossState;

	public AudioSource audioSource;
	public AudioClip hitNoise;

	///private Node topNode;

	void Awake()
	{
		Manager.instance.boss = GetComponent<BossController>();
	}

    // Start is called before the first frame update
    void Start()
    {
        player = Manager.instance.player;
        idleMoveDirection.Normalize();
        attackMoveDirection.Normalize();
        enemyRB = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        isTouchingUp = Physics2D.OverlapCircle(groundCheckUp.position, groundCheckRadius, groundLayer);
        isTouchingDown = Physics2D.OverlapCircle(groundCheckDown.position, groundCheckRadius, groundLayer);
        isTouchingWall = Physics2D.OverlapCircle(groundCheckWall.position, groundCheckRadius, groundLayer);

		anim.SetInteger("BossState", (int)bossState);

        //IdleState();
        //AOEState();
        //if(Input.GetKeyDown(KeyCode.H))
        //{
        	//SpecialState();
        //}
        //FlipTowardsPlayer();
    }

	void LateUpdate()
	{
		hitInfo = null;
	}

	public void Shoot()
	{
		GameObject bullet = Instantiate(shotPrefab, firePoint.position, firePoint.rotation);
	}

	public void FacePlayer()
	{
		player.transform.position.Normalize();
		if(player.transform.position.x > transform.position.x && facingLeft)
		{
			WallFlip();
		}
		else if(player.transform.position.x < transform.position.x && !facingLeft)
		{
			WallFlip();
		}
	}

    private void OnDrawGizmoSelected()
    {
    	Gizmos.color = Color.cyan;
    	Gizmos.DrawWireSphere(groundCheckUp.position, groundCheckRadius);
    	Gizmos.DrawWireSphere(groundCheckDown.position, groundCheckRadius);
    	Gizmos.DrawWireSphere(groundCheckWall.position, groundCheckRadius);
    }

	public void CeilingFlip()
	{
		facingLeft = !facingLeft;
		//attackMoveDirection.x *= -1;     //we leave this out since the attack vectors are all relative to boss facing/position instead of global
		transform.Rotate(0,180,0);
	}

    public void WallFlip()
    {
		//Debug.Log("Flipping " + idleMoveDirection.x);
    	facingLeft = !facingLeft;
    	idleMoveDirection.x *= -1;
    	//attackMoveDirection.x *= -1;   //we leave this out since the attack vectors are all relative to boss facing/position instead of global
    	transform.Rotate(0,180,0);
    }

    public int DecideAttack()
    {
    	int rand = Random.Range(0,6);
    	Vector2 playerPos = player.transform.position;
    	if(Mathf.Abs(transform.position.x - playerPos.x) < 3)
    	{
    		if(rand > 2)
    		{
    			return 1;   //melee attack
    		}
    		else if(rand == 2)
    		{
    			return 4;   //Special dive attack;
    		}
    		else if(rand < 2)
    		{
    			return 3;  //AOE attack;
    		}
    	}
    	else
    	{
    		if(rand > 3)
    		{
    			return 2;
    		}
    		else if(rand == 3)
    		{
    			return 3;  
    		}
    		else
    		{
    			return 4; 
    		}
    	}
    	//this is where we decide what attack to use
    	return 0;
    }

}
