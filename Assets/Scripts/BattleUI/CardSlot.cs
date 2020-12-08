using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSlot : MonoBehaviour
{
    public Image icon;
    public Card card;
    public string keyCode;
    public Image bindIcon;

    private float timer = 0.2f;

    //public Damageable playerDamage;

    public void AddCard(Card newCard)
    {
    	card = newCard;
    	icon.sprite = newCard.icon;
    	icon.enabled = true;
        if(bindIcon != null)
        {
            Debug.Log("ButtonIcons/" + newCard.bindingIcon);
            bindIcon.sprite = Resources.Load<Sprite>("ButtonIcons/" + newCard.bindingIcon) as Sprite;
        }

    }

    //this gets called when updating the programmatic hand
    public void ClearSlot()
    {
    	card = null;
        keyCode = null;

    	icon.sprite = null;
    	icon.enabled = false;
    	///move the card to the discard pile
    }

    public void PurchaseCard()
    {
        if(Manager.instance.currentState == Manager.GameState.MENU)
        {
            if(Manager.instance.money >= this.card.cost)
            {
                Manager.instance.money -= this.card.cost;
                Deck.instance.deckOfCards.Add(this.card);
                Manager.instance.UpdateDeckUI(Deck.instance.deckOfCards, Deck.instance.shopDeckUI);
                Manager.instance.UpdateDeckUI(Deck.instance.deckOfCards, Deck.instance.deckUI);
            }
            else
            {
                Debug.Log("cant afford this");
            }
        }
    }

    public void TrashCard()
    {
        Debug.Log("trashing card");
        if(Manager.instance.currentState == Manager.GameState.MENU)
        {
            if(Manager.instance.money >= (this.card.cost * 2))
            {
                Manager.instance.money -= (this.card.cost * 2);
                Deck.instance.RemoveCard(this.card);
                Manager.instance.UpdateDeckUI(Deck.instance.deckOfCards, Deck.instance.shopDeckUI);
                Manager.instance.UpdateDeckUI(Deck.instance.deckOfCards, Deck.instance.deckUI);
            }
        }
    }

    
    //Function that gets called when button is pressed
    //loops through your current hand slots and stores the card in the first available slot in your hand
    //called every time you click a card button from the list of drawnCard pile
    public void BuyCard()
    {
        if(Manager.instance.currentState == Manager.GameState.MENU)
        {
    	   int length = Deck.instance.handCards.Length;
    	   for(int i = 0; i < length; i++)
    	   {
    		  if(card != null && Deck.instance.handCards[i].card == null)
    		  {

    		      if(Manager.instance.playerDamageable.health > this.card.cost)
    			  {
                    //hard code in the jump condition because this is always a default to space bar. we must skip all the steps
                    //that assign the action to a key.
                    if(this.card.name == "Jump")
                    {
                        Manager.instance.playerDamageable.TakeDamage(this.card.cost);
                        Deck.instance.storedKeys.Add(KeyCode.Space);
                        Deck.instance.handCards[i].AddCard(this.card);
                        Deck.instance.handCards[i].keyCode = "Space";
                        this.ClearSlot();
                        return;
                    }
                    StartCoroutine(WaitForInput(i));
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
    }

    public void oldBuyCard()
    {
        if(Manager.instance.currentState == Manager.GameState.MENU)
        {
           int length = Deck.instance.handCards.Length;
           for(int i = 0; i < length; i++)
           {
              if(card != null && Deck.instance.handCards[i].card == null)
              {

                  if(Manager.instance.playerDamageable.health > this.card.cost)
                  {

                        Manager.instance.playerDamageable.TakeDamage(this.card.cost);
                        //Deck.instance.storedKeys.Add(KeyCode.Space);
                        Deck.instance.handCards[i].AddCard(this.card);
                        //Deck.instance.handCards[i].keyCode = "Space";
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
    }


    //Helper functions for the BuyCard function that accepts input, identifies it and stores it in the appropriate place

    //figures out the exact input that was pressed and stores it in an array to make for more efficient lookup later
    //helper function to BuyCard(). Accepts the input and stores it in the proper places
    public bool AcceptInput(int i)
    {
            //find a better way to do this
            if(Input.anyKeyDown)// && Input.GetButtown)// && !Input.GetButtonDown("Jump") && !Input.GetButtonDown("Horizontal") && !Input.GetButtonDown("Vertical") && !Input.GetKeyDown(KeyCode.Mouse0) && !Input.GetKeyDown(KeyCode.P) && !Input.GetKeyDown(KeyCode.I)) //&& !Input.GetButtonDown("Jump"))
            {

                foreach (KeyCode keyCode in Deck.instance.allKeyCodes)
                {
                    if(Input.GetKeyDown(keyCode))
                    {
                        Deck.instance.storedKeys.Add(keyCode);
                        Deck.instance.handCards[i].keyCode = keyCode.ToString();
                        Manager.instance.RevertState();
                        return true;
                    }
                }
            }
            return false;
    }

    //waits to receive input before the process of buying the card
    //helper function to BuyCard().  after waiting for input, stores the card in the hand and removes it from drawnCards
    private IEnumerator WaitForInput(int i)
    {
        yield return 1;

        //Debug.Log("waiting for input");
        Manager.instance.NewState(Manager.GameState.ACCEPTINPUT);
        while(!AcceptInput(i))
        {
            yield return null;
        }
        Manager.instance.playerDamageable.TakeDamage(this.card.cost);
        Deck.instance.handCards[i].AddCard(this.card);
        //Hand.instance.cards.Add(this.card);
        this.ClearSlot();
    }
}
