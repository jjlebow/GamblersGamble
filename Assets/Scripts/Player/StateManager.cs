using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    //handles player states that are unique and can not be shared through the use of enums
    //public PlayerController player;
    

    public bool attacking = false;

    //public bool playerGrounded;
    public bool attackCooldown = false;   //this stops only the player from attacking again.
    public bool isAttacking = false;   //this is the whole period of the attack animation
    public bool jump = false;
    public bool isJumping = false;
    public bool crouch = false;
    public bool cantDamage = false;
    //public bool faceRight = false;
    public bool knockback = false;
    public bool attackContinue = false;
    public bool attackInitiate = false;
    public bool hasLanded = false;
    //public bool cantAttack = false;
    public bool stance = false;
    public bool switchStance = false;
    public bool isStanceChanging = false;   //this represents the duration of the stance changing


    public enum PlayerState{MELEE, SHOOT, DASH, KNOCKBACK, IDLE, DEAD, CANCEL};
    public PlayerState currentState;
    public PlayerState previousState;

    public bool dashDirection = false;





    public static StateManager instance = null;


    public PlayerController player;
    //public bool isActive = false;
    //public bool isShooting = false;
    public bool playerStatic = false;
    public bool walking = true;
    public bool playerGrounded = false;
    public bool faceRight = false;
    //public bool airFalling = false;
    //public bool airRising = false;
    public bool grounded = false;
    public bool charging = false;
    //public bool canDamage = true;

    private void Start()
    {
        
    }

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else if(instance != this)
            Destroy(gameObject);
        currentState = PlayerState.IDLE;

        DontDestroyOnLoad(this.gameObject);
        //animator = player.GetComponent<Animator>();
    }

    public void ChangeState(PlayerState newState)
    {
        previousState = currentState;
        currentState = newState;
    }

    public void RevertState()
    {
        var temp = currentState;
        currentState = previousState;
        previousState = temp;

    }
}
