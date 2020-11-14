using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleMenu : MonoBehaviour
{
    public GameObject battleMenu;
    public PlayerController player;
    private CardSlot[] drawnCards;
    public GameObject firstSelectedButton;
    EventSystem es;

    private void Awake()
    {
        player = player.GetComponent<PlayerController>();
    }

    //Cleanup function that discards any remaining cards in the drawn cards
	public void CloseMenu()
    {
        Time.timeScale = 1f;
        drawnCards = Deck.instance.drawnCards;
        for(int i = 0; i < drawnCards.Length; i++)
        {
            if(drawnCards[i].card != null)
            {
                //Debug.Log(Deck.instance.discardPile);
                //Deck.instance.deckOfCards.Add(drawnCards[i].card);
                Deck.instance.discardPile.Add(drawnCards[i].card);
                drawnCards[i].ClearSlot();
            }
        }
        Manager.instance.UpdateDeckUI(Deck.instance.discardPile, Deck.instance.discardUI);
        battleMenu.SetActive(false);
        Manager.instance.NewState(Manager.GameState.BATTLE);
        //foreach(Card card in Deck.instance.discardPile)
        //{
            //Debug.Log("Card " + card.name);
        //}
    }

    public void StartTimer()
    {
        Manager.instance.TimerStart();
    }

    public void Select()
    {
        es = EventSystem.current;
        es.SetSelectedGameObject(firstSelectedButton);
    }
}
