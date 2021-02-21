using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card
{
    public string name;
    public Sprite icon;
    public int cost;
    public int damage;
    public int suit;
    //public string bindingIcon;


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
