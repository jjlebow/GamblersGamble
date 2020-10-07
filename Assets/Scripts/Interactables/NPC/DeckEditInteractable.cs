using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckEditInteractable : Interactable
{
    // Start is called before the first frame update
    void Start()
    {
        player = Manager.instance.player;
    }

    // Update is called once per frame
    void Update()
    {
     		//create a new menu that allows button presses to get rid of cards and also displays cards that are in storage... wait... do we even want storage?
     	if(canInteract && Input.GetKeyDown(KeyCode.E))
    	{
    		Manager.instance.deckEditPanel.SetActive(true);
    		//Deck.instance.DrawCards(Deck.instance.drawnCards);
            Manager.instance.NewState(Manager.GameState.MENU);
    	}   
    }
}
