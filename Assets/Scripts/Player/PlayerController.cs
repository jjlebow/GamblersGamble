using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System; 
using UnityEngine.InputSystem;
//using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    //[HideInInspector] public Animator playerAnim;

    //initialized private variables that can be edited in the editor
    //[Range(0,1)] [SerializeField] private float crouchSpeed = .36f;
    
    //public bool airControl = false;
    //[SerializeField] public LayerMask whatIsGround;

    //an array of all keycodes for input reference

    

    private bool keyRelease;

    //Calls an assortment of functions on landing
    public delegate void hasLanded();
    public event hasLanded hasLandedCallback;
    //----------------------------------------------
    //checks for ground and ceiling collisions
    [SerializeField] public Transform groundCheck;
    [SerializeField] public Transform ceilingCheck;
    //Radius of the circle that determines if we are grounded or not
    const float groundedRadius = 0.2f;
    //radius of the circle that determinds if the player is touching a ceiling or not
    const float ceilingRadius = 0.2f;
    //this adjusts the length for the raycast that recognizes if grounded or not
    private float raycastMaxDistance = 1.1f; //this needs to change if you change the size of the player
    private Vector2 direction = new Vector2(0,-1);
    //[SerializeField] public Collider2D crouchDisableCollider;
    //----------------------------------------------

    //jump variables
    public float fallMultiplier;
    public float lowJumpMultiplier;
    public int numberOfJumps;
    //[SerializeField] private float gravityScale = 1f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float jumpTimer; //if the jumpTimer is low, there is no variability in jump heights
    public bool jumpPressed = false;
    public bool canDoubleJump = false;
    private bool startTimer = false;
    private float timer;
    public int availJumps;
    private bool jumpSound = true;
    //----------------------------------------------

    //Dash variables
    public int numDash;
    public float dashTimer;
    private float tempTimer;
    public bool isDashing = false;
    //----------------------------------------------
    

    //General Physics variables/movement
    [HideInInspector] public Rigidbody2D m_Rigidbody2D;
    public float runningSpeed = 20f;
    [Range(0, 0.3f)][SerializeField] private float movementSmoothing = 0.05f;
    private Vector3 m_velocity = Vector3.zero;
    private float horizontal = 0f;

    //----------------------------------------------
    
    //EquipMent/Inventory variables
    EquipType equipType;
    //----------------------------------------------


    
    //References to other gameObject scripts
    //----------------------------------------------


    //Knockback variables
    [HideInInspector] public Vector3 knockbackDir;
    private float knockbackDuration = 0.3f; //how long player is knocked back for
    private float timeBtwDamage = 1.5f; //this is the cooldown between which the player can take damage
    //----------------------------------------------

    //Player components
    public Animator torsoAnim;
    public int intendedLayer;
    private Damageable playerDamage;
    //----------------------------------------------

    //Shooting variables
    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject heavyShotImpactEffect;
    public LineRenderer lineRenderer;
    public GameObject cardPrefab;
    //----------------------------------------------


    //attack hitboxes
    public GameObject neutralHitbox;
    public GameObject aerialNeutralHitbox;
    public GameObject heavyHitbox;
    public GameObject aerialHeavyHitbox;
    public GameObject precisionHitbox;
    public GameObject aerialPrecisionHitbox;

    //================================================
    //damage variables
    public int damageHolder;    //damage is applied by taking the damage value off the card (when its initialized). when the card is called, we store the 
                                ///damage value here to make it accessible to other scripts easily, and then apply it to the damage dealt. 
    private KeyCode chargeKey;
    private float chargeTime = 1f;
    private float temp;
    private Collider2D col;
    private bool isTriggered = false;

    public Controls controls;
    Vector2 move;

    bool jump = false;
    [HideInInspector] public bool canInteract = false;


    [SerializeField] bool m_logInput = false;

    public int dashID;
    private bool canDash = true;
    [SerializeField] private float dashCooldown = 3f;

    //[SerializeField] bool m_logInput = false;

    //InputAction myAction = new InputAction(binding: "/*/<button>");

    



    private void Awake()
    {
        controls = new Controls();

        //myAction.performed += (control) => Debug.Log($"Button {control.name} pressed!");
        //unimyAction.Enable();


        controls.PlayerControls.Jump.started += context => jumpPressed = true;
        controls.PlayerControls.Jump.canceled += context => jumpPressed = false;
        controls.PlayerControls.Dash.performed += context => Dash();
        controls.PlayerMovement.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.PlayerMovement.Move.canceled += ctx => move = Vector2.zero;
        controls.PlayerMovement.OpenCards.performed += ctx => OpenCards();
        //controls.PlayerMovement.Attack.performed += ctx => Attack(7);
        //controls.PlayerControls.Interact.performed += ctx => canInteract = true;
        //controls.PlayerControls.Interact.canceled += ctx => canInteract = false;
        
        controls.PlayerInteract.Interact.performed += context => Interact();

        //playerAttacker = GetComponent<PlayerAttack>();
        //playerMove = GetComponent<PlayerMovement>();
        //playerAnim = GetComponent<Animator>();
        //controls = new PlayerControls();
        //controls.Gameplay.Move.performed += ctx => Move(1f);
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        playerDamage = GetComponent<Damageable>();
        timer = jumpTimer;
        temp = chargeTime;




        //fsm = StateManager.instance.fsm;
        
        //hasLandedCallback += playerAttacker.attackCancel;
        hasLandedCallback += jumpReset;
        tempTimer = dashTimer;
        //hasLandedCallback += landingAnimation;
        //hasLandedCallback += playerMove.DashReset;
        DontDestroyOnLoad(gameObject);
        
        
    }

    void OpenCards() // have the arm go up first then delay then have the menu go up
    {
        if(Manager.instance.turnEnd)
        {
            Manager.instance.turnEnd = false;
            Deck.instance.DiscardHand();
            Deck.instance.DrawCards(Deck.instance.drawnCards);
        
            Manager.instance.NewState(Manager.GameState.MENU);
            Time.timeScale = 0f;
            Manager.instance.battleMenu.SetActive(true);
            Manager.instance.battleMenuUI.GetComponent<BattleMenu>().Select();
        }
    }

    void Interact()
    {
        
        if(canInteract && Manager.instance.currentState == Manager.GameState.BATTLE && StateManager.instance.grounded == true)
        {
            Debug.Log("here");
            col.SendMessage("Interact");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        isTriggered = true;
        col = other;
        if(other.gameObject.tag == "Interactable" && StateManager.instance.grounded)
        {
            canInteract = true;
            controls.PlayerControls.Disable();
            controls.PlayerInteract.Enable();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //this is in the even thtat we naturally exit the trigger. contributes to the
        //update function's ontriggerexit
        col = null;
        /*
        if(other.gameObject.tag == "Interactable")
        {
            controls.PlayerControls.Enable();
            controls.PlayerInteract.Disable();
            canInteract = false;
            col = null;
        }
        */
    }

    void OnEnable()
    {
        controls.PlayerControls.Enable();
        controls.PlayerInteract.Enable();
        controls.PlayerMovement.Enable();
    }

    void OnDisable()
    {
        controls.PlayerControls.Disable();
        controls.PlayerInteract.Disable();
        controls.PlayerMovement.Disable();
    }

    public void Start()
    {
        availJumps = numberOfJumps;
        controls.PlayerInteract.Disable();  
    }



    private void Update()
    {

        //OnTriggerExit functionality - inlcudes both cases where we exit the trigger as well 
        //as having the triggered object be destroyed
        if(isTriggered && !col)
        {
            controls.PlayerControls.Enable();
            controls.PlayerInteract.Disable();
            canInteract = false;
            isTriggered = false;
        }

        //the rest of the Update functionality
        
        Move(); 
        if (m_logInput){
         foreach(KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
         {
             if (Input.GetKeyDown(kcode))
                 Debug.Log("KeyCode down: " + kcode);
         }
        }

        //if (m_logInput){
         //foreach(KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
         //{
             //if (Input.GetKeyDown(kcode))
                 //Debug.Log("KeyCode down: " + kcode);
         //}
     //}       
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
        //only runs once per key pressdown regardless of duration held, but multiple keys can be pressed down at once and all are registered

        //this only gets run once every time a key is pressed.
        //if(StateManager.instance.charging)
            //temp -= Time.deltaTime;
        /*if(Input.GetKeyUp(chargeKey) || temp <= 0)
            {
                StateManager.instance.charging = false;
                temp = chargeTime;
            }
        */

        if(Manager.instance.currentState == Manager.GameState.BATTLE && StateManager.instance.playerStatic == false && Input.anyKey)
        {
            CheckKeyInput();

            
/*
            //condition for the grounded jump
            if(Input.GetButtonDown("Jump") && availJumps > 0 && StateManager.instance.grounded)
            {
                m_Rigidbody2D.velocity = new Vector3(m_Rigidbody2D.velocity.x,0,0);
                jumpPressed = true;
                jumpReleased = false;
                availJumps -= 1;
            }
            //condition for the air jump not needed for a constant air jump
            else if(Input.GetButtonDown("Jump") && availJumps > 0 && m_Rigidbody2D.velocity.y <= 0)
            {
                m_Rigidbody2D.velocity = new Vector3(m_Rigidbody2D.velocity.x,0,0);
                jumpPressed = true;
                jumpReleased = false;
                availJumps -= 1;
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
        */
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

    public void Dash()
    {
        if(canDash)
        {
            AudioManager.instance.audioSource.PlayOneShot(AudioManager.instance.dashSound, 1f);
            canDash = false;
            StartCoroutine(DashTimer());      //starts timer for when new dash can be used again
            TurnOffLayers();

            StateManager.instance.ChangeState(StateManager.PlayerState.DASH);
            dashID = LeanTween.move(gameObject, gameObject.transform.TransformPoint(new Vector3(3,0, 0)), 0.5f).setEase(LeanTweenType.easeOutSine).setOnComplete(delegate(){DashFinish();}).id;
            StateManager.instance.playerStatic = true;
        }
    }

    public void DashFinish()
    {
        StateManager.instance.RevertState();
        StateManager.instance.playerStatic = false;
        m_Rigidbody2D.velocity = new Vector3(0,0,0);   //resets player's current acceleration and velocity at the end of a dash
    }

    private IEnumerator DashTimer()
    {
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

/*
    public IEnumerator DashRoutine()
    {
        float temp = dashTimer;
        m_Rigidbody2D.gravityScale = 0f;
        m_Rigidbody2D.velocity = new Vector3(0,0,0);
        StateManager.instance.ChangeState(StateManager.PlayerState.DASH);
        while(temp >= 0)
        {
            if(StateManager.instance.faceRight == true)
            {
                //transform.position = new Vector3((float)transform.position.x + 0.1, transform.position.y, 0);
                m_Rigidbody2D.velocity = new Vector3(25,0,0);
            }
            temp -= Time.deltaTime;
            yield return null;
        }
        StateManager.instance.ChangeState(StateManager.PlayerState.IDLE);
        m_Rigidbody2D.velocity = new Vector3(0,0,0);
        m_Rigidbody2D.gravityScale = 1f;
    }

    public IEnumerator BackDashRoutine()
    {
        
        float temp = dashTimer;
        m_Rigidbody2D.gravityScale = 0f;
        m_Rigidbody2D.velocity = new Vector3(0,0,0);
        StateManager.instance.ChangeState(StateManager.PlayerState.DASH);
        while(temp >= 0)
        {
            if(StateManager.instance.faceRight == true)
            {
                //transform.position = new Vector3((float)transform.position.x - 0.1, transform.position.y, 0);
                m_Rigidbody2D.velocity = new Vector3(-25,0,0);
            }
            temp -= Time.deltaTime;
            yield return null;
        }
        StateManager.instance.ChangeState(StateManager.PlayerState.IDLE);
        m_Rigidbody2D.velocity = new Vector3(0,0,0);
        m_Rigidbody2D.gravityScale = 1f;
    }
    */

    private void FixedUpdate()
    {
        RaycastCheckUpdateGround();

        //Jump Stuff
        if(jumpPressed)
        {
            StartJump();
        }
        if(!jumpPressed)
            StopJump();
    
        if(m_Rigidbody2D.velocity.y < 0)
        {
            m_Rigidbody2D.velocity += (Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime);
        }
        else if(m_Rigidbody2D.velocity.y > 0 && jumpPressed == false)// && !Input.GetButtonDown("Jump"))
        {
            m_Rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }


        //Dash Stuff


    /*
        if(StateManager.instance.currentState == StateManager.PlayerState.DASH)
            {
                if(tempTimer > 0)
                {
                    if(StateManager.instance.faceRight)
                    {
                        m_Rigidbody2D.velocity = new Vector3(20,0,0);
                        tempTimer -= Time.deltaTime;
                    }
                    else
                    {
                        m_Rigidbody2D.velocity = new Vector3(-20,0,0);
                        tempTimer -= Time.deltaTime;
                    }
                }
                else
                {
                    StateManager.instance.RevertState();
                    m_Rigidbody2D.velocity = Vector2.zero;
                    tempTimer = dashTimer;
                    StateManager.instance.playerStatic = false;
                    m_Rigidbody2D.gravityScale = 1f;
                    //Debug.Log("shouldnt be here");
                }   

            }
            else if(StateManager.instance.currentState == StateManager.PlayerState.KNOCKBACK)
            {
                tempTimer = dashTimer;
                m_Rigidbody2D.gravityScale = 1f;
                //Debug.Log("cancelling");
            }
            */

        
    }

    public void Shoot(int damage)
    {
        TurnOffLayers();
        intendedLayer = 0;
        damageHolder = damage;
        StateManager.instance.playerStatic = true;
        StateManager.instance.ChangeState(StateManager.PlayerState.SHOOT);
        //StateManager.instance.isShooting = true;
    }


    public void HeavyShot(int damage)
    {
        TurnOffLayers();
        torsoAnim.SetLayerWeight(1,1);
        intendedLayer = 1;
        damageHolder = damage;
        StateManager.instance.ChangeState(StateManager.PlayerState.SHOOT);
        //StateManager.instance.isShooting = true;
    }

    public void StartHeavyCoroutineShot()
    {
        StartCoroutine(RevolverShot());
    }

    private IEnumerator RevolverShot()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, -firePoint.right);
        if(hitInfo)  
        {
            Transform hitParent;
            //Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
            //if(hitInfo.transform.GetComponent<Boss>() != null)    //if we are colliding with a boss, (if we collide with something else then this returns null)
            //{
                               // make the enemy take damaage
            //}
            //Instantiate(heavyShotImpactEffect, hitInfo.point, Quaternion.identity); //quaternion identity just tells us that we dont need to rotate
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, hitInfo.point);        //if we hit something, then this draws the line up to the thing we hit
            hitParent = PublicFunctions.FindParent(hitInfo.transform);   //ensures that damage is applied to the highest parent of whatever was collided with
            if(hitParent.GetComponent<Damageable>() != null)
            {
                //Might have to code a function in damageable specifically for raycasts
                //hitParent.GetComponent<Damageable>().TakeCollisionDamage(damageHolder, hitInfo.collider.name, this.gameObject);
            }
        }
        else
        {
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, firePoint.position +  (-firePoint.right) * 100);  //taking start position and shifting it 100 units forward?? i dot get this
        }
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(0.02f); // this forces us to wait for this many seconds before turning the line off
        lineRenderer.enabled = false;
    }

    public void PrecisionShot(int damage)
    {
        TurnOffLayers();
        StateManager.instance.charging = true;
        intendedLayer = 2;
        damageHolder = damage;
        StateManager.instance.playerStatic = true;
        m_Rigidbody2D.velocity = new Vector3(0,0,0);
        StateManager.instance.ChangeState(StateManager.PlayerState.SHOOT);
    }


    public void Attack(int damage)
    {
        //Debug.Log("we are going to attack");
        TurnOffLayers();
        intendedLayer = 0;
        damageHolder = damage;
        StateManager.instance.playerStatic = true;
        //StateManager.instance.isActive = true;
        StateManager.instance.ChangeState(StateManager.PlayerState.MELEE);
    }

    public void HeavyAttack(int damage)
    {
        TurnOffLayers();
        //this sets the animation layer heavy to being active to override the default layer.
        torsoAnim.SetLayerWeight(1,1);
        damageHolder = damage;
        intendedLayer = 1;
        StateManager.instance.ChangeState(StateManager.PlayerState.MELEE);
        StateManager.instance.playerStatic = true;
        m_Rigidbody2D.velocity = new Vector3(0,0,0);
        //StateManager.instance.isActive = true;
    }

    public void PrecisionAttack(int damage)
    {
        TurnOffLayers();
        torsoAnim.SetLayerWeight(2,1);
        damageHolder = damage;
        intendedLayer = 2;
        //StateManager.instance.isActive = true;
        StateManager.instance.ChangeState(StateManager.PlayerState.MELEE);
        StateManager.instance.playerStatic = true;
        m_Rigidbody2D.velocity = new Vector3(0,0,0);
    }

    //double jump functions
    public void Jump(int damage)
    {
        TurnOffLayers();
        //if we want to make the double jump a constant jump instead of variable,
        //we need to take the airjump condition out of the update, and put the constant jump force in here
        //delete the extra conditioin in checkKeyInput()
       // m_Rigidbody2D.AddForce(transform.up * 500f);// * StateManager.instance.faceRight);
        availJumps += 1;
    }


    private void TurnOffLayers()
    {
        //before we start an attack, we turn off all non-default layers
        torsoAnim.SetLayerWeight(1,0);
        torsoAnim.SetLayerWeight(2,0);
    }

    //this matches the input with the appropriate action function and executes it. we use the storedKeys array to 
    //avoid looping through the entire keyboard every frame and only looping through the keys that were registered to buttons
    public void CheckKeyInput()
    {
        
        for(int i = 0; i < Deck.instance.handCards.Length; i++)
            {
                if(Deck.instance.handCards[i].card != null)
                {
                    foreach(KeyCode code in Deck.instance.storedKeys)
                    {
                        if(Input.GetKeyDown(code) && code.ToString() == Deck.instance.handCardsBackend[Deck.instance.handCards[i].card].useKey)//Deck.instance.handCards[i].keyCode)
                        {
                            
                            if((code == KeyCode.Space && !StateManager.instance.grounded) || code != KeyCode.Space)//this condition is for the hard coding of the Jump function to make sure we dont call this on our first jump (delete this for other uses of this)
                            //if(code == KeyCode.Space && !StateManager.instance.grounded) this line is used if we are doing the constant double jump
                            {
                                
                                chargeKey = code;
                                this.SendMessage(Deck.instance.handCards[i].card.name, Deck.instance.handCards[i].card.damage);
                                Deck.instance.discardPile.Add(Deck.instance.handCards[i].card);
                                Deck.instance.handCardsBackend[Deck.instance.handCards[i].card].quantity -= 1;
                                Deck.instance.handCards[i].cardQuantity.SetText((Deck.instance.handCardsBackend[Deck.instance.handCards[i].card].quantity).ToString());
                                if(Deck.instance.handCardsBackend[Deck.instance.handCards[i].card].quantity == 0)
                                {
                                    //Deck.instance.handCards[i].cardQuantity.SetText("");
                                    Deck.instance.handCardsBackend.Remove(Deck.instance.handCards[i].card);
                                    Deck.instance.handCards[i].ClearSlot();
                                    Deck.instance.storedKeys.Remove(code);
                                }
                                    
                                Manager.instance.UpdateDeckUI(Deck.instance.discardPile, Deck.instance.discardUI); // this updates the discard UI with the skill that was just used
                                
                                        //foreach(Card card in Deck.instance.discardPile)
                                        //{
                                        //        Debug.Log("Card " + card.name);
                                        //}
                                return;
                            }
                        }
                    }
                }
            }
    }


    



    private void StartJump()
    {
        if(jumpSound && availJumps > 0 && StateManager.instance.grounded)
        {
            AudioManager.instance.audioSource.PlayOneShot(AudioManager.instance.jumpNoise, 1.0f);
            jumpSound = false;
        }
        else if(jumpSound && availJumps > 0 && StateManager.instance.grounded == false)
        {
            AudioManager.instance.audioSource.PlayOneShot(AudioManager.instance.doubleJumpNoise, 1.0f);
            jumpSound = false;
        }
        if(!StateManager.instance.playerStatic && Manager.instance.currentState == Manager.GameState.BATTLE)
        {
            
            if(availJumps > 0 && !StateManager.instance.grounded && canDoubleJump)
            {

                timer = jumpTimer;
                m_Rigidbody2D.velocity = new Vector3(m_Rigidbody2D.velocity.x,0,0);
                availJumps -= 1;
                canDoubleJump = false;
            }

            if(timer > 0 && !canDoubleJump)
            {

                Debug.Log("jump2");
                m_Rigidbody2D.velocity = Vector2.up * jumpForce;
                startTimer = true;
                canDoubleJump = false;
            }
            if(startTimer) 
            {
                //Debug.Log("jump3");
                timer -= Time.deltaTime;
                if(timer <= 0) 
                {
                    jumpPressed = false;
                } 
            }
            
//            horizontal = Input.GetAxisRaw("Horizontal") * runningSpeed;
            //add a grounded check to this when you get that working
            //if(horizontal == 0)
                //StateManager.instance.walking = false;
            //else
                //StateManager.instance.walking = true;


            //Debug.Log(m_Rigidbody2D.gravityScale);
            //Move(horizontal * Time.fixedDeltaTime);
        }
        //m_Rigidbody2D.gravityScale = 1;
        
    }

    private void StopJump()
    {
        if(StateManager.instance.grounded == false)
        {
            canDoubleJump = true;
            jumpSound = true;
        }
        //m_Rigidbody2D.gravityScale = gravityScale;
        ///timer = jumpTimer;
        //startTimer = false;
    }



    private void landingAnimation()
    {
        //anim.SetBool("airRising", false);
        //anim.SetBool("airFalling", false);
        StateManager.instance.hasLanded = true;
    }

    public void TriggerLandEvent()
    {
        if(hasLandedCallback != null)
            hasLandedCallback.Invoke();
    }

     public void jumpReset()
    {
        availJumps = numberOfJumps;
    }

    public void Move()
    {
        //sets movement vector to player input only while in BATTLE state, otherwise sets movement vector to 0,0
        Vector2 m;
        if(Manager.instance.currentState == Manager.GameState.BATTLE && !StateManager.instance.playerStatic)
        {
            m = new Vector2(move.x, move.y) * Time.deltaTime * runningSpeed;
            if(m.x > 0 && StateManager.instance.faceRight == true)
                Flip();
            else if(move.x < 0 && StateManager.instance.faceRight == false)
                Flip();
        }
        else
        {
            m = new Vector2(0, 0);
        }
        transform.Translate(m, Space.World);
        //this is done opposite to what you would expect since the sprites are flipped on the x axis in the editor's sprite renderer.
        
        if(m.x != 0f)
            StateManager.instance.walking = true;
        else
            StateManager.instance.walking = false;
    }
    
    public void OldMove(float move)
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
            if(move > 0 && StateManager.instance.faceRight == false)
            {
                Flip();
                //StateManager.instance.faceRight = true;
            }
            else if(move < 0 && StateManager.instance.faceRight == true)
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
            StateManager.instance.faceRight = !StateManager.instance.faceRight;//1;//StateManager.instance.faceRight * -1;
            transform.Rotate(0f, 180f, 0f);

            //multiplies the players x local scale by -1
            //Vector3 theScale = transform.localScale;
            //theScale.x *= -1;
            //transform.localScale = theScale;
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
        //playerDamage.TakeCollisionDamage(damage, "null");
    }

    //determines how long the player will be in the knockback phase
    public IEnumerator KnockbackTimer()
    {
        float copy = knockbackDuration;
        //StateManager.instance.playerState = StateManager.PlayerStates.HOLD;
        StateManager.instance.knockback = true;
        //StateManager.instance.cantDamage = true;
        while(copy > 0)
        {
            copy -= Time.deltaTime;
            yield return null;
        }
        //StateManager.instance.cantDamage = false;
        StateManager.instance.knockback = false;
        StateManager.instance.ChangeState(StateManager.PlayerState.IDLE);
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
        //this condition catches the exact moment of landing
        if(hit.collider && StateManager.instance.grounded == false)
        {
            //Debug.Log("landed");
            canDoubleJump = false;
            timer = jumpTimer;
            StateManager.instance.grounded = true;
            StateManager.instance.ChangeState(StateManager.PlayerState.CANCEL);
            hasLandedCallback.Invoke();
            StateManager.instance.ChangeState(StateManager.PlayerState.IDLE);
        }
        else if(!hit.collider && StateManager.instance.grounded == true)
        {
            StateManager.instance.grounded = false;
        }
    }
}




