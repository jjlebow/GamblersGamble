using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{

	[Header("Idle")]
	[SerializeField] float idleMoveSpeed;
	[SerializeField] Vector2 idleMoveDirection;

	[Header("AttackUpAndDown")]
	[SerializeField] float attackMoveSpeed;
	[SerializeField] Vector2 attackMoveDirection;

	[Header("AttackPlayer")]
	[SerializeField] float attackPlayerSpeed;
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
	private bool goingUp = true;
	public Rigidbody2D enemyRB;
	private bool facingLeft = false;
	[HideInInspector] public bool bossActive = false;
	public Transform firePoint;
	public GameObject shotPrefab;

	public GameObject meleeHitbox;

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
    }

    // Update is called once per frame
    void Update()
    {
        isTouchingUp = Physics2D.OverlapCircle(groundCheckUp.position, groundCheckRadius, groundLayer);
        isTouchingDown = Physics2D.OverlapCircle(groundCheckDown.position, groundCheckRadius, groundLayer);
        isTouchingWall = Physics2D.OverlapCircle(groundCheckWall.position, groundCheckRadius, groundLayer);
        //IdleState();
        //AOEState();
        //if(Input.GetKeyDown(KeyCode.H))
        //{
        	//SpecialState();
        //}
        //FlipTowardsPlayer();
    }

    public void SpecialState(Vector2 playerPosition)
    {
    	playerPosition.Normalize();
    	enemyRB.velocity = playerPosition * attackPlayerSpeed;

    	//make this state end once one of the ground checks is true
    }

    void FlipTowardsPlayer()
    {
    	float playerDirection = Manager.instance.transform.position.x - transform.position.x;
    	if(playerDirection > 0 && facingLeft)
    	{
    		Flip();
    	}
    	else if (playerDirection < 0 && !facingLeft)
    	{
    		Flip();
    	}
    }

    private void OnDrawGizmoSelected()
    {
    	Gizmos.color = Color.cyan;
    	Gizmos.DrawWireSphere(groundCheckUp.position, groundCheckRadius);
    	Gizmos.DrawWireSphere(groundCheckDown.position, groundCheckRadius);
    	Gizmos.DrawWireSphere(groundCheckWall.position, groundCheckRadius);
    }

    public void IdleState()
    {
    	if(isTouchingUp && goingUp)
    	{
    		ChangeDirection();
    	}
    	else if(isTouchingDown && !goingUp)
    	{
    		ChangeDirection();
    	}

    	if(isTouchingWall)
    	{
    		if(facingLeft)
    		{
    			Flip();
    		}
    		else if(!facingLeft)
    		{
    			Flip();
    		}
    	}
    	enemyRB.velocity = idleMoveSpeed * idleMoveDirection;
    }

    public void ShootState()
    {
    	GameObject bullet = Instantiate(shotPrefab, firePoint.position, firePoint.rotation);
    }

   public void MeleeState()
    {
    	meleeHitbox.SetActive(true);
    }

    public void AOEState()
    {
    	if(isTouchingUp && goingUp)
    	{
    		ChangeDirection();
    	}
    	else if(isTouchingDown && !goingUp)
    	{
    		ChangeDirection();
    	}

    	if(isTouchingWall)
    	{
    		if(facingLeft)
    		{
    			Flip();
    		}
    		else if(!facingLeft)
    		{
    			Flip();
    		}
    	}
    	enemyRB.velocity = attackMoveSpeed * attackMoveDirection;
    }

    private void ChangeDirection()
    {
    	goingUp = !goingUp;
    	idleMoveDirection.y *= -1;
    	attackMoveDirection.y *= -1;
    }

    private void Flip()
    {
    	facingLeft = !facingLeft;
    	idleMoveDirection.x *= -1;
    	attackMoveDirection.x *= -1;
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
    		Debug.Log("we are hereeeeeee");
    		if(rand > 3)
    		{
    			Debug.Log("shotting");
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
    	Debug.Log("we should not be here: DecideAttack");
    	return 0;
    }
}
