using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Card
{
    public string name;
    [NonSerialized] public Sprite icon;
    public int cost;
    public int damage;
    public string bindingKey;
    public string bindingKeyAlt;
    public int suit;
    //public string bindingIcon;

    public Card() {}

    public Card(string name, string iconPath, int cost, int damage, int suit) {
    	this.name = name;
    	this.icon = (Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath(iconPath, typeof(Sprite));
        this.cost = cost;
        this.damage = damage;
        this.suit = suit;
    }

    public Card(Card templateCard) {
        this.name = templateCard.name;
        this.icon = templateCard.icon;
        this.cost = templateCard.cost;
        this.damage = templateCard.damage;
        this.bindingKey = templateCard.bindingKey;
        this.bindingKeyAlt = templateCard.bindingKeyAlt;
        this.suit = templateCard.suit;
    }

    //these two override functions ensure that each card type will be recognized as the same object type in the dictionary of cards
    public override int GetHashCode()
    {
        if (name == null) return 0;
        return name.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        Card other = obj as Card;
        return other != null && other.name == this.name;
    }
}
