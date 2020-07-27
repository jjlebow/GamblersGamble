using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPrototype : MonoBehaviour
{

    private FSM bossFSM;
    private FSMState s_MeleeState;
    private FSMState s_NeutralState;
    private FSMState s_AOEState;
    private FSMState s_SpecialState;
    private FSMState s_MissileState;

    private NeutralAction a_NeutralAction;
    private MeleeAction a_MeleeAction;
    private AOEAction a_AOEAction;
    private SpecialAction a_SpecialAction;
    private MissileAction a_MissileAction;


    private Animator animator;
    private string[] endTransitions = new string[] {"ToNeutral", "ToMelee", "ToRanged", "ToAOE", "ToSpecial", "ToCancel"};
    private BossController boss;

    // Start is called before the first frame update
    void Start()
    {
        
        bossFSM = new FSM("BossAI FSM");

        s_NeutralState = bossFSM.AddState("NeutralState");
        s_MeleeState = bossFSM.AddState("MeleeState");
        s_AOEState = bossFSM.AddState("AOEState");
        s_SpecialState = bossFSM.AddState("SpecialState");
        s_MissileState = bossFSM.AddState("MissileState");

        a_NeutralAction = new NeutralAction(s_NeutralState);
        a_MeleeAction = new MeleeAction(s_MeleeState);
        a_AOEAction = new AOEAction(s_AOEState);
        a_SpecialAction = new SpecialAction(s_SpecialState);
        a_MissileAction = new MissileAction(s_MissileState);


        s_NeutralState.AddAction(a_NeutralAction);
        s_MeleeState.AddAction(a_MeleeAction);
        s_AOEState.AddAction(a_AOEAction);
        s_SpecialState.AddAction(a_SpecialAction);
        s_MissileState.AddAction(a_MissileAction);

        
        s_NeutralState.AddTransition("ToMelee", s_MeleeState);
        s_NeutralState.AddTransition("ToAOE", s_AOEState);
        s_NeutralState.AddTransition("ToSpecial", s_SpecialState);
        s_NeutralState.AddTransition("ToRanged", s_MissileState);


        s_AOEState.AddTransition("ToNeutral", s_NeutralState);

        s_SpecialState.AddTransition("ToNeutral", s_NeutralState);


        s_MeleeState.AddTransition("ToNeutral", s_NeutralState);

        s_MissileState.AddTransition("ToNeutral", s_NeutralState);


        a_NeutralAction.Init("Neutral", endTransitions, animator);
        a_AOEAction.Init("AOE", endTransitions, animator);
        a_SpecialAction.Init("Special", endTransitions, animator);
        a_MeleeAction.Init("Melee", endTransitions, animator);
        a_MissileAction.Init("Shoot", endTransitions, animator);




        
        bossFSM.Start("NeutralState");

    }

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bossFSM.Update();
    }
}
