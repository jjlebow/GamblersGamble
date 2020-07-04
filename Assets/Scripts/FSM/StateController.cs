using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    public FSM fsm;
    private FSMState s_IdleState;
    private FSMState s_AttackState;
    private FSMState s_WalkState;
    private FSMState s_AirState;
    private FSMState s_ShootState;
    private FSMState s_KnockbackState;
    private FSMState s_DashState;
    private FSMState s_DeathState;


    private IdleAction a_IdleAction;
    private AttackAction a_AttackAction;
    private WalkAction a_WalkAction;
    private AirAction a_AirAction;
    private ShootAction a_ShootAction;
    private KnockbackAction a_KnockbackAction;
    private DashAction a_DashAction;
    private DeathAction a_DeathAction;


    public Animator animator;
    public Animator legAnimator;
    private string[] endTransitions = new string[] {"ToIdle", "ToAttack", "ToWalking", "ToAir", "ToShoot", "ToKnockback", "ToDash", "ToDeath"};

    private PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        fsm = new FSM("Player FSM");
        s_IdleState = fsm.AddState("IdleState");
        s_AttackState = fsm.AddState("AttackState");
        s_WalkState = fsm.AddState("WalkState");
        s_AirState = fsm.AddState("AirState");
        s_ShootState = fsm.AddState("ShootState");
        s_KnockbackState = fsm.AddState("KnockbackState");
        s_DashState = fsm.AddState("DashState");
        s_DeathState = fsm.AddState("DeathState");

        a_IdleAction = new IdleAction(s_IdleState);
        a_AttackAction = new AttackAction(s_AttackState);
        a_WalkAction = new WalkAction(s_WalkState);
        a_AirAction = new AirAction(s_AirState);
        a_ShootAction = new ShootAction(s_ShootState);
        a_KnockbackAction = new KnockbackAction(s_KnockbackState);
        a_DashAction = new DashAction(s_DashState);
        a_DeathAction = new DeathAction(s_DeathState);

        s_IdleState.AddAction(a_IdleAction);
        s_AttackState.AddAction(a_AttackAction);
        s_WalkState.AddAction(a_WalkAction);
        s_AirState.AddAction(a_AirAction);
        s_ShootState.AddAction(a_ShootAction);
        s_KnockbackState.AddAction(a_KnockbackAction);
        s_DashState.AddAction(a_DashAction);
        s_DeathState.AddAction(a_DeathAction);

        s_IdleState.AddTransition("ToAttack", s_AttackState);
        s_IdleState.AddTransition("ToWalking", s_WalkState);
        s_IdleState.AddTransition("ToAir", s_AirState);
        s_IdleState.AddTransition("ToShoot", s_ShootState);
        s_IdleState.AddTransition("ToKnockback", s_KnockbackState);
        s_IdleState.AddTransition("ToDash", s_DashState);
        s_IdleState.AddTransition("ToDeath", s_DeathState);

        s_AttackState.AddTransition("ToIdle", s_IdleState);
        s_AttackState.AddTransition("ToWalking", s_WalkState);
        s_AttackState.AddTransition("ToAir", s_AirState);
        s_AttackState.AddTransition("ToKnockback", s_KnockbackState);
        s_AttackState.AddTransition("ToDash", s_DashState);
        s_AttackState.AddTransition("ToDeath", s_DeathState);
        s_AttackState.AddTransition("ToShoot", s_ShootState);

        s_WalkState.AddTransition("ToIdle", s_IdleState);
        s_WalkState.AddTransition("ToAttack", s_AttackState);
        s_WalkState.AddTransition("ToAir", s_AirState);
        s_WalkState.AddTransition("ToShoot", s_ShootState);
        s_WalkState.AddTransition("ToKnockback", s_KnockbackState);
        s_WalkState.AddTransition("ToDash", s_DashState);
        s_WalkState.AddTransition("ToDeath", s_DeathState);

        s_AirState.AddTransition("ToIdle", s_IdleState);
        s_AirState.AddTransition("ToAttack", s_AttackState);
        s_AirState.AddTransition("ToShoot", s_ShootState);
        s_AirState.AddTransition("ToWalking", s_WalkState);
        s_AirState.AddTransition("ToAir", s_AirState);
        s_AirState.AddTransition("ToKnockback", s_KnockbackState);
        s_AirState.AddTransition("ToDash", s_DashState);
        s_AirState.AddTransition("ToDeath", s_DeathState);

        s_ShootState.AddTransition("ToIdle", s_IdleState);
        s_ShootState.AddTransition("ToWalking", s_WalkState);
        s_ShootState.AddTransition("ToAir", s_AirState);
        s_ShootState.AddTransition("ToKnockback", s_KnockbackState);
        s_ShootState.AddTransition("ToDash", s_DashState);
        s_ShootState.AddTransition("ToDeath", s_DeathState);

        s_KnockbackState.AddTransition("ToIdle", s_IdleState);
        s_KnockbackState.AddTransition("ToWalking", s_WalkState);
        s_KnockbackState.AddTransition("ToAir", s_AirState);
        s_KnockbackState.AddTransition("ToAttack", s_AttackState);
        s_KnockbackState.AddTransition("ToShoot", s_ShootState);
        s_KnockbackState.AddTransition("ToDash", s_DashState);
        s_KnockbackState.AddTransition("ToDeath", s_DeathState);

        s_DashState.AddTransition("ToIdle", s_IdleState);
        s_DashState.AddTransition("ToWalking", s_WalkState);
        s_DashState.AddTransition("ToAir", s_AirState);
        s_DashState.AddTransition("ToAttack", s_AttackState);
        s_DashState.AddTransition("ToShoot", s_ShootState);
        s_DashState.AddTransition("ToKnockback", s_KnockbackState);
        s_DashState.AddTransition("ToDeath", s_DeathState);

        s_DeathState.AddTransition("ToIdle", s_IdleState);
        s_DeathState.AddTransition("ToWalking", s_WalkState);
        s_DeathState.AddTransition("ToAir", s_AirState);
        s_DeathState.AddTransition("ToAttack", s_AttackState);
        s_DeathState.AddTransition("ToShoot", s_ShootState);
        s_DeathState.AddTransition("ToKnockback", s_KnockbackState);
        s_DeathState.AddTransition("ToDash", s_DashState);



        a_IdleAction.Init("Idle", endTransitions, animator, legAnimator);
        a_AttackAction.Init("Attacking", "airAttack", endTransitions, animator, legAnimator);
        a_WalkAction.Init("Walking", endTransitions, animator, legAnimator);
        a_AirAction.Init("risingAir", "fallingAir", endTransitions, animator, legAnimator, player);
        a_ShootAction.Init("Shooting", "airShoot", endTransitions, animator, legAnimator);
        a_KnockbackAction.Init("Knockback", endTransitions, animator, legAnimator, player);
        a_DashAction.Init("Dashing", "BackDash", endTransitions, animator, legAnimator);
        a_DeathAction.Init("Death", endTransitions, animator, legAnimator);

        fsm.Start("IdleState");
    }

    public void Awake()
    {
        player = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update() //will need to set bools for legAnimator too when we implement that
    {
        fsm.Update();
        if(StateManager.instance.currentState == StateManager.PlayerState.MELEE || StateManager.instance.currentState == StateManager.PlayerState.SHOOT)
            animator.SetBool("activeAction", true);
        else
            animator.SetBool("activeAction", false);
        animator.SetBool("Charging", StateManager.instance.charging);
        //animator.SetBool("walking", StateManager.instance.walking);
        //animator.SetInteger("verticalVelocity", (int)player.m_Rigidbody2D.velocity.y);
    }
}
