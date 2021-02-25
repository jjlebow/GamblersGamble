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
	public bool facingLeft = false;
	[HideInInspector] public bool bossActive = false;
	public Transform firePoint;
	public GameObject shotPrefab;
	public int idleAngle;

	private bool deflected = false;

	public GameObject meleeHitbox;

	private Node topNode;

	void Awake()
	{
		Manager.instance.boss = GetComponent<BossController>();
	}

    // Start is called before the first frame update
    void Start()
    {
		ConstructBehaviorTree();
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

	private void ConstructBehaviorTree()
	{
		
	}

    public void SpecialState(Vector2 playerPosition)
    {
    	playerPosition.Normalize();
		if(player.transform.position.x > transform.position.x && facingLeft)
		{
			WallFlip();
		}
		else if(player.transform.position.x < transform.position.x && !facingLeft)
		{
			WallFlip();
		}
    	enemyRB.velocity = playerPosition * attackPlayerSpeed;

    	//make this state end once one of the ground checks is true
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
        ///Debug.Log("boss controller we should be movig");
    	if(isTouchingUp && goingUp)
    	{
    		ChangeIdleDirection();
    	}
    	else if(isTouchingDown && !goingUp)
    	{
    		ChangeIdleDirection();
    	}

    	if(isTouchingWall)
    	{
    		if(facingLeft)
    		{
    			WallFlip();
				//ChangeIdleDirection();
    		}
    		else if(!facingLeft)
    		{
				//ChangeIdleDirection();
    			WallFlip();
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
    			WallFlip();
    		}
    		else if(!facingLeft)
    		{
    			WallFlip();
    		}
    	}
    	enemyRB.velocity = attackMoveSpeed * attackMoveDirection;
    }

    private void ChangeDirection()
    {
    	goingUp = !goingUp;
    	//idleMoveDirection.y *= -1;
		//put some kind of pause and squishing effect here. 
    	attackMoveDirection.y *= -1;
		if(idleMoveDirection.x > 0 && facingLeft)
		{
			CeilingFlip();
			//facingLeft = !facingLeft;
			//transform.Rotate(0,180,0);
		}
		else if(idleMoveDirection.x < 0 && !facingLeft)
		{
			CeilingFlip();
			//facingLeft = !facingLeft;
			//transform.Rotate(0,180,0);
		}
    }

	private void ChangeIdleDirection()
	{
		goingUp = !goingUp;
		float p = Random.Range(-1.0f, 1.0f);
		//idleMoveDirection = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
		idleMoveDirection = new Vector2(p, idleMoveDirection.y * -1);
		attackMoveDirection.y *= -1;

		if(p > 0 && facingLeft)
		{
			CeilingFlip();
			//facingLeft = !facingLeft;
			//transform.Rotate(0,180,0);
		}
		else if(p < 0 && !facingLeft)
		{
			CeilingFlip();
			//facingLeft = !facingLeft;
			//transform.Rotate(0,180,0);
		}

		
	}

	private void CeilingFlip()
	{
		facingLeft = !facingLeft;
		transform.Rotate(0,180,0);
	}

    private void WallFlip()
    {
		//Debug.Log("Flipping " + idleMoveDirection.x);
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

	private void CalculateVertices()
	{
		//float distance = (transform.position - player.transform.position).magnitude;
		//float l = Math.sqrt(Math.pow(distance, 2) + Math.pow(distance, 2) - (2 * distance * distance * Math.Cos(idleAngle)));


	}
}
