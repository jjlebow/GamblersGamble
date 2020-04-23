using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System; 

public class PlayerController : MonoBehaviour
{

    //[HideInInspector] public Animator playerAnim;

    //initialized private variables that can be edited in the editor
    //[Range(0,1)] [SerializeField] private float crouchSpeed = .36f;
    
    //public bool airControl = false;
    //[SerializeField] public LayerMask whatIsGround;
    [SerializeField] public Transform groundCheck;
    [SerializeField] public Transform ceilingCheck;
    //[SerializeField] public Collider2D crouchDisableCollider;

    public delegate void LandedDelegate();
    public event LandedDelegate landedEvent;

    //Jump variables
    [SerializeField] private float gravityScale = 1f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float jumpTimer = 0.5f;
    private bool jumpPressed = false;
    private bool jumpReleased = false;
    private bool startTimer = false;
    private float timer;

    

    //Radius of the circle that determines if we are grounded or not
    const float groundedRadius = 0.2f;
    //radius of the circle that determinds if the player is touching a ceiling or not
    const float ceilingRadius = 0.2f;
    [HideInInspector] public Rigidbody2D m_Rigidbody2D;
    //public Rigidbody2D leg_Rigidbody2D;
    //determines whether the player is facing right or not
    
    EquipType equipType;
    [Range(0, 0.3f)][SerializeField] private float movementSmoothing = 0.05f;
    private Vector3 m_velocity = Vector3.zero;
    private float horizontal = 0f;
    public float runningSpeed = 20f;

    //this adjusts the length for the raycast that recognizes if grounded or not
    private float raycastMaxDistance = 0.32f; //this needs to change if you change the size of the player
    private Vector2 direction = new Vector2(0,-1);

    


    


    //private bool isFalling;

    private float knockbackDuration = 0.3f; //how long player is knocked back for
    private float timeBtwDamage = 1.5f; //this is the cooldown between which the player can take damage
    
    
    

    private PlayerAttack playerAttacker;
    private PlayerMovement playerMove;

    public Boss boss;

    //private Vector3 knockbackDirRight;
    //private Vector3 knockbackDirLeft;
    [HideInInspector]public Vector3 knockbackDir;

    


    //private bool wasCrouching = false;

    private void Awake()
    {
        //playerAttacker = GetComponent<PlayerAttack>();
        //playerMove = GetComponent<PlayerMovement>();
        //playerAnim = GetComponent<Animator>();
        
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        timer = jumpTimer;

        //fsm = StateManager.instance.fsm;
        
        //landedEvent += playerAttacker.attackCancel;
        //landedEvent += playerMove.jumpReset;
        //landedEvent += landingAnimation;
        ////landedEvent += playerMove.DashReset;
        
        
        
    }

    public void Start()
    {
        
    }

    

    private void Update()
    {

        //if(health <= 0)
        //{
            //StateManager.instance.playerState = StateManager.PlayerStates.DEAD;
        //}
        

        //if(StateManager.instance.playerState != StateManager.PlayerStates.HOLD)
        //{
            //if(playerMove.horizontal != 0 && StateManager.instance.playerGrounded)
                //StateManager.instance.playerState = StateManager.PlayerStates.MOVING;
            //else if(playerMove.horizontal == 0 && StateManager.instance.playerGrounded)
                //StateManager.instance.playerState = StateManager.PlayerStates.IDLE;
        //}
        if(Input.GetKey(KeyCode.K) && StateManager.instance.isActive == false)
        {
            CheckKeyK();
        }

        if(Input.GetButtonDown("Jump"))
        {
            jumpPressed = true;
        }
        if(Input.GetButtonUp("Jump"))
        {
            jumpReleased = true;
        }
        if(startTimer) 
        {
            timer -= Time.deltaTime;
            if(timer <= 0) 
            {
                jumpReleased = true;
            } 
        }
        
        //this chain of if statements is used to determine which direction the attack is used in. GetKey is used instead so that we can read 
        //multiple inputs at once
        /*
        if(Input.GetAxisRaw("Vertical") < 0)
        {
            StateManager.instance.directionalFacing = StateManager.Directional.DOWN;
        }
        else if(Input.GetAxisRaw("Vertical") > 0)
        {
            StateManager.instance.directionalFacing = StateManager.Directional.UP;
        }
        else if(Input.GetAxisRaw("Vertical") == 0)
        {
            StateManager.instance.directionalFacing = StateManager.Directional.NEUTRAL;
        }
        if(Input.GetKey(KeyCode.LeftShift) && StateManager.instance.playerState != StateManager.PlayerStates.HOLD && !StateManager.instance.attackCooldown && !StateManager.instance.isStanceChanging)
        {
            StateManager.instance.switchStance = true;
            StateManager.instance.isStanceChanging = true;
        }
        */


        /*
        if(!StateManager.instance.attackCooldown && Input.GetKey(KeyCode.K) && StateManager.instance.playerState != StateManager.PlayerStates.HOLD)
            playerAttacker.Attack();

        if(Input.GetKey(KeyCode.J) && StateManager.instance.playerState != StateManager.PlayerStates.HOLD && StateManager.instance.stance)// this is if you dont want to be able to dodge out of an attack mid attack:  && !StateManager.instance.isAttacking)
            playerMove.BackDash();
        else if(Input.GetKey(KeyCode.J) && StateManager.instance.playerState != StateManager.PlayerStates.HOLD && StateManager.instance.stance == false)
            playerMove.ForwardDash();

        
        //animation triggers
        if(m_Rigidbody2D.velocity.y <= 0 && !StateManager.instance.playerGrounded)
        {
            //anim.SetBool("airRising", false);
            playerAnim.SetBool("airFalling", true);
            playerAnim.SetBool("airRising", false);
        }
        else if(m_Rigidbody2D.velocity.y > 0 && !StateManager.instance.playerGrounded)
        {
            playerAnim.SetBool("airRising", true);
            playerAnim.SetBool("airFalling", false);
            //anim.SetBool("airFalling", false);
        }
        else if((m_Rigidbody2D.velocity.y == 0 && !StateManager.instance.playerGrounded) || StateManager.instance.playerGrounded)
        {
            playerAnim.SetBool("airRising", false);
            playerAnim.SetBool("airFalling", false);
        }


        if(StateManager.instance.playerState == StateManager.PlayerStates.MOVING)
            playerAnim.SetBool("Walking", true);
        else
            playerAnim.SetBool("Walking", false);
        /*
        playerAnim.SetBool("Attacking", StateManager.instance.isAttacking);
        playerAnim.SetBool("attackCooldown", StateManager.instance.attackCooldown);        
        playerAnim.SetBool("Grounded", StateManager.instance.playerGrounded);
        playerAnim.SetBool("AttackContinue", StateManager.instance.attackContinue);
        playerAnim.SetBool("Knockback", StateManager.instance.knockback);
        playerAnim.SetBool("StanceSwitch", StateManager.instance.switchStance);
        playerAnim.SetBool("isStanceChanging", StateManager.instance.isStanceChanging);
        playerAnim.SetBool("inStance", StateManager.instance.stance);
        playerAnim.SetBool("AttackInitiate", StateManager.instance.attackInitiate);
        playerAnim.SetBool("Landing", StateManager.instance.hasLanded);
        playerAnim.SetBool("cantDamage", StateManager.instance.cantDamage);
        playerAnim.SetBool("AttackContinue", StateManager.instance.attackContinue);
        if((StateManager.instance.faceRight && m_Rigidbody2D.velocity.x < 0) || (!StateManager.instance.faceRight && m_Rigidbody2D.velocity.x > 0))
            playerAnim.SetBool("WalkBackwards", true);
        else
            playerAnim.SetBool("WalkBackwards", false);
        */
        //anim.SetBool("Landing", false);    //ensures that landing animation is not true for any frame past the frame that the character lands
    }

    private void FixedUpdate()
    {
        RaycastCheckUpdateGround();
        horizontal = Input.GetAxisRaw("Horizontal") * runningSpeed;
        //add a grounded check to this when you get that working
        if(horizontal == 0)
            StateManager.instance.walking = false;
        else
            StateManager.instance.walking = true;

        if(jumpPressed)
            StartJump();
        if(jumpReleased)
            StopJump();

        Move(horizontal * Time.fixedDeltaTime);
    }


    public void Attack()
    {
        StateManager.instance.isActive = true;
    }


    //try to make a different function for each key that will do different stuff
    //based on what keystroke each card is stored under. 
    private void CheckKeyK()
    {
        //for now i am just going to access the first card in the hand, but in the future,
        //try to access cards specific to this key
        for(int i = 0; i < Deck.instance.handCards.Length; i++)
        {
            if(Deck.instance.handCards[i].card != null)
            {
                Debug.Log("clearing the slot " + i);
                this.SendMessage(Deck.instance.handCards[i].card.name);
                Deck.instance.handCards[i].ClearSlot();
                break;
            }
            //else 
                //Debug.Log("nothing is equipped there");
        }
    }


    private void StartJump()
    {
        m_Rigidbody2D.gravityScale = 0;
        m_Rigidbody2D.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        jumpPressed = false;
        startTimer = true;
    }

    private void StopJump()
    {
        m_Rigidbody2D.gravityScale = gravityScale;
        jumpReleased = false;
        timer = jumpTimer;
        startTimer = false;
    }



    private void landingAnimation()
    {
        //anim.SetBool("airRising", false);
        //anim.SetBool("airFalling", false);
        StateManager.instance.hasLanded = true;
    }

    public void TriggerLandEvent()
    {
        if(landedEvent != null)
            landedEvent();
    }

    
    public void Move(float move)
    {
        //if(!inCooldown)
        //{
            /*
            if(!crouch)
            {
                if(Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, whatIsGround))
                {
                    crouch = true;
                }
            }*/
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_velocity, movementSmoothing);
            //can only control the player if grounded or airControl is on
            //if the input is moving the player to the right and the player is facing left
            if(move > 0 && !StateManager.instance.faceRight)
            {
                Flip();
                //StateManager.instance.faceRight = true;
            }
            else if(move < 0 && StateManager.instance.faceRight)
            {
                Flip();
                //StateManager.instance.faceRight = false;
            }
            
                /*
                if(crouch)
                {
                    if(!wasCrouching)
                    {
                        wasCrouching = true;
                        //OnCrouchEvent.Invoke(true);
                    }

                    //Reduce speed my the crouchSpeed multiplier
                    move *= crouchSpeed;

                    if(crouchDisableCollider != null)
                        crouchDisableCollider.enabled = false;
                }
                else
                {
                    if(crouchDisableCollider != null)
                        crouchDisableCollider.enabled = true;

                    if(wasCrouching)
                    {
                        wasCrouching = false;
                        //OnCrouchEvent.Invoke(false);
                    }
                }*/
                
            
            //This is called when the player jumps and they are grounded
            //if(StateManager.instance.playerGrounded && StateManager.instance.jump)
            //{
                //StateManager.instance.isJumping = true;
                //jumpTimeCounter = jumpTime;
                //Jump(jumpForce);
            //}
            //this is called when the player is not grounded but still has 
            //available jumps(double jump)
            
            //else if(!StateManager.instance.playerGrounded && StateManager.instance.jump && availJumps > 0)
            //{
                //StateManager.instance.isJumping = true;
                //jumpTimeCounter = jumpTime;        
                //controller.m_Rigidbody2D.velocity = new Vector3(controller.m_Rigidbody2D.velocity.x,0,0);
                //Jump((float)(jumpForce * 2));    //more force to the double jump to counterbalance the negative velocity
                //--availJumps;
            //}
            //This is when the jump button is being held down
            //if(StateManager.instance.isJumping)
            //{
                //This confirms that the timer for the jump does not exceed
                //if(jumpTimeCounter > 0)
                //{
                    //Jump(jumpForce);
                    //jumpTimeCounter -= Time.deltaTime;
                //}
                //else
                    //StateManager.instance.isJumping = false;
            //}
        //}
    }
    

    

    

    

    
    
    /*
    public IEnumerator Knockback(float knockDur, float knockbackPwr, Vector3 knockbackDir)
    {
        float timer = 0;
        while(knockDur > timer)
        {
            //knockbackPwr = knockbackPwr *2;
            timer += Time.deltaTime;
            //m_Rigidbody2D.velocity = new Vector3(knockbackDir.x * knockbackPwr, knockbackDir.y * knockbackPwr, knockbackDir.z * knockbackPwr);
            m_Rigidbody2D.AddForce(new Vector3(knockbackDir.x * knockbackPwr, knockbackDir.y * knockbackPwr, transform.position.z));
        }
        yield return 0;
    }
    */
    /*
    public void Knockback()
    {
        Vector3 knockbackDir;
        if(StateManager.instance.faceRight == true)
            knockbackDir = new Vector3(transform.position.x - 100, transform.position.y + 100, transform.position.z);
        else
            knockbackDir = new Vector3(transform.position.x + 100, transform.position.y + 100, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, knockbackDir, Time.deltaTime * 2);
    }
    */

    private void Flip()
    {
        if(!StateManager.instance.attacking) //&& !StateManager.instance.stance)
        {
            //switches the way the player is facing
            StateManager.instance.faceRight = !StateManager.instance.faceRight;
            //multiplies the players x local scale by -1
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }

    

    //this includes the knockback but want to edit it to make the knockback move slower
    public void PlayerDamage(int damage, Vector3 attacker)
    {
        //Vector3 knockbackDir = (m_Rigidbody2D.transform.position - boss.transform.position).normalized;
        //m_Rigidbody2D.velocity = new Vector3(knockbackDir.x * 65, knockbackDir.y * 65, knockbackDir.z * 65);
        //m_Rigidbody2D.AddForce(new Vector3(knockbackDir.x * 5000, knockbackDir.y * 5000, knockbackDir.z * 5000));
        //StartCoroutine(Knockback(5f, 10, knockbackDir));
        //StartCoroutine(Knockback(0.5f));
        //ensures that current velocity does not impact knockback
        m_Rigidbody2D.velocity = new Vector3(0,0,0);
        //determines which direction you will be going in 
        Vector3 oppositeDir = (attacker - m_Rigidbody2D.transform.position).normalized;
        //sets up the angle at which the player will be knocked back, using the direction determined from the above collision data
        if(oppositeDir.x >= 0)
            knockbackDir = new Vector3(transform.position.x - 10, transform.position.y + 12, transform.position.z);
        else
            knockbackDir = new Vector3(transform.position.x + 10, transform.position.y + 12, transform.position.z);
        //the x and y position modifiers are mostly for finding which angle to send the player at during knockback but also affects magnitude
        //knockbackDirRight = new Vector3(transform.position.x - 12, transform.position.y + 10, transform.position.z);
        //knockbackDirLeft = new Vector3(transform.position.x + 12, transform.position.y + 10, transform.position.z);
        StartCoroutine(KnockbackTimer());
        StartCoroutine(DamageTimer());
        Manager.instance.health -= damage;
    }

    //determines how long the player will be in the knockback phase
    private IEnumerator KnockbackTimer()
    {
        float copy = knockbackDuration;
        StateManager.instance.playerState = StateManager.PlayerStates.HOLD;
        StateManager.instance.knockback = true;
        //StateManager.instance.cantDamage = true;
        while(copy > 0)
        {
            copy -= Time.deltaTime;
            yield return null;
        }
        //StateManager.instance.cantDamage = false;
        StateManager.instance.knockback = false;
        StateManager.instance.playerState = StateManager.PlayerStates.IDLE;
    }
    //determines how long the player has invulnerability between attacks
    private IEnumerator DamageTimer()
    {
        float copy = timeBtwDamage;
        StateManager.instance.cantDamage = true;
        while(copy > 0)
        {
            copy -= Time.deltaTime;
            yield return null;
        }
        StateManager.instance.cantDamage = false;
    }

    //this is for creating the raycast given a direction, and returning that raycast
    private RaycastHit2D CheckRaycastGround(Vector2 direction)
    {
        Vector2 startingPosition = new Vector2(transform.position.x, transform.position.y);// + directionOriginOffset);
        //Debug.DrawRay(startingPosition,direction, Color.red, raycastMaxDistance); //this is not accurate
        //the final argument is a layer mask telling the raycast to only pay attention to layer 10 (ground)
        
        return Physics2D.Raycast(startingPosition, direction, raycastMaxDistance, 1 << 10);
    }

    //This function is called in fixed update and constantly checks to see if there is ground
    private void RaycastCheckUpdateGround()
    {
        RaycastHit2D hit = CheckRaycastGround(direction);
        //Debug.DrawRay(transform.position, direction, Color.red);
        if(hit.collider)// && StateManager.instance.grounded == false)
        {
            StateManager.instance.grounded = true;
            //controller.TriggerLandEvent();
        }
        else if(!hit.collider)
        {
            StateManager.instance.grounded = false;
        }
    }
}



