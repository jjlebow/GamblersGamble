using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Manager : MonoBehaviour
{  
    public static Manager instance;
    private PlayerController player;
    private FirstBoss boss;
    public TextMeshProUGUI timerUI;


    public GameObject GameOverPanel;
    public GameObject VictoryPanel;

    public float timer = 10f;

    public int health;
    public Slider healthBar;
    public float timeLeft;
    public bool startTimer = false;

    private void Awake()
    {
        //boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<FirstBoss>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();    
        if(instance != null)
        {
            Debug.LogWarning("More than one instance of inventory found");
            return;
        }
        instance = this;
    }

    private void Start()
    {
        timeLeft = timer;
    }

    private void Update()
    {
        healthBar.value = health;
        //runs the game over function when the player has died, regardless of whether or not the boss has died
        if(StateManager.instance.playerState == StateManager.PlayerStates.DEAD)
        {
            GameOver();
        }
        //else if(boss.boss.isDead && StateManager.instance.playerState != StateManager.PlayerStates.DEAD)
        //{
            //Victory();
        //}
    }
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
        for(int i = 0; i < Deck.instance.handCards.Length; i++)
        {
            if(Deck.instance.handCards[i].cards != null)
            {
                Deck.instance.discardPile.Add(Deck.instance.handCards[i].card);
                Deck.instance.handCards[i].ClearSlot();
            }
        }
    }
}
