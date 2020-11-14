using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInteractable : Interactable
{
	public GameObject battleMenu;
	//public GameObject bmScript;
	//BattleMenu bm;

	public void Awake()
	{
		player = Manager.instance.player;
        battleMenu = Manager.instance.battleMenu;
	}


    protected override void Interact()
    {
        if(player.canInteract)
        {
    		battleMenu.SetActive(true);
    		Deck.instance.DrawCards(Deck.instance.drawnCards);
            Manager.instance.NewState(Manager.GameState.MENU);
        }
    }

    void Update()
    {
    }
}
