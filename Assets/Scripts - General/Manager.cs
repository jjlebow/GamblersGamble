using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Manager : MonoBehaviour
{  
    public static Manager instance;
    private FirstBoss boss;
    public TextMeshProUGUI timerUI;




    public GameObject GameOverPanel;
    public GameObject VictoryPanel;

    public float timer = 10f;
    
    public Slider healthBar;
    public Slider badHealthBar;
    public float timeLeft;
    public bool startTimer = false;

    public enum GameState {PAUSED, BATTLE, DIALOGUE, MENU, ACCEPTINPUT, NEUTRAL};
    public GameState currentState;
    public GameState previousState;
    public GameState gameState;
    public PlayerController player;
    public StateController stateMachine;
    //public delegate void OnRoundChange();
    //public OnRoundChange OnRoundChangeCallback;
    private Vector2 startingPoint;
    public Damageable playerDamageable;
    public Boss currentBoss;
    public Damageable bossDamageable;




    //make a delegate and trigger events for onRoundChange so that every time a round ends, 
    //we pause the screen for a set duration, then reset character state to idle, and then
    //reset character position back to startingPoint;

    private void Awake()
    {
        currentState = GameState.BATTLE;
        //boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<FirstBoss>(); 
        if(instance != null)
        {
            Debug.LogWarning("More than one instance of inventory found");
            return;
        }
        instance = this;
        //player = player.GetComponent<PlayerController>();
        //stateMachine = stateMachine.GetComponent<StateController>();
        //playerDamageable = player.GetComponent<Damageable>();
        //currentBoss = currentBoss.GetComponent<Boss>();
        //bossDamageable = currentBoss
        // this should go in some kind of OnScenLoadedthing 
        startingPoint = GameObject.FindGameObjectWithTag("StartingPosition").transform.position;

    }

    private void Start()
    {
        timeLeft = timer;
        
        //OnRoundChangeCallback += PauseScreen;
        //OnRoundChangeCallback += ResetPlayerPosition;
        //OnRoundChangeCallback += ResetPlayerState;
    }

    private void Update()
    {
        //if(gameState.GameState = PAUSED)
            //Time.timeScale = 0;
        healthBar.value = playerDamageable.health;
        badHealthBar.value = bossDamageable.health;
        //runs the game over function when the player has died, regardless of whether or not the boss has died
        if(StateManager.instance.playerState == StateManager.PlayerStates.DEAD)
        {
            GameOver();
        }
        if(Input.GetKeyDown(KeyCode.P) && gameState != GameState.MENU)
        {
            if(gameState != GameState.PAUSED)
            {
                Debug.Log("pausing game");
                Time.timeScale = 0;
                gameState = GameState.PAUSED;
            }
            else if(gameState == GameState.PAUSED)
            {
                Debug.Log("Unpausing game");
                Time.timeScale = 1;
                gameState = GameState.BATTLE;
            }
        }

        //else if(boss.boss.isDead && StateManager.instance.playerState != StateManager.PlayerStates.DEAD)
        //{
            //Victory();
        //}
    }

    public void NewState(GameState newState)
    {
        previousState = currentState;
        currentState = newState;
    }

    public void RevertState()
    {
        var temp = previousState;
        previousState = currentState;
        currentState = temp;
    }

    private void ResetPlayerPosition()
    {
        player.transform.position = startingPoint;
    }
    /*
    private void ResetPlayerState()
    {
        stateMachine.fsm.currentState.SendEvent("IdleAction");
    */

    private void FixedUpdate()
    {

        timerUI.text = timeLeft.ToString();
        if(startTimer == true)
        {
            timeLeft -= Time.deltaTime;
            if(timeLeft <= 0)
            {
                startTimer = false;
                timeLeft = timer;
                RoundEnd();
            }
        }
    }



    private void GameOver()
    {
        //KillPlayer(player);
        GameOverPanel.SetActive(true);
        Time.timeScale = 0;
    } 
    private void Victory()
    {
        //KillBoss(boss);
        VictoryPanel.SetActive(true);
        Time.timeScale = 0;
    }
    public static void KillPlayer(PlayerController player)
    {
        //Destroy(player.gameObject);
        //trigger some kind of animation
        //end the game here
    }
    public static void KillBoss(Boss boss)
    {
        //Destroy(boss.gameObject);
        //play some kind of animation here
        //transition to some kind of win screen now
    }

    public void TimerStart()
    {
        startTimer = true;
    }

    public void RoundEnd()
    {
        StartCoroutine(RoundChange());
        //OnRoundChangeCallback.Invoke();
    }

    private IEnumerator RoundChange()
    {
        float localTime = 3f;
        //do something where the fade in of a black game over screen fades in at some point 
        Time.timeScale = 0;
        while(localTime >= 0)
        {
            localTime -= Time.unscaledDeltaTime;//Time.unscaledTime * 0.001f;
            yield return null;
        }
        ResetPlayerPosition();
        for(int i = 0; i < Deck.instance.handCards.Length; i++)
        {
            if(Deck.instance.handCards[i].card != null)
            {
                Deck.instance.discardPile.Add(Deck.instance.handCards[i].card);
                Deck.instance.handCards[i].ClearSlot();
            }
        }
        Time.timeScale = 1.0f;
        foreach(Card card in Deck.instance.discardPile)
        {
            Debug.Log("Card " + card.name);
        }
        foreach(Card card in Deck.instance.deckOfCards)
        {
            Debug.Log("CardDeck " + card.name);
        }
    }

}
