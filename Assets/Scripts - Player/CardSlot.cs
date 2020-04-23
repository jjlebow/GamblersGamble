using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSlot : MonoBehaviour
{
    public Image icon;
    public Card card;

    public void AddCard(Card newCard)
    {
    	//Debug.Log(card);
    	card = newCard;
    	//Debug.Log(card);
    	icon.sprite = newCard.icon;
    	icon.enabled = true;
    }

    //this gets called when updating the programmatic hand
    public void ClearSlot()
    {
    	card = null;

    	icon.sprite = null;
    	icon.enabled = false;
    	///move the card to the discard pile
    }

    

    //loops through your current hand slots and stores the card in the first available slot in your hand
    public void BuyCard()
    {
    	int length = Deck.instance.handCards.Length;
    	for(int i = 0; i < length; i++)
    	{
    		if(card != null && Deck.instance.handCards[i].card == null)
    		{
    			if(Manager.instance.health > this.card.cost)
    			{
    				Manager.instance.health -= this.card.cost;
	    			Deck.instance.handCards[i].AddCard(this.card);
	    			Hand.instance.cards.Add(this.card);
	    			this.ClearSlot();
	    			break;
	    		}
	    		else
	    		{
	    			Debug.Log("card is too expensive");
	    			break;
	    		}
	    	}
	    }
    }

    public void ExitMenu()
    {
    	//cleanup: puts all remaining cards into the discard pile
    }
}
