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
		//bm = bmScript.GetComponent<BattleMenu>();
	}


    protected override void Interact()
    {
    	if(canInteract && Input.GetKeyDown(KeyCode.E))
    	{
    		battleMenu.SetActive(true);
    		Deck.instance.DrawCards(ref Deck.instance.drawnCards);
    	}
    }
}
