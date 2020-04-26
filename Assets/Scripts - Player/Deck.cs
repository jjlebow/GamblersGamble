using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//The deck holds the deck of cards, the discard pile, the current drawn cards, and the array of cards in hand
public class Deck : MonoBehaviour
{
	public static Deck instance;
	public List<Card> deckOfCards;
	public List<Card> discardPile;
	public Transform drawnCardsParent;
	public Transform handParent;

	public CardSlot[] drawnCards;
	public CardSlot[] handCards;



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
    	for(int i = 0; i < 10; i++)
    	{
    		deckOfCards.Add(GenerateCard("Attack", "Art/sword_icon", 10));
    		//Debug.Log(deckOfCards[i].name);
    	}
    	handCards = handParent.GetComponentsInChildren<CardSlot>();
    	drawnCards = drawnCardsParent.GetComponentsInChildren<CardSlot>();
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

     public void Shuffle(List<Card> deck)
     {
     	System.Random rand = new System.Random();
     	Card mGO;

     	int n = deck.Count;
     	for(int i = 0; i < n; i++)
     	{
     		int r = i + (int)(rand.NextDouble() * (n - i));
     		mGO = deck[i];
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
     public void DrawCards(ref CardSlot[] m_array)
     {
     	int length = m_array.Length;
     	int deckLength;
     	for(int i = length - 1; i >= 0; i--)
     	{
     		deckLength = deckOfCards.Count - 1;
     		if(deckLength == 0)
     		{
     			Reshuffle();
     			deckLength = deckOfCards.Count;  //must refresh the size variable after we reshuffle
     		}

     		m_array[i].AddCard(deckOfCards[deckLength - 1]);
     		deckOfCards.RemoveAt(deckLength -1);
     		//Debug.Log("index " + deckLength);
     	}
     }

     //this is a helper function to DrawCards that will reshuffle the discard
     //pile and put it into the deck slot (which should be empty whenn this
     //function is called) 
     public void Reshuffle()
     {
     	//Debug.Log("reshuffling");
     	Shuffle(discardPile);
     	deckOfCards.AddRange(discardPile);
     	discardPile.Clear();
     }
}
