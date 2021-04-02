using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


//The deck holds the deck of cards, the discard pile, the current drawn cards, and the array of cards in hand
public class Deck : MonoBehaviour
{

	public static Deck instance;

    // public int basicAttack;
    // public int heavyAttack;
    // public int precisionAttack;
    // public int jump;
    // public int dash;
    // public int backDash;
    // public int basicShot;
    // public int heavyShot;
    // public int precisionShot;

    public int drawNumber = 10;

	public List<Card> deckOfCards;
	public List<Card> discardPile;
    public List<Card> shop;
    public List<KeyCode> storedKeys;
    public List<KeyCode> possibleKeys = new List<KeyCode>();   //this list holds all the possible keycodes we can use to attaack with 
	public Transform drawnCardsParent;
	public Transform handParent;

	public CardSlot[] drawnCards;
    public Dictionary<Card, Pair> handCardsBackend = new Dictionary<Card, Pair>();
	public CardSlot[] handCards;
    public readonly Array allKeyCodes = Enum.GetValues(typeof(KeyCode));
    private List<Card> temporaryHoldingCards = new List<Card>();

    private System.Random rand = new System.Random();

    public Transform deckUIParent;
    public Transform discardUIParent;
    public Transform shopDeckUIParent;
    public Transform shopUIParent;
    public Transform deckEditorUIParent;
    [HideInInspector] public CardSlot[] shopUI;
    [HideInInspector] public CardSlot[] deckUI;
    [HideInInspector] public CardSlot[] shopDeckUI;
    [HideInInspector] public CardSlot[] discardUI;
    [HideInInspector] public CardSlot[] deckEditorUI;



    void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("More than one instance of inventory found");
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);


        // deckOfCards = new List<Card>();
        deckOfCards = CardFunctions.GetDefaultCardDeck();
        discardPile = new List<Card>();
        // shop = new List<Card>();
        shop = CardFunctions.GetShopCards();
        


        // for(int i = 0; i < basicAttack; i++)
        // {
        //     deckOfCards.Add(GenerateCard("Attack", "Art/sword_icon", 5, 7,"JoystickButton0", "A"));
        // }
        // for(int i = 0; i < heavyAttack; i ++)
        // {
        //     deckOfCards.Add(GenerateCard("HeavyAttack", "Art/heavyAttack", 5, 10, "", ""));
        // }
        // for(int j = 0; j < jump; j++)
        // {
        //     deckOfCards.Add(GenerateCard("Jump", "Art/double_jump", 5, 0,"", ""));
        // }
        
        // for(int k = 0; k < dash; k++)
        // {
        //     deckOfCards.Add(GenerateCard("Dash", "Art/Dash", 5, 0,"", ""));
        // }
        // for(int l = 0; l < backDash; l++)
        // {
        //     deckOfCards.Add(GenerateCard("BackDash", "Art/BackDash", 5, 0,"", ""));
        // }
        
        // for(int i = 0; i < precisionShot; i++)
        // {
        //     deckOfCards.Add(GenerateCard("PrecisionShot", "Art/precision_shot", 5, 8,"", ""));
        // }
        // for(int i = 0; i < basicShot; i++)
        // {
        //     deckOfCards.Add(GenerateCard("Shoot", "Art/Dart", 5, 5, "JoystickButton2", "D"));
        // }
        // for(int i = 0; i < heavyShot; i++)
        // {
        //     deckOfCards.Add(GenerateCard("HeavyShot", "Art/heavy_shot", 5, 7, "", ""));
        // }

        // for(int i = 0; i < precisionAttack; i++)
        // {
        //     deckOfCards.Add(GenerateCard("PrecisionAttack", "Art/precision_attack", 5, 5, "", ""));
        // }

        // shop.Add(GenerateCard("Attack", "Art/sword_icon", 5, 7,"", ""));
        // shop.Add(GenerateCard("HeavyAttack", "Art/heavyAttack", 5, 10,"", ""));
        // //shop.Add(GenerateCard("Jump", "Art/double_jump", 5, 0,0));
        // shop.Add(GenerateCard("Dash", "Art/Dash", 5, 0,"", ""));
        // //shop.Add(GenerateCard("BackDash", "Art/BackDash", 5, 0,0));
        // shop.Add(GenerateCard("PrecisionShot", "Art/precision_shot", 5, 8,"", ""));
        // shop.Add(GenerateCard("Shoot", "Art/Dart", 5, 5,"", ""));
        // shop.Add(GenerateCard("HeavyShot", "Art/heavy_shot", 5, 7,"", ""));
        // shop.Add(GenerateCard("PrecisionAttack", "Art/precision_attack", 5, 5,"", ""));

     
        // test code serialization
        // Card testCard = GenerateCard("Attack", "Art/sword_icon", 5, 7,1);
        // string testCardJson = JsonUtility.ToJson(testCard);
        // Debug.Log(testCardJson);
        // Card testCard2 = JsonUtility.FromJson<Card>(testCardJson);
        // Debug.Log(testCard2);


        handCards = handParent.GetComponentsInChildren<CardSlot>();
        drawnCards = drawnCardsParent.GetComponentsInChildren<CardSlot>();
        deckUI = deckUIParent.GetComponentsInChildren<CardSlot>();
        shopUI = shopUIParent.GetComponentsInChildren<CardSlot>();
        shopDeckUI = shopDeckUIParent.GetComponentsInChildren<CardSlot>();
        discardUI = discardUIParent.GetComponentsInChildren<CardSlot>();
        deckEditorUI = deckEditorUIParent.GetComponentsInChildren<CardSlot>();
        Shuffle(deckOfCards);

    }

    //instantiate the default deck to have 10 attack cards
    void Start()
    {

    }

    //call this function to generate a card that you want
    public Card GenerateCard(string newName, string newIconPath, int newCost, int newDamage, string newKey, string newAltKey)
    {
    	Card newCard = new Card();
    	newCard.cost = newCost;
    	newCard.name = newName;
    	newCard.icon = Resources.Load<Sprite>(newIconPath) as Sprite;
        newCard.damage = newDamage;
        newCard.bindingKey = newKey;
        newCard.bindingKeyAlt = newAltKey;
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

     public void DiscardHand()
     {
        for(int i = 0; i < handCards.Length; i++)
        {
            if(handCards[i].card != null)
            {
                for(int j = 0; j < handCards[i].quantity; j++)
                {
                    discardPile.Add(handCards[i].card);
                }
            }
            handCards[i].ClearSlot();
        }
        handCardsBackend.Clear();
        //Manager.instance.UpdateDeckUI(Deck.instance.discardPile, Deck.instance.discardUI);
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
     	int deckLength = deckOfCards.Count;
        bool wasDrawn = false;
        for(int j = 0; j < drawNumber; j++)
        {
            wasDrawn = false;
            deckLength = deckOfCards.Count;//we readjust the length variable of the deck
            if(deckLength == 0)  //if the deckLength is zero, then we reshuffle the discaqrd into the deck and continue
            {
                //Debug.Log("reshuffling");
                //Debug.Log("pre length " + discardPile.Count);
                //Debug.Log("current index " + i);
                Reshuffle();
                //Debug.Log("post length " + discardPile.Count);
                deckLength = deckOfCards.Count;  //must refresh the size variable after we reshuffle
            }
            for(int i = length - 1; i >= 0; i--) //for each of the drawnCard slots,
            {
                //Debug.Log(deckLength - 1);
                //Debug.Log(deckOfCards[deckLength - 1].bindingKey);
                if(m_array[i].keyBinding.Contains(deckOfCards[deckLength - 1].bindingKey))
                {
                    if(m_array[i].card == null || m_array[i].card.name == deckOfCards[deckLength - 1].name)
                    {
                        //Debug.Log("we are drawing a card" + deckOfCards.Count);
                        //altkey and bindingkey should be the same after the firs battle, this is just for the sprites to show correctly at the first battle.  
                        if(sceneManagement.instance.isKeyboard)
                            m_array[i].bindIcon.sprite = Resources.Load<Sprite>("ButtonIcons/" + deckOfCards[deckLength - 1].bindingKeyAlt) as Sprite;
                        else
                            m_array[i].bindIcon.sprite = Resources.Load<Sprite>("ButtonIcons/" + deckOfCards[deckLength - 1].bindingKey) as Sprite;
                        m_array[i].quantity += 1;
                        m_array[i].AddCard(deckOfCards[deckLength - 1]);
                        deckOfCards.RemoveAt(deckLength - 1);
                        wasDrawn = true;
                        m_array[i].cardQuantity.SetText((m_array[i].quantity).ToString());
                        break;
                    }
                    
                }
                
            }
            //if we make it here by passing the break statement, this means that the card did not fit in any slots, so we add it to this list. 
            if(wasDrawn == false)
            {
                //Debug.Log("we are here");
                j--; //revert back one so that we count the next card
                temporaryHoldingCards.Add(deckOfCards[deckLength - 1]);
                deckOfCards.RemoveAt(deckLength - 1);
            }            
            
        }
        //re add all temporary discard cards back into the hand here, and then SHUFFLE. 
        if(temporaryHoldingCards.Count != 0)
        {
            deckOfCards.AddRange(temporaryHoldingCards);
            Shuffle(deckOfCards);
            temporaryHoldingCards.Clear();
        }
        Manager.instance.UpdateDeckUI(Deck.instance.deckOfCards, deckUI);
        
     	
     }


     public void Reshuffle()
     {
     	//Debug.Log("reshuffling");
     	
     	deckOfCards.AddRange(discardPile);
        Shuffle(deckOfCards);
     	discardPile.Clear();
        //Debug.Log("we have reshuffled and the lenght of the deck is: " + deckOfCards.Count);
     }

     public void RemoveCard(Card card)
     {
        //int length = deckOfCards.Length;
        deckOfCards.Remove(card);

     }

     public void UpdateHandCardsUI(Card card)
     {
         if(handCardsBackend.ContainsKey(card) == false)
         {
             foreach(KeyValuePair<Card, Pair> entry in handCardsBackend)
             {
            
             }
         }
     }
}
