using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopInteractable : Interactable
{
    // Start is called before the first frame update
    void Start()
    {
        player = Manager.instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        //display a new menu type that will show cards available for purchase and shows cards 
        //currently in your deck as well all in the same menu
        if(canInteract && Input.GetKeyDown(KeyCode.E))
    	{
    		Manager.instance.shopPanel.SetActive(true);
    		//Deck.instance.DrawCards(Deck.instance.drawnCards);
            Manager.instance.NewState(Manager.GameState.MENU);
    	} 
    }
}
