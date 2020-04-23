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

     public void Shuffle()
     {
     	System.Random rand = new System.Random();
     	Card mGO;

     	int n = deckOfCards.Count;
     	for(int i = 0; i < n; i++)
     	{
     		int r = i + (int)(rand.NextDouble() * (n - i));
     		mGO = deckOfCards[i];
     		deckOfCards[r] = deckOfCards[i];
     		deckOfCards[i] = mGO;
     	}
     }



     public void moveToDiscard(int length)
     {
     	discardPile.Add(deckOfCards[length]);
     	deckOfCards.RemoveAt(length);
     }


     public void DrawCards(ref CardSlot[] m_array)
     {
     	int length = m_array.Length;
     	//Debug.Log(m_array.Length);
     	for(int i = length - 1; i >= 0; i--)
     	{
     		//Debug.Log(m_array[i]);
     		//Debug.Log(deckOfCards[deckOfCards.Count - 1]);
     		m_array[i].AddCard(deckOfCards[deckOfCards.Count - 1]);
     		//Debug.Log(m_array[i].card);
     		//m_array[i].card = deckOfCards[deckOfCards.Count - 1];
     		//Debug.Log(m_array[i].icon);
     		//m_array[i].icon = deckOfCards[deckOfCards.Count - 1].icon;
     		//moveToDiscard(deckOfCards.Count - 1);
     		deckOfCards.RemoveAt(deckOfCards.Count -1);
     	}
     }
}
