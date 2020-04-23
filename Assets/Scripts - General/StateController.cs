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


    private IdleAction a_IdleAction;
    private AttackAction a_AttackAction;
    private WalkAction a_WalkAction;
    private AirAction a_AirAction;


    public Animator animator;
    public Animator legAnimator;
    private string[] endTransitions = new string[] {"ToIdle", "ToAttack", "ToWalking", "ToAir"};

    private PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        fsm = new FSM("Player FSM");
        s_IdleState = fsm.AddState("IdleState");
        s_AttackState = fsm.AddState("AttackState");
        s_WalkState = fsm.AddState("WalkState");
        s_AirState = fsm.AddState("AirState");

        a_IdleAction = new IdleAction(s_IdleState);
        a_AttackAction = new AttackAction(s_AttackState);
        a_WalkAction = new WalkAction(s_WalkState);
        a_AirAction = new AirAction(s_AirState);
        //animator = GetComponent<Animator>();

        s_IdleState.AddAction(a_IdleAction);
        s_AttackState.AddAction(a_AttackAction);
        s_WalkState.AddAction(a_WalkAction);
        s_AirState.AddAction(a_AirAction);

        s_IdleState.AddTransition("ToAttack", s_AttackState);
        s_IdleState.AddTransition("ToWalking", s_WalkState);
        s_IdleState.AddTransition("ToAir", s_AirState);

        s_AttackState.AddTransition("ToIdle", s_IdleState);
        s_AttackState.AddTransition("ToWalking", s_WalkState);
        s_AttackState.AddTransition("ToAir", s_AirState);

        s_WalkState.AddTransition("ToIdle", s_IdleState);
        s_WalkState.AddTransition("ToAttack", s_AttackState);
        s_WalkState.AddTransition("ToAir", s_AirState);

        s_AirState.AddTransition("ToIdle", s_IdleState);
        s_AirState.AddTransition("ToAttack", s_AttackState);
        

        a_IdleAction.Init("Idle", endTransitions, animator, legAnimator);
        a_AttackAction.Init("Attacking", "airAttack", endTransitions, animator, legAnimator);
        a_WalkAction.Init("Walking", endTransitions, animator, legAnimator);
        a_AirAction.Init("risingAir", "fallingAir", endTransitions, animator, legAnimator, player);

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
        animator.SetBool("activeAction", StateManager.instance.isActive);
        //animator.SetBool("walking", StateManager.instance.walking);
        //animator.SetInteger("verticalVelocity", (int)player.m_Rigidbody2D.velocity.y);
    }
}
