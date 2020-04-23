using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public static Hand instance;
    public delegate void OnCardChange();
    public OnCardChange OnCardChangeCallback;
    public CardSlot[] handCards;
    public Transform handParent;

    void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("More than one instance of inventory found");
            return;
        }
        instance = this;
        
    }

    void Start()
    {
    }
    
    void Update()
    {
    	//whatever key is pressed, send the corresponding message and call the corresponding function
    	//ex: if K is pressed, then we send access whatever card is equipped to K, and 
    	//do gameObject.SendMessage(card.name)  - this will call the appropriate function for 
    	//what we need to do based on the equipped card. all the functions will be in player controller. (or we have
    	//the functions in this file and the update key presses in player controller)
    }

    public List<Card> cards = new List<Card>();

    public void Add(Card card)
    {
    	if(OnCardChangeCallback != null)
    		OnCardChangeCallback.Invoke();
    }

    public void Remove(Card card)
    {
    	if(OnCardChangeCallback != null)
    		OnCardChangeCallback.Invoke();
    	cards.Remove(card);
    }
}
