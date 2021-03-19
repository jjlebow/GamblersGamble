using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class Manager : MonoBehaviour
{  
    public static Manager instance;
    //private FirstBoss boss;
    //public TextMeshProUGUI timerUI;
    public TextMeshProUGUI moneyUI;


    public GameObject GameOverPanel;
    public GameObject VictoryPanel;
    public GameObject deckPanel;
    //public GameObject discardPanel;
    public GameObject deckEditPanel;
    public GameObject shopPanel;
    public GameObject musicPanel;
    public GameObject handPanel;


    public float timerMax;
    public float timer;
    public int money = 50;
    
    public Slider healthBar;
    public Slider badHealthBar;
    public Slider timerUI;
    public bool startTimer = false;

    public bool turnEnd = false;

    public enum GameState {MENU, BATTLE, PAUSED, DIALOGUE, ACCEPTINPUT, NEUTRAL, HUB};
    public GameState currentState;
    public GameState previousState;
    //public GameState gameState;

    

    public PlayerController player;
    public StateController stateMachine;
    //public delegate void OnRoundChange();
    //public OnRoundChange OnRoundChangeCallback;
    [HideInInspector] public Vector2 startingPoint;
    public Damageable playerDamageable;
    [HideInInspector] public Damageable bossDamageable;
    [HideInInspector] public BossController boss;
    public GameObject battleMenuUI;
    public GameObject battleMenu;
    //public GameObject goodHealthPanel;
    //public GameObject badHealthPanel;
    public GameObject healthTimePanel;



    




    //make a delegate and trigger events for onRoundChange so that every time a round ends, 
    //we pause the screen for a set duration, then reset character state to idle, and then
    //reset character position back to startingPoint;

    private void Awake()
    {
        ///gameState = GameState.BATTLE;
        //boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<FirstBoss>(); 5
        if(instance != null)
        {
            Debug.LogWarning("More than one instance of inventory found");
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        
        //UpdateDeckUI(Deck.instance.deckOfCards, Deck.instance.shopDeckUI);

        //player = player.GetComponent<PlayerController>();
        //stateMachine = stateMachine.GetComponent<StateController>();
        //playerDamageable = player.GetComponent<Damageable>();
        //currentBoss = currentBoss.GetComponent<Boss>();
        //bossDamageable = currentBoss
        // this should go in some kind of OnScenLoadedthing 

    }

    private void Start()
    {
        //Debug.Log(Deck.instance.deckOfCards.Count);
        //Debug.Log(Deck.instance.deckUI.Length);
        timer = timerMax;
        timerUI.maxValue = timerMax;
        /*
        for(int i = 0; i < Deck.instance.deckOfCards.Count; i++)
        {
            Deck.instance.deckUI[i].AddCard(Deck.instance.deckOfCards[i]);
            Deck.instance.shopDeckUI[i].AddCard(Deck.instance.deckOfCards[i]);
        }
        */
        for(int i = 0; i < Deck.instance.shop.Count; i++)
        {
            Deck.instance.shopUI[i].AddCard(Deck.instance.shop[i]);
        }

        
        //OnRoundChangeCallback += PauseScreen;
        //OnRoundChangeCallback += ResetPlayerPosition;
        //OnRoundChangeCallback += ResetPlayerState;
    }

    private void Update()
    {
        //if(gameState.GameState = PAUSED)
            //Time.timeScale = 0;
        healthBar.value = playerDamageable.health;
        if(bossDamageable != null)
        {
            badHealthBar.value = bossDamageable.health;
        }
        if(Input.GetKeyDown(KeyCode.JoystickButton8) || Input.GetKeyDown(KeyCode.Tab))
        {
            if(currentState == GameState.BATTLE)
            {
                //DeckUI will update every time cards are drawn (deck panel) or changes to the deck have been made as oopposed to every time the player accesses the deck menu
                //Manager.instance.UpdateDeckUI(Deck.instance.deckOfCards, Deck.instance.deckUI);
                LeanTween.alphaCanvas(deckPanel.GetComponent<CanvasGroup>(), 0.5f, 0.15f);
                //LeanTween.move(Manager.instance.deckPanel, new Vector3(0f, 0f, 0f), 0.1f).setEase(LeanTweenType.easeOutSine);
                deckPanel.SetActive(!deckPanel.activeInHierarchy);
                StateManager.instance.playerStatic = true;
            }
            else if(currentState == GameState.HUB)
            {
                if(!deckEditPanel.activeInHierarchy)
                {
                    LeanTween.alphaCanvas(deckEditPanel.GetComponent<CanvasGroup>(), 1f, 0.5f).setOnComplete(delegate(){Deck.instance.deckEditorUI[0].GetComponentInChildren<Button>().Select();});
                    deckEditPanel.SetActive(true);
                    NewState(GameState.MENU);
                }
            }
            else if(currentState == GameState.MENU && deckEditPanel.activeInHierarchy)
            {
                LeanTween.alphaCanvas(deckEditPanel.GetComponent<CanvasGroup>(), 0f, 0.5f).setOnComplete(delegate(){deckEditPanel.SetActive(!deckEditPanel.activeInHierarchy);});
                RevertState();        
            }
        }
        if(Input.GetKeyUp(KeyCode.JoystickButton8) || Input.GetKeyUp(KeyCode.Tab))
        {
            if(currentState == GameState.BATTLE)
            {
                LeanTween.alphaCanvas(deckPanel.GetComponent<CanvasGroup>(), 0f, 0.15f).setOnComplete(delegate(){deckPanel.SetActive(!deckPanel.activeInHierarchy);});
                StateManager.instance.playerStatic = false;
            }
        }


        
        //badHealthBar.value = bossDamageable.health;
        //runs the game over function when the player has died, regardless of whether or not the boss has died

/*
        if(StateManager.instance.currentState == StateManager.PlayerState.DEAD)
        {
            GameOver();
        }
        if(Input.GetKeyDown(KeyCode.P) && currentState != GameState.MENU)
        {
            if(currentState != GameState.PAUSED)
            {
                Debug.Log("pausing game");
                Time.timeScale = 0;
                NewState(GameState.PAUSED);
            }
            else if(currentState == GameState.PAUSED)
            {
                Debug.Log("Unpausing game");
                Time.timeScale = 1;
                RevertState();
            }
        }
        else if(Input.GetKeyDown(KeyCode.U))
        {
            if(currentState == GameState.BATTLE && turnEnd == true)
            {
                NewState(GameState.MENU);
                turnEnd = false;
                Deck.instance.DiscardHand();
                Deck.instance.DrawCards(Deck.instance.drawnCards);
                Time.timeScale = 0f;
                battleMenu.SetActive(true);
            }
        }
        else if(Input.GetKeyDown(KeyCode.O))
        {
            if(currentState == GameState.BATTLE)
            {
                NewState(GameState.MENU);
                deckPanel.SetActive(true);
            }
            else if(currentState == GameState.MENU && deckPanel.activeSelf)
            {
                RevertState();
                deckPanel.SetActive(false);
            }
        }
        else if(Input.GetKeyDown(KeyCode.Y))
        {
            if(currentState == GameState.BATTLE)
            {
                NewState(GameState.MENU);
                discardPanel.SetActive(true);
            }
            else if(currentState == GameState.MENU && discardPanel.activeSelf)
            {
                RevertState();
                discardPanel.SetActive(false);
            }

        }
*/
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
        StateManager.instance.ChangeState(StateManager.PlayerState.IDLE);
        player.m_Rigidbody2D.velocity = new Vector3(0,0,0);
        player.transform.position = startingPoint;
    }
    /*
    private void ResetPlayerState()
    {
        stateMachine.fsm.currentState.SendEvent("IdleAction");
    */

    private void FixedUpdate()
    {
        moneyUI.text = money.ToString();
        timerUI.value = timer;
        //if(startTimer == true)
        //{
        if(Manager.instance.currentState == Manager.GameState.BATTLE)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                //startTimer = false;
                //turnEnd = true;//RoundEnd();
                Deck.instance.DiscardHand();
                Deck.instance.DrawCards(Deck.instance.handCards);
                timer = timerMax;

            }
        }
        //}
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

    public void TimerStart()
    {
        timer = 0f;
        //maybe play a sound effect here or something and make the bar shimmmer to signify that its ready
        startTimer = true;
    }

    public void UpdateDeckUI(List<Card> cardList, CardSlot[] cardSlotUI)
    {
        List<Card> tempCards;
        tempCards = cardList.OrderBy(c => c.bindingKey).ThenBy(c => c.name).ToList();
        for(int i = 0; i < cardSlotUI.Length; i++)
        {
            cardSlotUI[i].ClearSlot();
        }
        for(int i = 0; i < tempCards.Count; i++)
        {
            cardSlotUI[i].AddCard(tempCards[i]);
            if(sceneManagement.instance.isKeyboard)
                cardSlotUI[i].bindIcon.sprite = Resources.Load<Sprite>("ButtonIcons/" + tempCards[i].bindingKeyAlt) as Sprite;
            else if(!sceneManagement.instance.isKeyboard)
                cardSlotUI[i].bindIcon.sprite = Resources.Load<Sprite>("ButtonIcons/" + tempCards[i].bindingKey) as Sprite;
        }
        //deckUI = deckUI.OrderBy(c => c.card.suit).ThenBy(c => c.card.name).ToArray();
        //OnRoundChangeCallback.Invoke();
    }
    /*
    public void UpdateDiscard()
    {
        List<Card> tempCards;
        tempCards = Deck.instance.discardPile.OrderBy(c => c.suit).ThenBy(c =>c.name).ToList();
        for(int i = 0; i < Deck.instance.discardUI.Length; i++)
        {
            Deck.instance.discardUI[i].ClearSlot();
        }
        for(int i = 0; i < Deck.instance.discardPile.Count; i++)
        {
            Deck.instance.discardUI[i].AddCard(tempCards[i]);
        }
    }
    */
    private IEnumerator RoundChange()
    {
        //float localTime = 3f;
        //do something where the fade in of a black game over screen fades in at some point 
        //Time.timeScale = 0;
        //while(localTime >= 0)
        //{
            //localTime -= Time.unscaledDeltaTime;//Time.unscaledTime * 0.001f;
            yield return null;
        //}
        //ResetPlayerPosition();
        //StateManager.instance.currentState = StateManager.PlayerState.CANCEL;
        
        //Time.timeScale = 1.0f;
        foreach(Card card in Deck.instance.discardPile)
        {
            //Debug.Log("Card " + card.name);
        }
        foreach(Card card in Deck.instance.deckOfCards)
        {
            //Debug.Log("CardDeck " + card.name);
        }
    }

}
