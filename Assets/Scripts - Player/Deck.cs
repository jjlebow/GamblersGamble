using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


//The deck holds the deck of cards, the discard pile, the current drawn cards, and the array of cards in hand
public class Deck : MonoBehaviour
{
	public static Deck instance;
	public List<Card> deckOfCards;
	public List<Card> discardPile;
    public List<KeyCode> storedKeys;
	public Transform drawnCardsParent;
	public Transform handParent;

	public CardSlot[] drawnCards;
	public CardSlot[] handCards;
    public readonly Array allKeyCodes = Enum.GetValues(typeof(KeyCode));

    private System.Random rand = new System.Random();



    void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("More than one instance of inventory found");
            return;
        }
        instance = this;
    }

    //instantiate the default deck to have 10 attack cards
    void Start()
    {
    	deckOfCards = new List<Card>();
    	discardPile = new List<Card>();
    	for(int i = 0; i < 5; i++)
    	{
            deckOfCards.Add(GenerateCard("Attack", "Art/sword_icon", 5));
    	}
        for(int i = 0; i < 5; i ++)
        {
            deckOfCards.Add(GenerateCard("HeavyAttack", "Art/heavyAttack", 5));
        }
        for(int j = 0; j < 5; j++)
        {
            deckOfCards.Add(GenerateCard("Jump", "Art/double_jump", 5));
        }

        for(int i = 0; i < 5; i++)
        {
            deckOfCards.Add(GenerateCard("PrecisionAttack", "Art/precision_attack", 5));
        }
        
        for(int k = 0; k < 5; k++)
        {
            deckOfCards.Add(GenerateCard("Dash", "Art/Dash", 5));
        }
        for(int l = 0; l < 5; l++)
        {
            deckOfCards.Add(GenerateCard("BackDash", "Art/BackDash", 5));
        }
        
    	handCards = handParent.GetComponentsInChildren<CardSlot>();
    	drawnCards = drawnCardsParent.GetComponentsInChildren<CardSlot>();
        Shuffle(deckOfCards);
    }

    //call this function to generate a card that you want
    public Card GenerateCard(string newName, string newIconPath, int newCost)
    {
    	Card newCard = new Card();
    	newCard.cost = newCost;
    	newCard.name = newName;
    	newCard.icon = Resources.Load<Sprite>(newIconPath) as Sprite;
    	return newCard;
    }
    //create some kind of default deck values for the deck
    
    ///adds a specific card to the deck 
    //void AddCard() //make a predicate parameter
    //{

    //}

    //removes a card from the deck
    //void RemoveCard() //make a predicate parameter
    //{

    //}

    //this is a helper function to DrawCards that will reshuffle the discard
     //pile and put it into the deck slot (which should be empty whenn this
     //function is called) 
     public void Shuffle(List<Card> deck)
     {
     	int n = deck.Count;
     	for(int i = 0; i < n - 1; i++)
     	{
     		int r = i + rand.Next(n - i);
     		Card mGO = deck[r];
     		deck[r] = deck[i];
     		deck[i] = mGO;
     	}

     	//return deck;
     }





     public void moveToDiscard(int length)
     {
     	discardPile.Add(deckOfCards[length]);
     	deckOfCards.RemoveAt(length);
     }

     //ref will pass by reference to directly modify the array, though this is 
     //redundant in this case since the array is now part of this script.
     //and lists are passed by reference anyways automatically...
     //this function will draw a number of cards from the back of the deck
     //equal to the number of cards to fill a hand of drawnCards. if there are
     //not enough cards to fill that hand, we will reshuffle the discard pile back
     //into the deck and finish drawing.
     public void DrawCards(CardSlot[] m_array)
     {
     	int length = m_array.Length;
     	int deckLength;
     	for(int i = length - 1; i >= 0; i--) //for each of the drawnCard slots,
     	{
     		deckLength = deckOfCards.Count;//we readjust the length variable of the deck
     		if(deckLength == 0)  //if the deckLength is zero, then we 
     		{
                //Debug.Log("reshuffling");
                //Debug.Log("pre length " + discardPile.Count);
                //Debug.Log("current index " + i);
     			Reshuffle();
                //Debug.Log("post length " + discardPile.Count);
     			deckLength = deckOfCards.Count;  //must refresh the size variable after we reshuffle
     		}

     		m_array[i].AddCard(deckOfCards[deckLength - 1]);
     		deckOfCards.RemoveAt(deckLength -1);
     	}
     }


     public void Reshuffle()
     {
     	//Debug.Log("reshuffling");
     	
     	deckOfCards.AddRange(discardPile);
        Shuffle(deckOfCards);
     	discardPile.Clear();
     }
}
